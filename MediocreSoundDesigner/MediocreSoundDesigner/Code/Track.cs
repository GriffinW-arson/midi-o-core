using NAudio.Wave;

namespace MediocreSoundDesigner;

public enum SampleRegion {
  BEGINNING_ONLY,
  END_ONLY,
  BEGINNING_AND_END,
  ANYWHERE,
}

public class Track : IDisposable {
  // Fields
  private string file;
  private TimeSpan duration;
  private float offset;
  private float timeScale;
  private int sampleRate;
  private float[] samples;
  private float pitch;
  private int bits;
  private int channels;
  private AudioFileReader fileReader;
  private IWavePlayer outputDevice;
  private double playbackStartTime;
  private Panel panContainer;
  private WaveformPeaks waveformPeaks;

  // consts
  private static readonly Color SAMPLES_DRAW_COLOR = Color.Cyan;
  private static readonly Color SAMPLES_DRAW_COLOR_DISABLED = Color.LightGray;

  // properties
   
  // start time in seconds
  public double StartTime { get; private set; }
  // calculated duration
  // dur = (number of samples * number of channels) / sample rate
  public double Duration => (double)(samples.Length / channels) / sampleRate;
  public int SampleRate => sampleRate;
  public int Channels => channels;
  public event EventHandler Changed;
  public bool IsDisabled { get; private set; }
  public Panel PanDrawSamples { get; private set; }
  public float Volume { get; private set; }
  private readonly string tempFilePath = Path.Combine(Path.GetTempPath(), "MediocreSoundDesigner", $"{Guid.NewGuid():N}.wav");
  public PlaybackState State => outputDevice?.PlaybackState ?? PlaybackState.Stopped;

  // ctor
  public Track(string file) {
    using var reader = new AudioFileReader(file);
    int sampleCount = checked((int)(reader.Length / sizeof(float)));
    this.samples = new float[sampleCount];
    int samplesRead = 0;
    while (samplesRead < sampleCount) {
      int count = ((ISampleProvider)reader).Read(samples.AsSpan(samplesRead));
      if (count == 0) break;
      samplesRead += count;
    }
    if (samplesRead < sampleCount) {
      Array.Resize(ref samples, samplesRead);
    }

    this.file = file;
    this.duration = reader.TotalTime;
    this.offset = 0;
    this.timeScale = 1.0f;
    this.Volume = 1.0f;
    this.pitch = 1.0f;
    this.bits = reader.WaveFormat.BitsPerSample;
    this.channels = reader.WaveFormat.Channels;
    this.sampleRate = reader.WaveFormat.SampleRate;
    IsDisabled = false;
  }
  public Track(float[] samples, int sampleRate = 44100, int channels = 1) {
    if (sampleRate <= 0 || channels <= 0) throw new ArgumentOutOfRangeException();
    this.samples = samples;

    this.file = "";
    this.duration = TimeSpan.Zero;
    this.offset = 0;
    this.timeScale = 1.0f;
    this.Volume = 1.0f;
    this.pitch = 1.0f;
    this.bits = 32;
    this.channels = channels;
    this.sampleRate = sampleRate;
    IsDisabled = false;
  }
  public void LinkPanels(Panel panDrawSamples, Panel panContainer) {
    this.PanDrawSamples = panDrawSamples;
    this.panContainer = panContainer;
  }

  // edits
  public float[] CloneSamples() {
    return (float[])samples.Clone();
  }
  public void ReplaceSamples(float[] samples) {
    this.samples = samples;
    SamplesChanged();
  }
  private void SamplesChanged() {
    waveformPeaks = null;
    OnChanged();
  }
  private void OnChanged() {
    PanDrawSamples?.Invalidate();
    Changed?.Invoke(this, EventArgs.Empty);
  }
  public void Reverse() {
    for (int i = 0; i < samples.Length / 2; i++) {
      var temp = samples[i];
      samples[i] = samples[samples.Length - 1 - i];
      samples[samples.Length - 1 - i] = temp;
    }
    SamplesChanged();
  }
  public void Flip() {
    for (int i = 0; i < samples.Length; i++) {
      samples[i] *= -1;
    }
    SamplesChanged();
  }
  public void Trim() {
        // write data to/after current byte to 0
        // cur_byte = outputDevice?.GetPosition()
        // for all bytes before/after cur_byte: byte = 0000
    }
    public void AlterTimeScale(float scale) {
        // sample rate times scale?
  }
  public void SetVolume(float newVolume) {
    Volume = newVolume;
    OnChanged();
  }
  public void EliminateSilence(SampleRegion region, float minDuration, float thres) {
    float[] newSamples = CloneSamples();
    if (region is SampleRegion.BEGINNING_ONLY or SampleRegion.BEGINNING_AND_END) {
      int silentDurationInNumSamples = 0;
      for (int i = 0; i < newSamples.Length; i++) {
        if (Math.Abs(newSamples[i]) <= thres) {
          silentDurationInNumSamples++;
        }
        else {
          break;
        }
      }
      if ((float)silentDurationInNumSamples / (sampleRate * channels) >= minDuration) {
        newSamples = newSamples[silentDurationInNumSamples..];
      }
    }
    if (region is SampleRegion.END_ONLY or SampleRegion.BEGINNING_AND_END) {
      int silentDurationInNumSamples = 0;
      for (int i = newSamples.Length - 1; i >= 0; i--) {
        if (Math.Abs(newSamples[i]) <= thres) {
          silentDurationInNumSamples++;
        }
        else {
          break;
        }
      }
      if ((float)silentDurationInNumSamples / (sampleRate * channels) >= minDuration) {
        newSamples = newSamples[0..^silentDurationInNumSamples];
      }
    }
    if (region is SampleRegion.ANYWHERE) {
      int silentDurationInNumSamples = 0;
      int silentRegionStart = 0;
      int i;
      for (i = 0; i < newSamples.Length; i++) {
        if (Math.Abs(newSamples[i]) <= thres) {
          silentDurationInNumSamples++;
        }
        else {
          if ((float)silentDurationInNumSamples / (sampleRate * channels) >= minDuration) {
            newSamples = [..newSamples[0..silentRegionStart], ..newSamples[i..]];
            i = 0;
          }
          silentRegionStart = i + 1;
          silentDurationInNumSamples = 0;
        }
      }
      if ((float)silentDurationInNumSamples / (sampleRate * channels) >= minDuration) {
        newSamples = [.. newSamples[0..silentRegionStart], .. newSamples[i..]];
      }
    }
    ReplaceSamples(newSamples);
  }

  // play
  public void Play(double startTime = 0, Func<IWavePlayer> createOutput = null) {
    if (State == PlaybackState.Paused) {
      outputDevice.Play();
      return;
    }
    ReleasePlayback();
    try {
      SaveToTempFile();
      fileReader = new AudioFileReader(tempFilePath);
      long startFrame = (long)Math.Round(Math.Clamp(startTime, 0, Duration) * sampleRate);
      fileReader.Position = startFrame * fileReader.WaveFormat.BlockAlign;
      playbackStartTime = (double)fileReader.Position / fileReader.WaveFormat.AverageBytesPerSecond;
      outputDevice = createOutput?.Invoke() ?? new WaveOut();
      outputDevice.Init(fileReader);
      outputDevice.Play();
    }
    catch {
      ReleasePlayback();
      throw;
    }
  }
  public void Pause() {
    outputDevice?.Pause();
  }
  public void Reset() {
        // set offset to 0
        // add a button for this
  }
  public double GetPlaybackTime() {
    if (outputDevice is not IWavePosition position) return 0;
    return playbackStartTime + (double)position.GetPosition() / position.OutputWaveFormat.AverageBytesPerSecond;
  }
  public void ToggleDisabled() {
    IsDisabled = !IsDisabled;
    OnChanged();
  }

  // file i/o

      // could add our own logo

  private void SaveToTempFile() {
    Directory.CreateDirectory(Path.GetDirectoryName(tempFilePath));
    WriteWaveFile(tempFilePath);
  }
  public void SaveToFile(string path) {
    string directory = Path.GetDirectoryName(path);
    if (!string.IsNullOrEmpty(directory)) Directory.CreateDirectory(directory);
    WriteWaveFile(path);
  }
  private void WriteWaveFile(string path) {
    using var writer = new WaveFileWriter(path, WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channels));
    // Write in chunks rather than allocating another full copy of a long mix.
    float[] buffer = new float[8192];
    long padding = checked((long)Math.Round(StartTime * sampleRate) * channels);
    while (padding > 0) {
      int count = (int)Math.Min(padding, buffer.Length);
      writer.WriteSamples(buffer, 0, count);
      padding -= count;
    }
    for (int position = 0; position < samples.Length;) {
      int count = Math.Min(buffer.Length, samples.Length - position);
      for (int i = 0; i < count; i++) buffer[i] = samples[position + i] * Volume;
      writer.WriteSamples(buffer, 0, count);
      position += count;
    }
  }
  public float[] GetAdjustedSamples() {
    int padding = checked((int)Math.Round(StartTime * sampleRate) * channels);
    float[] adjSamples = new float[checked(padding + samples.Length)];
    for (int s = 0; s < samples.Length; s++) {
      adjSamples[padding + s] = samples[s] * Volume;
    }
    return adjSamples;
  }

  internal ISampleProvider CreateSampleProvider() => new TrackSampleProvider(samples, sampleRate, channels, Volume);

  private sealed class TrackSampleProvider : ISampleProvider {
    private readonly float[] data;
    private readonly float volume;
    private int position;
    public WaveFormat WaveFormat { get; }
    public TrackSampleProvider(float[] data, int rate, int channels, float volume) {
      this.data = data;
      this.volume = volume;
      WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(rate, channels);
    }
    public int Read(Span<float> buffer) {
      int count = Math.Min(buffer.Length, data.Length - position);
      for (int i = 0; i < count; i++) buffer[i] = data[position + i] * volume;
      position += count;
      return count;
    }
  }

  // render
  public void ShiftSampleOver(double seconds) {
    StartTime = Math.Max(0, StartTime + seconds);
    OnChanged();
  }
  public void DrawSamples(Graphics g, TimelineViewport viewport) {
    if (PanDrawSamples is null || samples.Length == 0) return;
    int frames = samples.Length / channels;
    float center = PanDrawSamples.ClientSize.Height / 2f;
    float height = Math.Max(0, center - 1);
    using Pen pen = new(IsDisabled ? SAMPLES_DRAW_COLOR_DISABLED : SAMPLES_DRAW_COLOR);

    if (viewport.SecondsPerPixel * sampleRate < 1) {
      // Preserve individual channel traces when zoomed closer than one frame/pixel.
      int first = (int)Math.Clamp(Math.Floor((viewport.Start - StartTime) * sampleRate) - 1, 0, frames);
      int end = (int)Math.Clamp(Math.Ceiling((viewport.TimeAt(viewport.Width) - StartTime) * sampleRate) + 1, 0, frames);
      for (int channel = 0; channel < channels; channel++) {
        PointF? previous = null;
        for (int frame = first; frame < end; frame++) {
          PointF point = new((float)viewport.XAt(StartTime + (double)frame / sampleRate),
            center - Math.Clamp(samples[frame * channels + channel] * Volume, -1, 1) * height);
          if (previous.HasValue) g.DrawLine(pen, previous.Value, point);
          else g.DrawLine(pen, point.X, point.Y, point.X + 1, point.Y);
          previous = point;
        }
      }
      return;
    }

    waveformPeaks ??= new WaveformPeaks(samples, channels);
    for (int x = 0; x < PanDrawSamples.ClientSize.Width; x++) {
      double start = (viewport.TimeAt(x) - StartTime) * sampleRate;
      double end = (viewport.TimeAt(x + 1) - StartTime) * sampleRate;
      if (end <= 0 || start >= frames) continue;
      int first = (int)Math.Clamp(Math.Floor(start), 0, frames);
      int last = (int)Math.Clamp(Math.Ceiling(end), 0, frames);
      if (last <= first) continue;
      var peak = waveformPeaks.Query(first, last);
      float top = center - Math.Clamp(peak.Max * Volume, -1, 1) * height;
      float bottom = center - Math.Clamp(peak.Min * Volume, -1, 1) * height;
      g.DrawLine(pen, x, top, x, Math.Max(top + 1, bottom));
    }
  }

  // dispose
  private void ReleasePlayback() {
    outputDevice?.Stop();
    outputDevice?.Dispose();
    outputDevice = null;
    fileReader?.Dispose();
    fileReader = null;
    playbackStartTime = 0;
  }
  public void Dispose() {
    ReleasePlayback();
    if (File.Exists(tempFilePath)) File.Delete(tempFilePath);
  }
}
