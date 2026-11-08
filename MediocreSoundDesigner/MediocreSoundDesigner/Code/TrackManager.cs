using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace MediocreSoundDesigner;

public class TrackManager : IDisposable {
  private Track pseudoCombinedTrack;
  private readonly Panel panContainer;
  private readonly Func<IWavePlayer> createOutput;
  private bool playbackDirty;
  public List<Track> Tracks { get; } = new();
  public Track SelectedTrack { get; private set; }
  public TimelineViewport Viewport { get; } = new();
  public event EventHandler TimelineChanged;
  public PlaybackState State => pseudoCombinedTrack?.State ?? PlaybackState.Stopped;
  public double PlaybackTime => pseudoCombinedTrack?.GetPlaybackTime() ?? 0;

  public TrackManager(Panel panContainer, Func<IWavePlayer> createOutput = null) {
    this.panContainer = panContainer;
    this.createOutput = createOutput ?? (() => new WaveOut());
  }

  public void AddTrack(Track track) {
    ResetPlayback();
    Viewport.SetStart(0);
    Tracks.Add(track);
    track.Changed += TrackChanged;
    RefreshTimeline();
  }

  public void RemTrack(Track track) {
    ResetPlayback();
    Viewport.SetStart(0);
    track.Changed -= TrackChanged;
    Tracks.Remove(track);
    if (SelectedTrack == track) {
      SelectedTrack = null;
      if (Tracks.Count > 0) SelectTrack(Tracks[^1]);
    }
    RefreshTimeline();
  }

  private void TrackChanged(object sender, EventArgs e) {
    if (State == PlaybackState.Playing) {
      double position = PlaybackTime;
      ResetPlayback();
      StartPlayback(position);
    }
    else if (State == PlaybackState.Paused) {
      // Keep the paused cursor, but never resume audio rendered before this edit
      playbackDirty = true;
    }
    else ResetPlayback();
    RefreshTimeline();
  }

  private void ResetPlayback() {
    pseudoCombinedTrack?.Dispose();
    pseudoCombinedTrack = null;
    playbackDirty = false;
  }

  public void RefreshTimeline() {
    double duration = Tracks.Count == 0 ? 0 : Tracks.Max(track => track.StartTime + track.Duration);
    Viewport.Configure(duration, panContainer.ClientSize.Width);
    if (Tracks.Count == 0) Viewport.Fit();
    TimelineChanged?.Invoke(this, EventArgs.Empty);
    RedrawAll();
  }

  public void SelectTrack(Track track) {
    SelectedTrack = track;
    foreach (Track item in Tracks) {
      item.PanDrawSamples.BorderStyle = BorderStyle.None;
      item.PanDrawSamples.BackColor = item == track ? Color.Gray : Color.DimGray;
    }
    track.PanDrawSamples.BackColor = Color.Gray;
  }

  public void RedrawAll() {
    foreach (Track track in Tracks) track.PanDrawSamples?.Invalidate();
  }

  public void SaveToFile(string filename) {
    // Saving must not replace an active playback session.
    using Track combined = CombineTracks();
    combined.SaveToFile(filename);
  }

  public void PlayAll() {
    if (State == PlaybackState.Paused && !playbackDirty) {
      pseudoCombinedTrack.Play();
      return;
    }
    double position = State == PlaybackState.Paused ? PlaybackTime : 0;
    ResetPlayback();
    StartPlayback(position);
  }

  private void StartPlayback(double position) {
    if (Tracks.Count == 0) return;
    pseudoCombinedTrack = CombineTracks();
    if (position >= pseudoCombinedTrack.Duration) {
      ResetPlayback();
      return;
    }
    pseudoCombinedTrack.Play(position, createOutput);
  }

  private Track CombineTracks() {
    const int rate = 44100, channels = 2;
    // Muting changes audio, not project length; even an entirely muted mix can advance.
    double duration = Tracks.Select(track => track.StartTime + track.Duration).DefaultIfEmpty(0).Max();
    int frames = checked((int)Math.Ceiling(duration * rate));
    float[] combined = new float[checked(frames * channels)];
    float[] buffer = new float[8192];
    foreach (Track track in Tracks.Where(track => !track.IsDisabled)) {
      ISampleProvider source = track.CreateSampleProvider();
      if (track.Channels == 1) source = new MonoToStereoSampleProvider(source);
      else if (track.Channels != channels) {
        // Match the stereo output to the first two channels of multichannel audio.
        var stereo = new MultiplexingSampleProvider(new[] { source }, channels);
        stereo.ConnectInputToOutput(0, 0);
        stereo.ConnectInputToOutput(1, 1);
        source = stereo;
      }
      if (track.SampleRate != rate) source = new WdlResamplingSampleProvider(source, rate);
      int position = checked((int)Math.Round(track.StartTime * rate) * channels);
      while (position < combined.Length) {
        int count = source.Read(buffer.AsSpan(0, Math.Min(buffer.Length, combined.Length - position)));
        if (count == 0) break;
        for (int i = 0; i < count; i++) combined[position + i] += buffer[i];
        position += count;
      }
    }
    return new Track(combined, rate, channels);
  }

  public void PauseAll() => pseudoCombinedTrack?.Pause();

  public void Dispose() {
    ResetPlayback();
    foreach (Track track in Tracks) {
      track.Changed -= TrackChanged;
      track.Dispose();
    }
  }
}
