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
      Require(HasCyan(bitmap, 450, 0, 500, 15), "The final peak was not drawn in the scrolled view.");
      track.Flip();
      graphics.Clear(Color.Black);
      track.DrawSamples(graphics, view);
      Require(HasCyan(bitmap, 450, 85, 500, 100), "Editing did not invalidate cached peaks.");
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
      Require(HasCyan(bitmap, 49, 0, 52, 5));
      Require(HasCyan(bitmap, 49, 95, 52, 100));
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

    Check("form layout and viewport integration", () => {
      using var form = new FrmMain();
      var field = typeof(FrmMain).GetField("trackManager", BindingFlags.Instance | BindingFlags.NonPublic);
      using var manager = (TrackManager)field.GetValue(form);
      var createPanel = typeof(FrmMain).GetMethod("CreateTrackPanel", BindingFlags.Instance | BindingFlags.NonPublic);
      var track = new Track(new float[44100]);
      createPanel.Invoke(form, new object[] { track });
      manager.AddTrack(track);
      form.ClientSize = new Size(1000, 700);
      Near(manager.Viewport.Width, track.PanDrawSamples.ClientSize.Width);
      Near(manager.Viewport.Duration, 1);
      Require(track.PanDrawSamples.Height > 0);

      float[] song = new float[180000];
      for (int i = 0; i < song.Length; i++) song[i] = (float)(Math.Sin(i * 0.4) * (0.2 + 0.7 * Math.Abs(Math.Sin(i * 0.0001))));
      var longTrack = new Track(song, 1000);
      createPanel.Invoke(form, new object[] { longTrack });
      manager.AddTrack(longTrack);
      form.ClientSize = new Size(1100, 740);
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

  private static bool HasCyan(Bitmap image, int left, int top, int right, int bottom) {
    for (int x = left; x < right; x++)
      for (int y = top; y < bottom; y++)
        if (image.GetPixel(x, y).ToArgb() == Color.Cyan.ToArgb()) return true;
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
