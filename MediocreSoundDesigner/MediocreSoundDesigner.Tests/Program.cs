using System.Reflection;
using MediocreSoundDesigner;
using NAudio.Wave;

// No test-framework packages required. Run with dotnet run --project MediocreSoundDesigner.Tests.
internal static class Program {
  private static int passed;

  [STAThread]
  private static void Main() {
    Check("duration-based fit and resize", () => {
      var view = new TimelineViewport();
      view.Configure(7200, 1000);
      Near(view.XAt(7200), 1000);
      view.Configure(7200, 500);
      Near(view.XAt(7200), 500);
      view.Configure(0, 0);
      Require(double.IsFinite(view.SecondsPerPixel) && view.SecondsPerPixel > 0);
    });

    Check("zoom anchor, scrolling, clamping, and playback following", () => {
      var view = new TimelineViewport();
      view.Configure(3600, 1000);
      double center = view.TimeAt(500);
      view.SetZoom(0.8);
      Near(view.TimeAt(500), center);
      view.SetStart(double.MaxValue);
      Near(view.TimeAt(1000), 3600);
      view.SetStart(0);
      Require(view.Follow(3000));
      Require(view.XAt(3000) >= 0 && view.XAt(3000) < 1000);
      view.Configure(10, 1000);
      Require(view.Start >= 0 && view.Start <= view.MaximumStart);
    });

    Check("late waveform and subpixel peaks render before playback", () => {
      var samples = new float[3_600_000]; // One hour at 1 kHz, beyond the old drawing extent.
      samples[^37] = 0.9f;
      using var track = new Track(samples, 1000);
      using var panel = new Panel { Size = new Size(500, 100) };
      track.LinkPanels(panel, null);
      var view = new TimelineViewport();
      view.Configure(track.Duration, 500);
      view.SetZoom(0.85);
      view.SetStart(view.MaximumStart);
      using var bitmap = new Bitmap(500, 100);
      using var graphics = Graphics.FromImage(bitmap);
      graphics.Clear(Color.Black);
      track.DrawSamples(graphics, view);
      Require(HasColor(bitmap, 450, 0, 500, 15, track.WaveformPen.Color), "The final peak was not drawn in the scrolled view.");
      track.Flip();
      graphics.Clear(Color.Black);
      track.DrawSamples(graphics, view);
      Require(HasColor(bitmap, 450, 85, 500, 100, track.WaveformPen.Color), "Editing did not invalidate cached peaks.");
      track.ReplaceSamples(new float[1000]);
      Near(track.Duration, 1);
    });

    Check("stereo envelope retains peaks in both channels", () => {
      var samples = new float[20000];
      samples[10001] = 1;
      samples[10002] = -1;
      using var track = new Track(samples, 1000, 2);
      using var panel = new Panel { Size = new Size(100, 100) };
      track.LinkPanels(panel, null);
      var view = new TimelineViewport();
      view.Configure(track.Duration, 100);
      using var bitmap = new Bitmap(100, 100);
      using var graphics = Graphics.FromImage(bitmap);
      track.DrawSamples(graphics, view);
      Require(HasColor(bitmap, 49, 0, 52, 5, track.WaveformPen.Color));
      Require(HasColor(bitmap, 49, 95, 52, 100, track.WaveformPen.Color));
    });

    Check("cached peaks match direct scans for arbitrary stereo ranges", () => {
      var random = new Random(91);
      float[] samples = Enumerable.Range(0, 100002).Select(_ => random.NextSingle() * 2 - 1).ToArray();
      Type type = typeof(Track).Assembly.GetType("MediocreSoundDesigner.WaveformPeaks", true);
      object peaks = Activator.CreateInstance(type, new object[] { samples, 2 });
      MethodInfo query = type.GetMethod("Query");
      for (int i = 0; i < 200; i++) {
        int first = random.Next(0, samples.Length / 2);
        int end = random.Next(first + 1, samples.Length / 2 + 1);
        var actual = ((float Min, float Max))query.Invoke(peaks, new object[] { first, end });
        float min = float.PositiveInfinity, max = float.NegativeInfinity;
        for (int s = first * 2; s < end * 2; s++) {
          min = Math.Min(min, samples[s]);
          max = Math.Max(max, samples[s]);
        }
        Near(actual.Min, min);
        Near(actual.Max, max);
      }
    });

    Check("track delay and export are independent of zoom", () => {
      using var panel = new Panel { Width = 1000 };
      using var manager = new TrackManager(panel);
      var samples = Enumerable.Repeat(0.25f, 2205).ToArray();
      var track = new Track(samples, 22050);
      manager.AddTrack(track);
      track.ShiftSampleOver(0.5);
      Near(track.Duration, 0.1);
      Near(manager.Viewport.Duration, 0.6);
      string first = Path.Combine(Path.GetTempPath(), $"timeline-{Guid.NewGuid()}.wav");
      string second = Path.Combine(Path.GetTempPath(), $"timeline-{Guid.NewGuid()}.wav");
      try {
        manager.SaveToFile(first);
        manager.Viewport.SetZoom(0.2);
        manager.Viewport.SetStart(manager.Viewport.MaximumStart);
        manager.SaveToFile(second);
        Require(File.ReadAllBytes(first).SequenceEqual(File.ReadAllBytes(second)));
        using var reader = new AudioFileReader(first);
        Near(reader.TotalTime.TotalSeconds, 0.6, 1.0 / 44100);
        var output = new float[(int)(reader.Length / 4)];
        reader.Read(output.AsSpan());
        Require(output.Take(44100).All(value => value == 0), "Delay padding changed.");
        Require(output.Skip(44100).Any(value => Math.Abs(value) > 0.1));
        manager.RemTrack(track);
        track.Dispose();
        Near(manager.Viewport.Duration, 0);
        Near(manager.Viewport.Start, 0);
        Require(manager.SelectedTrack is null);
      }
      finally {
        File.Delete(first);
        File.Delete(second);
      }
    });

    Check("muting rebuilds active audio and paused edits cannot resume an old mix", () => {
      using var panel = new Panel { Width = 1000 };
      var players = new List<TestWavePlayer>();
      using var manager = new TrackManager(panel, () => {
        var player = new TestWavePlayer();
        players.Add(player);
        return player;
      });
      var first = new Track(Enumerable.Repeat(0.25f, 88200).ToArray());
      var second = new Track(Enumerable.Repeat(0.5f, 88200).ToArray());
      manager.AddTrack(first);
      manager.AddTrack(second);
      manager.PlayAll();
      Near(players[^1].ReadSample(), 0.75);
      players[^1].Advance(0.25);
      double position = manager.PlaybackTime;
      var original = players[^1];
      first.ToggleDisabled();
      Require(original.IsDisposed);
      Require(manager.State == PlaybackState.Playing);
      Near(manager.PlaybackTime, position, 1.0 / 44100);
      Near(players[^1].ReadSample(), 0.5);
      second.ToggleDisabled();
      Require(manager.State == PlaybackState.Playing);
      Near(players[^1].ReadSample(), 0);
      first.ToggleDisabled();
      Near(players[^1].ReadSample(), 0.25);

      manager.PauseAll();
      position = manager.PlaybackTime;
      var paused = players[^1];
      int count = players.Count;
      manager.PlayAll();
      Require(players.Count == count && ReferenceEquals(paused, players[^1]), "Unedited pause should resume the same session.");
      manager.PauseAll();
      second.ToggleDisabled();
      Require(manager.State == PlaybackState.Paused);
      manager.PlayAll();
      Require(paused.IsDisposed && players.Count == count + 1);
      Near(manager.PlaybackTime, position, 1.0 / 44100);
      Near(players[^1].ReadSample(), 0.75);
      count = players.Count;
      manager.Viewport.SetZoom(0.7);
      manager.RefreshTimeline();
      Require(players.Count == count && !players[^1].IsDisposed, "Display changes must not restart playback.");
    });

    Check("deleting all tracks and loading replacement audio resets playback and cursor", () => {
      using var panel = new Panel { Width = 1000 };
      var players = new List<TestWavePlayer>();
      using var manager = new TrackManager(panel, () => {
        var player = new TestWavePlayer();
        players.Add(player);
        return player;
      });
      var old = new Track(Enumerable.Repeat(0.25f, 88200).ToArray());
      manager.AddTrack(old);
      manager.PlayAll();
      players[^1].Advance(0.5);
      manager.PauseAll();
      Require(manager.PlaybackTime > 0);
      manager.Viewport.SetZoom(0.8);
      manager.Viewport.SetStart(manager.Viewport.MaximumStart);
      manager.RemTrack(old);
      old.Dispose();
      Require(players[^1].IsDisposed && manager.State == PlaybackState.Stopped);
      Near(manager.PlaybackTime, 0);
      Near(manager.Viewport.Start, 0);
      manager.PlayAll();
      Require(players.Count == 1, "An empty project must not replay removed audio.");
      manager.AddTrack(new Track(Enumerable.Repeat(-0.5f, 44100).ToArray()));
      Near(manager.PlaybackTime, 0);
      Near(manager.Viewport.XAt(manager.PlaybackTime), 0);
      manager.PlayAll();
      Near(players[^1].ReadSample(), -0.5);
      var previous = players[^1];
      manager.AddTrack(new Track(Enumerable.Repeat(0.125f, 44100).ToArray()));
      Require(previous.IsDisposed && manager.State == PlaybackState.Stopped);
      Near(manager.PlaybackTime, 0);
      manager.PlayAll();
      Near(players[^1].ReadSample(), -0.375);
    });

    Check("reset and end stop playback, seek the viewport, and allow replay", () => {
      using var panel = new Panel { Width = 1000 };
      var players = new List<TestWavePlayer>();
      using var manager = new TrackManager(panel, () => {
        var player = new TestWavePlayer();
        players.Add(player);
        return player;
      });
      manager.AddTrack(new Track(Enumerable.Repeat(0.25f, 88200).ToArray()));
      var muted = new Track(new float[132300]);
      muted.ToggleDisabled();
      manager.AddTrack(muted);
      manager.Viewport.SetZoom(0.7);
      manager.PlayAll();
      players[^1].Advance(0.5);
      var active = players[^1];
      manager.GoToEnd();
      Require(active.IsDisposed && manager.State == PlaybackState.Stopped);
      Near(manager.PlaybackTime, 3); // Muted tracks still contribute to project length.
      Near(manager.Viewport.TimeAt(manager.Viewport.Width), 3);
      manager.PlayAll();
      Near(manager.PlaybackTime, 0);
      Near(manager.Viewport.Start, 0);
      Near(players[^1].ReadSample(), 0.25);
      players[^1].Advance(0.25);
      manager.PauseAll();
      var paused = players[^1];
      manager.ResetToStart();
      Require(paused.IsDisposed && manager.State == PlaybackState.Stopped);
      Near(manager.PlaybackTime, 0);
      Near(manager.Viewport.Start, 0);
      manager.PlayAll();
      Near(players[^1].ReadSample(), 0.25);
      foreach (var track in manager.Tracks.ToArray()) {
        manager.RemTrack(track);
        track.Dispose();
      }
      manager.GoToEnd();
      manager.ResetToStart();
      Near(manager.PlaybackTime, 0);
      Require(manager.State == PlaybackState.Stopped);
    });

    Check("seeking preserves playing, paused, and stopped states and selects the right audio", () => {
      using var panel = new Panel { Width = 1000 };
      var players = new List<TestWavePlayer>();
      using var manager = new TrackManager(panel, () => {
        var player = new TestWavePlayer();
        players.Add(player);
        return player;
      });
      var samples = Enumerable.Repeat(0.25f, 44100)
        .Concat(Enumerable.Repeat(-0.5f, 44100))
        .Concat(Enumerable.Repeat(0.75f, 44100)).ToArray();
      var track = new Track(samples);
      manager.AddTrack(track);
      manager.Seek(1.5);
      Require(manager.State == PlaybackState.Stopped && players.Count == 0);
      Near(manager.PlaybackTime, 1.5);
      manager.PlayAll();
      Near(players[^1].ReadSample(), -0.5);
      var previous = players[^1];
      manager.Seek(0.25);
      Require(manager.State == PlaybackState.Playing && previous.IsDisposed);
      Near(manager.PlaybackTime, 0.25);
      Near(players[^1].ReadSample(), 0.25);
      manager.PauseAll();
      int outputs = players.Count;
      manager.Seek(2.25);
      Require(manager.State == PlaybackState.Paused && players.Count == outputs,
        "Paused seeking must not start an output device.");
      Near(manager.PlaybackTime, 2.25);
      track.ToggleDisabled();
      manager.PlayAll();
      Near(manager.PlaybackTime, 2.25);
      Near(players[^1].ReadSample(), 0);
      track.ToggleDisabled();
      Near(players[^1].ReadSample(), 0.75);
      manager.Seek(-1);
      Near(manager.PlaybackTime, 0);
      Require(manager.State == PlaybackState.Playing);
      manager.Seek(100);
      Near(manager.PlaybackTime, 3);
      Require(manager.State == PlaybackState.Stopped); // Reaching EOF ends playback naturally.
      manager.PlayAll();
      Near(players[^1].ReadSample(), 0.25);
    });

    Check("pointer zoom preserves its anchor and clamps at zoom limits", () => {
      var view = new TimelineViewport();
      view.Configure(3600, 1000);
      double anchorTime = view.TimeAt(173);
      view.SetZoom(0.85, 173);
      Near(view.TimeAt(173), anchorTime);
      view.SetZoom(0.8, 173);
      Near(view.TimeAt(173), anchorTime);
      view.SetZoom(-100, 173);
      Near(view.ZoomFraction, 0);
      view.SetZoom(100, 173);
      Require(view.IsFit);
      Near(view.Start, 0);
    });

    Check("selected flip and reverse preserve outside samples and stereo channels", () => {
      float[] original = Enumerable.Range(1, 10).SelectMany(i => new[] { (float)i, i * 10f }).ToArray();
      using var track = new Track((float[])original.Clone(), 10, 2);
      track.ShiftSampleOver(2);
      track.SetSelection(2.6, 2.2); // Backward drag, accounting for the track offset.
      Require(track.SelectionStartFrame == 2 && track.SelectionEndFrame == 6);
      track.Flip();
      float[] flipped = track.CloneSamples();
      for (int i = 0; i < original.Length; i++) Near(flipped[i], i >= 4 && i < 12 ? -original[i] : original[i]);
      track.Flip();
      track.Reverse();
      float[] reversed = track.CloneSamples();
      for (int frame = 0; frame < 10; frame++)
        for (int channel = 0; channel < 2; channel++) {
          int source = frame >= 2 && frame < 6 ? 7 - frame : frame;
          Near(reversed[frame * 2 + channel], original[source * 2 + channel]);
        }
      track.ShiftSampleOver(1);
      Require(track.SelectionStartFrame == 2 && track.SelectionEndFrame == 6);
      track.ClearSelection();
      track.Flip();
      for (int i = 0; i < original.Length; i++) Near(track.CloneSamples()[i], -reversed[i]);
    });

    Check("region volume uses local gain while preserving whole-track volume", () => {
      using var track = new Track(Enumerable.Repeat(1f, 10).ToArray(), 10);
      track.SetVolume(0.5f);
      track.SetSelection(0.2, 0.5);
      track.SetVolume(2);
      Near(track.Volume, 0.5);
      float[] output = track.GetAdjustedSamples();
      for (int i = 0; i < output.Length; i++) Near(output[i], i >= 2 && i < 5 ? 1 : 0.5);
      track.ReplaceSamples(new float[3]);
      Require(!track.HasSelection);
    });

    Check("silence removal edits only the selected interval and keeps complete frames", () => {
      float[] data = { 1, 10, 2, 20, 0, 0, 0, 0, 3, 30, 0, 0, 4, 40, 5, 50 };
      using var track = new Track(data, 10, 2);
      track.SetSelection(0.2, 0.7);
      track.EliminateSilence(SampleRegion.BEGINNING_ONLY, 0.15f, 0.01f);
      float[] expected = { 1, 10, 2, 20, 3, 30, 0, 0, 4, 40, 5, 50 };
      Require(track.CloneSamples().SequenceEqual(expected));
      Require(track.SelectionStartFrame == 2 && track.SelectionEndFrame == 5);
      Near(track.Duration, 0.6);
      track.EliminateSilence(SampleRegion.ANYWHERE, 0.05f, 0.01f);
      Require(track.CloneSamples().SequenceEqual(new float[] { 1, 10, 2, 20, 3, 30, 4, 40, 5, 50 }));
      Require(track.SelectionEndFrame == 4);
      // One silent channel must not cause half of a stereo frame to be removed.
      using var stereo = new Track(new float[] { 0, 1, 0, 0, 2, 0 }, 10, 2);
      stereo.EliminateSilence(SampleRegion.ANYWHERE, 0.05f, 0.01f);
      Require(stereo.CloneSamples().SequenceEqual(new float[] { 0, 1, 2, 0 }));
      foreach (var mode in new[] { SampleRegion.END_ONLY, SampleRegion.BEGINNING_AND_END }) {
        using var ends = new Track(new float[] { 9, 0, 0, 1, 0, 0, 8 }, 10);
        ends.SetSelection(0.1, 0.6);
        ends.EliminateSilence(mode, 0.2f, 0.01f);
        float[] result = mode == SampleRegion.END_ONLY ? new float[] { 9, 0, 0, 1, 8 } : new float[] { 9, 1, 8 };
        Require(ends.CloneSamples().SequenceEqual(result));
      }
    });

    Check("selection changes do not restart playback, but selected effects update the mix", () => {
      using var panel = new Panel { Width = 1000 };
      var players = new List<TestWavePlayer>();
      using var manager = new TrackManager(panel, () => {
        var player = new TestWavePlayer(); players.Add(player); return player;
      });
      var track = new Track(Enumerable.Repeat(0.25f, 44100).ToArray());
      manager.AddTrack(track);
      manager.PlayAll();
      track.SetSelection(0.2, 0.5);
      Require(players.Count == 1 && !players[0].IsDisposed);
      manager.Seek(0.3);
      track.Flip();
      Near(players[^1].ReadSample(), -0.25);
      manager.Seek(0.7);
      Near(players[^1].ReadSample(), 0.25);
    });

    Check("region deletion preserves outside frames, track offset, and other tracks", () => {
      using var panel = new Panel { Width = 1000 };
      using var manager = new TrackManager(panel);
      float[] original = { 1, 10, 2, 20, 3, 30, 4, 40, 5, 50, 6, 60 };
      var track = new Track((float[])original.Clone(), 10, 2);
      var other = new Track(new float[] { 7, 8 }, 10);
      manager.AddTrack(track);
      manager.AddTrack(other);
      track.ShiftSampleOver(0.3);
      track.DeleteSelection();
      Require(track.CloneSamples().SequenceEqual(original), "No selection must not delete audio implicitly.");
      track.SetSelection(0.7, 0.5);
      track.DeleteSelection();
      Require(track.CloneSamples().SequenceEqual(new float[] { 1, 10, 2, 20, 5, 50, 6, 60 }));
      Require(!track.HasSelection && manager.Tracks.Count == 2);
      Near(track.StartTime, 0.3);
      Near(track.Duration, 0.4);
      Near(manager.Viewport.Duration, 0.7);
      Require(other.CloneSamples().SequenceEqual(new float[] { 7, 8 }));
      track.SetSelection(-100, 100);
      track.DeleteSelection();
      Require(track.CloneSamples().Length == 0 && !track.HasSelection);
      Require(manager.Tracks.Contains(track), "Deleting all selected audio should retain the track itself.");
    });

    Check("clipboard menu copies, cuts, and inserts complete frames", () => {
      using var form = new FrmMain();
      var field = typeof(FrmMain).GetField("trackManager", BindingFlags.Instance | BindingFlags.NonPublic);
      using var manager = (TrackManager)field.GetValue(form);
      var createPanel = typeof(FrmMain).GetMethod("CreateTrackPanel", BindingFlags.Instance | BindingFlags.NonPublic);
      var source = new Track(new float[] { 1, 10, 2, 20, 3, 30 }, 10, 2);
      var destination = new Track(new float[] { 4, 40, 5, 50, 6, 60 }, 10, 2);
      foreach (Track track in new[] { source, destination }) {
        createPanel.Invoke(form, new object[] { track });
        manager.AddTrack(track);
      }
      destination.ShiftSampleOver(0.4);

      var editMenu = (ToolStripMenuItem)form.MainMenuStrip.Items["menuEdit"];
      var cut = (ToolStripMenuItem)editMenu.DropDownItems[0];
      var copy = (ToolStripMenuItem)editMenu.DropDownItems[1];
      var paste = (ToolStripMenuItem)editMenu.DropDownItems[2];
      var pasteNewTrack = (ToolStripMenuItem)editMenu.DropDownItems[3];
      Require(cut.Text == "Cut" && cut.ShortcutKeys == (Keys.Control | Keys.X));
      Require(copy.Text == "Copy" && copy.ShortcutKeys == (Keys.Control | Keys.C));
      Require(paste.Text == "Paste" && paste.ShortcutKeys == (Keys.Control | Keys.V));
      Require(pasteNewTrack.Text == "Paste to New Track" && editMenu.DropDownItems[4].Text == "Delete");

      pasteNewTrack.PerformClick();
      Require(manager.Tracks.Count == 2);
      paste.PerformClick();
      Require(destination.CloneSamples().SequenceEqual(new float[] { 4, 40, 5, 50, 6, 60 }));
      manager.SelectTrack(source);
      source.SetSelection(0.1, 0.2);
      copy.PerformClick();
      Require(source.HasSelection && source.CloneSamples().SequenceEqual(new float[] { 1, 10, 2, 20, 3, 30 }));

      manager.SelectTrack(destination);
      manager.Seek(0.4);
      destination.SetSelection(0.5, 0.6);
      paste.PerformClick();
      Require(destination.CloneSamples().SequenceEqual(new float[] { 4, 40, 2, 20, 5, 50, 6, 60 }));
      Require(!destination.HasSelection);

      manager.SelectTrack(source);
      source.SetSelection(0.2, 0.3);
      cut.PerformClick();
      Require(!source.HasSelection && source.CloneSamples().SequenceEqual(new float[] { 1, 10, 2, 20 }));
      copy.PerformClick(); // An empty selection leaves the clipboard intact.
      manager.SelectTrack(destination);
      manager.Seek(0.5);
      paste.PerformClick();
      Require(destination.CloneSamples().SequenceEqual(new float[] { 4, 40, 3, 30, 2, 20, 5, 50, 6, 60 }));
      Near(destination.StartTime, 0.4);
      Near(manager.Viewport.Duration, 0.9);

      pasteNewTrack.PerformClick();
      Require(manager.Tracks.Count == 3);
      Track pasted = manager.Tracks[^1];
      Require(manager.SelectedTrack == pasted && pasted.PanDrawSamples.Height > 0);
      Require(pasted.SampleRate == source.SampleRate && pasted.Channels == source.Channels);
      Require(pasted.CloneSamples().SequenceEqual(new float[] { 3, 30 }));
      Near(pasted.StartTime, 0);
      pasted.Flip();

      foreach (Track track in manager.Tracks.ToArray()) {
        manager.RemTrack(track);
        track.PanDrawSamples.Dispose();
        track.Dispose();
      }
      Require(manager.SelectedTrack is null);
      pasteNewTrack.PerformClick();
      Require(manager.Tracks.Single().CloneSamples().SequenceEqual(new float[] { 3, 30 }));
    });

    Check("modulation dialogs apply settings only when confirmed", () => {
      (Func<FrmEditModulation> CreateDialog, Action<Track> ApplyDefaults)[] effects = {
        (() => new FrmEditPhaser(), track => track.Phaser()),
        (() => new FrmEditChorus(), track => track.Chorus())
      };
      float[] original = Enumerable.Repeat(0.5f, 100).ToArray();
      foreach (var effect in effects) {
        using var track = new Track((float[])original.Clone(), 1000);
        track.SetSelection(0.01, 0.09);
        using var defaults = new Track((float[])original.Clone(), 1000);
        defaults.SetSelection(0.01, 0.09);
        effect.ApplyDefaults(defaults);

        using var dialog = effect.CreateDialog();
        dialog.SetSound(track);
        var mix = dialog.Controls.OfType<NumericUpDown>().Single(input => input.AccessibleName == "Mix (%)");
        mix.Value = 100;
        dialog.btnCancel_Click(dialog.CancelButton, EventArgs.Empty);
        Require(dialog.DialogResult == DialogResult.Cancel && track.CloneSamples().SequenceEqual(original));

        using var applyDialog = effect.CreateDialog();
        applyDialog.SetSound(track);
        applyDialog.Controls.OfType<NumericUpDown>().Single(input => input.AccessibleName == "Mix (%)").Value = 100;
        applyDialog.btnApply_Click(applyDialog.AcceptButton, EventArgs.Empty);
        float[] edited = track.CloneSamples();
        Require(applyDialog.DialogResult == DialogResult.OK);
        Require(edited[..10].SequenceEqual(original[..10]) && edited[90..].SequenceEqual(original[90..]));
        Require(!edited.SequenceEqual(original) && !edited.SequenceEqual(defaults.CloneSamples()));
      }
    });

    Check("form layout and viewport integration", () => {
      using var form = new FrmMain();
      var field = typeof(FrmMain).GetField("trackManager", BindingFlags.Instance | BindingFlags.NonPublic);
      using var manager = (TrackManager)field.GetValue(form);
      var createPanel = typeof(FrmMain).GetMethod("CreateTrackPanel", BindingFlags.Instance | BindingFlags.NonPublic);
      var track = new Track(new float[44100]);
      createPanel.Invoke(form, new object[] { track });
      manager.AddTrack(track);
      var viewer = track.PanDrawSamples.Parent;
      var viewMenu = (ToolStripMenuItem)form.MainMenuStrip.Items["viewToolStripMenuItem"];
      var stretchTrackHeight = (ToolStripMenuItem)viewMenu.DropDownItems["stretchTrackHeightToolStripMenuItem"];
      Require(!stretchTrackHeight.Checked);
      int startupTrackHeightLimit = viewer.ClientSize.Height / 2;
      form.ClientSize = new Size(1000, form.ClientSize.Height + 100);
      Require(track.PanDrawSamples.Height == startupTrackHeightLimit);
      stretchTrackHeight.PerformClick();
      Require(stretchTrackHeight.Checked && track.PanDrawSamples.Height == viewer.ClientSize.Height - 1);
      form.ClientSize = new Size(1000, 700);
      Require(track.PanDrawSamples.Height == viewer.ClientSize.Height - 1);
      stretchTrackHeight.PerformClick();
      Require(!stretchTrackHeight.Checked);
      Near(manager.Viewport.Width, track.PanDrawSamples.ClientSize.Width);
      Near(manager.Viewport.Duration, 1);
      Require(track.PanDrawSamples.Height == startupTrackHeightLimit);

      float[] song = new float[180000];
      for (int i = 0; i < song.Length; i++) song[i] = (float)(Math.Sin(i * 0.4) * (0.2 + 0.7 * Math.Abs(Math.Sin(i * 0.0001))));
      var longTrack = new Track(song, 1000);
      createPanel.Invoke(form, new object[] { longTrack });
      manager.AddTrack(longTrack);
      form.ClientSize = new Size(1100, 740);
      Require(track.PanDrawSamples.Height == viewer.ClientSize.Height / 2 - 1);
      Require(longTrack.PanDrawSamples.Height == track.PanDrawSamples.Height);
      Require(longTrack.PanDrawSamples.Top == track.PanDrawSamples.Bottom);
      manager.Viewport.SetZoom(0.85);
      // Refresh the controls after changing the model, as the slider handler does.
      typeof(FrmMain).GetMethod("RefreshTimelineView", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(form, null);
      var scroll = (HScrollBar)typeof(FrmMain).GetField("timelineScroll", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(form);
      int lastScrollPosition = scroll.Maximum - scroll.LargeChange + 1;
      typeof(HScrollBar).GetMethod("OnScroll", BindingFlags.Instance | BindingFlags.NonPublic)
        .Invoke(scroll, new object[] { new ScrollEventArgs(ScrollEventType.ThumbPosition, lastScrollPosition) });
      Near(manager.Viewport.Start, manager.Viewport.MaximumStart);
      Require(manager.Viewport.Start > 0);
      Require(scroll.Value == lastScrollPosition && scroll.Enabled);
      var rightClick = new MouseEventArgs(MouseButtons.Right, 1, 200, 50, 0);
      double seekTime = manager.Viewport.TimeAt(200);
      typeof(Control).GetMethod("OnMouseDown", BindingFlags.Instance | BindingFlags.NonPublic)
        .Invoke(longTrack.PanDrawSamples, new object[] { rightClick });
      Near(manager.PlaybackTime, seekTime);
      Require(manager.State == PlaybackState.Stopped && !longTrack.IsDisabled);
      _ = form.Handle;
      _ = longTrack.PanDrawSamples.Parent.Handle;
      _ = longTrack.PanDrawSamples.Handle;
      Point screen = longTrack.PanDrawSamples.PointToScreen(new Point(200, 50));
      long packedPoint = ((long)(ushort)screen.Y << 16) | (ushort)screen.X;
      var wheel = Message.Create(longTrack.PanDrawSamples.Handle, 0x020A, (IntPtr)(120 << 16), (IntPtr)packedPoint);
      double beforeZoom = manager.Viewport.ZoomFraction;
      double anchorTime = manager.Viewport.TimeAt(200);
      Require(form.PreFilterMessage(ref wheel));
      Require(manager.Viewport.ZoomFraction < beforeZoom);
      Near(manager.Viewport.TimeAt(200), anchorTime);
      Near(manager.PlaybackTime, seekTime);
      // The wheel path must synchronize the slider using the regular refresh method.
      var slider = (TrackBar)typeof(FrmMain).GetField("trkDrawTimescale", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(form);
      Near(slider.Value, slider.Minimum + Math.Round(manager.Viewport.ZoomFraction * (slider.Maximum - slider.Minimum)));
      var reset = (Button)typeof(FrmMain).GetField("btnReset", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(form);
      var end = (Button)typeof(FrmMain).GetField("btnEnd", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(form);
      var playhead = (Label)typeof(FrmMain).GetField("lblPlayhead", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(form);
      Image normalEnd = end.BackgroundImage;
      typeof(FrmMain).GetMethod("btnEnd_Click", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(form, new object[] { end, EventArgs.Empty });
      Near(manager.PlaybackTime, 180);
      Require(playhead.Left + playhead.Width <= track.PanDrawSamples.ClientSize.Width);
      Require(!ReferenceEquals(end.BackgroundImage, normalEnd), "End should show its white click icon.");
      typeof(FrmMain).GetMethod("transportFeedback_Tick", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(form, new object[] { form, EventArgs.Empty });
      Require(ReferenceEquals(end.BackgroundImage, normalEnd), "End should return to its red icon.");
      typeof(FrmMain).GetMethod("btnReset_Click", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(form, new object[] { reset, EventArgs.Empty });
      Near(manager.PlaybackTime, 0);
      Near(playhead.Left, 0);
      Require(reset.Top >= track.PanDrawSamples.Parent.Bottom);
      typeof(FrmMain).GetMethod("transportFeedback_Tick", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(form, new object[] { form, EventArgs.Empty });
      var waveform = longTrack.PanDrawSamples;
      var mouseDown = typeof(Control).GetMethod("OnMouseDown", BindingFlags.Instance | BindingFlags.NonPublic);
      var mouseMove = typeof(Control).GetMethod("OnMouseMove", BindingFlags.Instance | BindingFlags.NonPublic);
      var mouseUp = typeof(Control).GetMethod("OnMouseUp", BindingFlags.Instance | BindingFlags.NonPublic);
      mouseDown.Invoke(waveform, new object[] { new MouseEventArgs(MouseButtons.Left, 1, 450, 50, 0) });
      // Drag backwards and vertically beyond the row: capture keeps it on this track.
      mouseMove.Invoke(waveform, new object[] { new MouseEventArgs(MouseButtons.Left, 0, 120, -30, 0) });
      mouseUp.Invoke(waveform, new object[] { new MouseEventArgs(MouseButtons.Left, 1, 120, -30, 0) });
      Require(longTrack.HasSelection && !track.HasSelection && !waveform.Capture);
      Require(manager.SelectedTrack == longTrack);
      Near(longTrack.SelectionStartFrame, Math.Round(manager.Viewport.TimeAt(120) * longTrack.SampleRate));
      Near(longTrack.SelectionEndFrame, Math.Round(manager.Viewport.TimeAt(450) * longTrack.SampleRate));
      int selectedFirst = longTrack.SelectionStartFrame, selectedEnd = longTrack.SelectionEndFrame;
      manager.Viewport.SetZoom(0.8);
      Require(longTrack.SelectionStartFrame == selectedFirst && longTrack.SelectionEndFrame == selectedEnd);
      manager.SelectTrack(track);
      Require(!longTrack.HasSelection);
      manager.SelectTrack(longTrack);
      mouseDown.Invoke(waveform, new object[] { new MouseEventArgs(MouseButtons.Left, 1, 120, 50, 0) });
      mouseMove.Invoke(waveform, new object[] { new MouseEventArgs(MouseButtons.Left, 0, 450, 50, 0) });
      mouseUp.Invoke(waveform, new object[] { new MouseEventArgs(MouseButtons.Left, 1, 450, 50, 0) });
      Require(longTrack.HasSelection);
      using var preview = new Bitmap(form.ClientSize.Width, form.ClientSize.Height);
      using (var graphics = Graphics.FromImage(preview)) graphics.Clear(Color.Black);
      RenderControls(form, preview, Point.Empty);
      preview.Save(Path.Combine(AppContext.BaseDirectory, "timeline-preview.png"));
    });

    Console.WriteLine($"Passed {passed} regression checks.");
  }

  // Exercise the real rendered playback stream without requiring an audio device.
  private sealed class TestWavePlayer : IWavePlayer, IWavePosition {
    private IWaveProvider source;
    private long position;
    public bool IsDisposed { get; private set; }
    public PlaybackState PlaybackState { get; private set; }
    public float Volume { get; set; } = 1;
    public WaveFormat OutputWaveFormat => source.WaveFormat;
    public event EventHandler<StoppedEventArgs> PlaybackStopped;
    public void Init(IWaveProvider waveProvider) => source = waveProvider;
    public void Play() {
      Require(!IsDisposed);
      PlaybackState = PlaybackState.Playing;
    }
    public void Pause() => PlaybackState = PlaybackState.Paused;
    public void Stop() {
      PlaybackState = PlaybackState.Stopped;
      PlaybackStopped?.Invoke(this, new StoppedEventArgs(null));
    }
    public long GetPosition() => position;
    public float ReadSample() {
      Require(PlaybackState == PlaybackState.Playing && !IsDisposed);
      byte[] frame = new byte[OutputWaveFormat.BlockAlign];
      int count = source.Read(frame.AsSpan());
      Require(count == frame.Length);
      position += count;
      return BitConverter.ToSingle(frame);
    }
    public void Advance(double seconds) {
      int remaining = (int)Math.Round(seconds * OutputWaveFormat.SampleRate) * OutputWaveFormat.BlockAlign;
      byte[] buffer = new byte[8192];
      while (remaining > 0) {
        int count = source.Read(buffer.AsSpan(0, Math.Min(buffer.Length, remaining)));
        Require(count > 0);
        remaining -= count;
        position += count;
      }
    }
    public void Dispose() {
      Stop();
      IsDisposed = true;
    }
  }

  private static void RenderControls(Control parent, Bitmap image, Point origin) {
    // Render controls individually so a hidden test form needs no desktop window.
    foreach (Control child in parent.Controls) {
      if (child is Label && child.Name == "lblPlayhead") continue;
      Point location = new(origin.X + child.Left, origin.Y + child.Top);
      if (child.Width <= 0 || child.Height <= 0) continue;
      using var bitmap = new Bitmap(child.Width, child.Height);
      child.DrawToBitmap(bitmap, new Rectangle(Point.Empty, child.Size));
      using (var graphics = Graphics.FromImage(image)) graphics.DrawImageUnscaled(bitmap, location);
      RenderControls(child, image, location);
    }
  }

  private static bool HasColor(Bitmap image, int left, int top, int right, int bottom, Color color) {
    for (int x = left; x < right; x++)
      for (int y = top; y < bottom; y++)
        if (image.GetPixel(x, y).ToArgb() == color.ToArgb()) return true;
    return false;
  }

  private static void Check(string name, Action test) {
    test();
    passed++;
    Console.WriteLine($"PASS {name}");
  }
  private static void Require(bool condition, string message = "Assertion failed.") {
    if (!condition) throw new Exception(message);
  }
  private static void Near(double actual, double expected, double tolerance = 1e-8) =>
    Require(Math.Abs(actual - expected) <= tolerance, $"Expected {expected}, got {actual}.");
}
