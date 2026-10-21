using NAudio.Wave;

namespace MediocreSoundDesigner;

public class Sound : IDisposable {
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

  private static readonly Color SAMPLES_DRAW_COLOR = Color.Cyan;

  public float Volume { get; private set; }
  private string tempFilePath => Path.Combine(Path.GetTempPath(), "MediocreSoundDesigner", "temp.wav");

  public PlaybackState State => outputDevice?.PlaybackState ?? PlaybackState.Stopped;

  public Sound(string file) {
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
  }

  public Sound(float[] samples) {
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

  public void Trim() {
  }

  public void AlterTimeScale(float scale) {
  }

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

  public void AdjustVolume(float adjustBy) {
    Volume += adjustBy;
    Volume = Math.Clamp(Volume, 0.0f, 2.0f);
  }

  public void SetVolume(float newVolume) {
    Volume = newVolume;
  }

  private float[] GetSamplesAdjustedByVolume() {
    float[] adjSamples = new float[samples.Length];
    for (int s = 0; s < samples.Length; s++) {
      adjSamples[s] = samples[s] * Volume;
    }
    return adjSamples;
  }

  public void DrawSamples(Graphics g, Panel panel) {
    const int SIZE = 1;

    float avgAmp = samples.Select(sample => Math.Abs(sample)).Average();
    float maxAmp = samples.Select(sample => Math.Abs(sample)).Max();

    float normScale = panel.Height / 2;
    if (samples != null && samples.Length > 0) {
      int x = 0;
      Pen p = new(SAMPLES_DRAW_COLOR);
      for (int s = 0; s < samples.Length; s += samples.Length / panel.Width) {
        var sample = (int)(samples[s] * normScale * Volume);
        g.DrawLine(p, new Point(x, panel.Height / 2), new Point(x, panel.Height / 2 - sample));
        x += SIZE;
      }
      p.Dispose();
      g.Flush();
    }
  }

  public void Dispose() {
    outputDevice?.Dispose();
    fileReader?.Dispose();
  }
}
