using NAudio.Wave;

namespace MediocreSoundDesigner;

[Flags]
public enum SampleRegion {
  NONE = 0,
  BEGINNING = 1,
  MIDDLE = 2,
  END = 4,
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
  private WaveOut outputDevice;
  private Panel panContainer;

  // consts
  private static readonly Color SAMPLES_DRAW_COLOR = Color.Cyan;
  private static readonly Color SAMPLES_DRAW_COLOR_DISABLED = Color.LightGray;

  // properties
  public bool IsDisabled { get; private set; }
  public Panel PanDrawSamples { get; private set; }
  public float Volume { get; private set; }
  private string tempFilePath => Path.Combine(Path.GetTempPath(), "MediocreSoundDesigner", "temp.wav");
  public PlaybackState State => outputDevice?.PlaybackState ?? PlaybackState.Stopped;

  // ctor
  public Track(string file) {
    fileReader = new AudioFileReader(file);
    int sampleCount = (int)(fileReader.Length / 4);
    this.samples = new float[sampleCount];
    int samplesRead = ((ISampleProvider)fileReader).Read(samples);
    if (samplesRead < sampleCount) {
      Array.Resize(ref samples, samplesRead);
    }

    this.file = file;
    this.duration = fileReader.TotalTime;
    this.offset = 0;
    this.timeScale = 1.0f;
    this.Volume = 1.0f;
    this.pitch = 1.0f;
    this.bits = fileReader.WaveFormat.BitsPerSample;
    this.channels = fileReader.WaveFormat.Channels;
    this.sampleRate = fileReader.WaveFormat.SampleRate;
    IsDisabled = false;
  }
  public Track(float[] samples) {
    this.samples = samples;

    this.file = "";
    this.duration = TimeSpan.Zero;
    this.offset = 0;
    this.timeScale = 1.0f;
    this.Volume = 1.0f;
    this.pitch = 1.0f;
    this.bits = 32;
    this.channels = 1;
    this.sampleRate = 0;
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
  }
  public void Reverse() {
    for (int i = 0; i < samples.Length / 2; i++) {
      var temp = samples[i];
      samples[i] = samples[samples.Length - 1 - i];
      samples[samples.Length - 1 - i] = temp;
    }
  }
  public void Flip() {
    for (int i = 0; i < samples.Length; i++) {
      samples[i] *= -1;
    }
  }
  public void Trim() {
  }
  public void AlterTimeScale(float scale) {
  }
  public void SetVolume(float newVolume) {
    Volume = newVolume;
  }
  public void EliminateSilence(SampleRegion region, float minDuration, float thres) {
    float[] newSamples = CloneSamples();
    if (region.HasFlag(SampleRegion.BEGINNING)) {
      int silentDurationInNumSamples = 0;
      for (int i = 0; i < newSamples.Length; i++) {
        if (Math.Abs(newSamples[i]) <= thres) {
          silentDurationInNumSamples++;
        }
        else {
          break;
        }
      }
      if ((float)silentDurationInNumSamples / sampleRate >= minDuration) {
        newSamples = newSamples[silentDurationInNumSamples..];
      }
    }
    if (region.HasFlag(SampleRegion.END)) {
      int silentDurationInNumSamples = 0;
      for (int i = newSamples.Length - 1; i >= 0; i--) {
        if (Math.Abs(newSamples[i]) <= thres) {
          silentDurationInNumSamples++;
        }
        else {
          break;
        }
      }
      if ((float)silentDurationInNumSamples / sampleRate >= minDuration) {
        newSamples = newSamples[0..silentDurationInNumSamples];
      }
    }
    if (region.HasFlag(SampleRegion.MIDDLE)) {
      int silentDurationInNumSamples = 0;
      int silentRegionStart = 0;
      for (int i = 0; i < newSamples.Length; i++) {
        if (Math.Abs(newSamples[i]) <= thres) {
          silentDurationInNumSamples++;
        }
        else {
          if ((float)silentDurationInNumSamples / sampleRate >= minDuration) {
            newSamples = newSamples[silentRegionStart..i];
          }
          silentRegionStart = i + 1;
          silentDurationInNumSamples = 0;
        }
      }
    }
    samples = newSamples;
  }

  // play
  public void Play() {
    outputDevice?.Dispose();
    fileReader?.Dispose();
    SaveToTempFile();
    fileReader = new AudioFileReader(tempFilePath);
    outputDevice = new WaveOut();
    outputDevice.Init(fileReader);
    outputDevice.Play();
  }
  public void Pause() {
    outputDevice?.Pause();
  }
  public void Reset() {
  }
  public int GetPlayheadPos() {
    return (int)((outputDevice?.GetPosition() ?? 0) / 4) / (samples.Length / panContainer.Width);
  }
  public void ToggleDisabled() {
    IsDisabled = !IsDisabled;
  }

  // file i/o
  private void SaveToTempFile() {
    Directory.CreateDirectory(Path.GetDirectoryName(tempFilePath));
    using var writer = new WaveFileWriter(tempFilePath, fileReader?.WaveFormat ?? new());
    writer.WriteSamples(GetSamplesAdjustedByVolume(), 0, samples.Length);
  }
  public void SaveToFile(string path) {
    Directory.CreateDirectory(Path.GetDirectoryName(path));
    using var writer = new WaveFileWriter(path, fileReader?.WaveFormat ?? new());
    writer.WriteSamples(GetSamplesAdjustedByVolume(), 0, samples.Length);
  }
  private float[] GetSamplesAdjustedByVolume() {
    float[] adjSamples = new float[samples.Length];
    for (int s = 0; s < samples.Length; s++) {
      adjSamples[s] = samples[s] * Volume;
    }
    return adjSamples;
  }

  // render
  public void DrawSamples(Graphics g) {
    if (PanDrawSamples is null) {
      return;
    }

    int stride = samples.Length / PanDrawSamples.Width;
    float heightAdj = PanDrawSamples.Height / 2;
    if (samples != null && samples.Length > 0) {
      int x = 0;
      Pen pen = new(IsDisabled ? SAMPLES_DRAW_COLOR_DISABLED : SAMPLES_DRAW_COLOR);
      List<Point> points = new();
      for (int s = 0; s < samples.Length; s += stride) {
        var sample = (int)(samples[s] * heightAdj * Volume);
        points.Add(new(x, PanDrawSamples.Height / 2));
        points.Add(new(x, PanDrawSamples.Height / 2 - sample));
        x++;
      }
      g.DrawLines(pen, points.ToArray());
      pen.Dispose();
    }
  }

  // dispose
  public void Dispose() {
    outputDevice?.Dispose();
    fileReader?.Dispose();
  }
}
