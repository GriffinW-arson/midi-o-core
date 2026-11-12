using NAudio.Wave;

namespace MediocreSoundDesigner;

public enum SampleRegion
{
    BEGINNING_ONLY,
    END_ONLY,
    BEGINNING_AND_END,
    ANYWHERE,
}

public class Track : IDisposable
{
    // Static variable to track last used color index across tracks (prevents consecutive duplicates)
    private static int lastAssignedIndex = -1;
    private static readonly Random Rand = new();

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
    private static readonly Color SAMPLES_DRAW_COLOR_DISABLED = Color.LightGray;

    // properties
    public Pen WaveformPen { get; private set; }
    public int PaletteIndex { get; private set; }

    // start time in seconds
    public double StartTime { get; private set; }
    // calculated duration
    // dur = (number of samples * number of channels) / sample rate
    public double Duration => (double)(samples.Length / channels) / sampleRate;
    public int SampleRate => sampleRate;
    public int Channels => channels; // stereo audio
    public event EventHandler Changed;
    public int SelectionStartFrame { get; private set; }
    public int SelectionEndFrame { get; private set; }
    public bool HasSelection => SelectionEndFrame > SelectionStartFrame;
    private (int First, int End) EditFrames => HasSelection
        ? (SelectionStartFrame, SelectionEndFrame) : (0, samples.Length / channels);
    public bool IsDisabled { get; private set; }
    public Panel PanDrawSamples { get; private set; }
    public float Volume { get; private set; }
    private readonly string tempFilePath = Path.Combine(Path.GetTempPath(), "MediocreSoundDesigner", $"{Guid.NewGuid():N}.wav");
    public PlaybackState State => outputDevice?.PlaybackState ?? PlaybackState.Stopped;

    // Helper method to assign a non-consecutive color index
    private void AssignColorFromProfile(FrmMain.ColorProfile profile)
    {
        var palette = profile.AccentWaveformPalette;
        if (palette.Count == 0)
        {
            WaveformPen = new Pen(profile.DefaultWaveformColor);
            PaletteIndex = 0;
            return;
        }

        int newIndex;
        if (palette.Count == 1)
        {
            newIndex = 0;
        }
        else
        {
            do
            {
                newIndex = Rand.Next(palette.Count);
            } while (newIndex == lastAssignedIndex);
        }

        lastAssignedIndex = newIndex;
        PaletteIndex = newIndex;
        WaveformPen = new Pen(palette[PaletteIndex]);
    }

    // Dynamically update pen color based on new profile while preserving relative hue shift (Option A)
    public void ApplyTheme(FrmMain.ColorProfile profile)
    {
        var palette = profile.AccentWaveformPalette;
        if (palette.Count > 0)
        {
            int safeIndex = PaletteIndex % palette.Count;
            WaveformPen = new Pen(palette[safeIndex]);
        }
        else
        {
            WaveformPen = new Pen(profile.DefaultWaveformColor);
        }
    }

    // ctor
    public Track(string file)
    {
        AssignColorFromProfile(FrmMain.ActiveProfile);

        // use a file reader
        using var reader = new AudioFileReader(file);

        // load the ENTIRE file
        int sampleCount = checked((int)(reader.Length / sizeof(float)));
        this.samples = new float[sampleCount];
        int samplesRead = 0;
        while (samplesRead < sampleCount)
        {
            int count = ((ISampleProvider)reader).Read(samples.AsSpan(samplesRead));
            if (count == 0) break;
            samplesRead += count;
        }
        if (samplesRead < sampleCount)
        {
            Array.Resize(ref samples, samplesRead);
        }

        // configure the track from the file
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
    public Track(float[] samples, int sampleRate = 44100, int channels = 1)
    {
        if (sampleRate <= 0 || channels <= 0) throw new ArgumentOutOfRangeException();
        AssignColorFromProfile(FrmMain.ActiveProfile);
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
    public void LinkPanels(Panel panDrawSamples, Panel panContainer)
    {
        this.PanDrawSamples = panDrawSamples;
        this.panContainer = panContainer;
    }

    // edits
    public float[] CloneSamples()
    {
        return (float[])samples.Clone();
    }
    public void ReplaceSamples(float[] samples)
    {
        this.samples = samples;
        ClearSelection();
        SamplesChanged();
    }

    public void SetSelection(double firstTime, double endTime)
    {
        int frames = samples.Length / channels;
        if (firstTime == endTime) { ClearSelection(); return; }
        SelectionStartFrame = (int)Math.Clamp(Math.Round((Math.Min(firstTime, endTime) - StartTime) * sampleRate), 0, frames);
        SelectionEndFrame = (int)Math.Clamp(Math.Round((Math.Max(firstTime, endTime) - StartTime) * sampleRate), 0, frames);
        PanDrawSamples?.Invalidate();
    }

    public void ClearSelection()
    {
        SelectionStartFrame = SelectionEndFrame = 0;
        PanDrawSamples?.Invalidate();
    }

    public void DeleteSelection()
    {
        if (!HasSelection) return;
        int first = SelectionStartFrame * channels;
        int end = SelectionEndFrame * channels;
        var remaining = new float[samples.Length - (end - first)];
        Array.Copy(samples, 0, remaining, 0, first);
        Array.Copy(samples, end, remaining, first, samples.Length - end);
        ReplaceSamples(remaining);
    }

    public void DrawSelection(Graphics g, TimelineViewport viewport)
    {
        if (!HasSelection || PanDrawSamples is null) return;
        double left = viewport.XAt(StartTime + (double)SelectionStartFrame / sampleRate);
        double right = viewport.XAt(StartTime + (double)SelectionEndFrame / sampleRate);
        float x = (float)Math.Clamp(left, 0, PanDrawSamples.ClientSize.Width);
        float end = (float)Math.Clamp(right, 0, PanDrawSamples.ClientSize.Width);
        if (end <= x) return;
        Color color = FrmMain.ActiveProfile.DefaultWaveformColor;
        using var fill = new SolidBrush(Color.FromArgb(55, color));
        using var border = new Pen(color);
        g.FillRectangle(fill, x, 0, end - x, PanDrawSamples.ClientSize.Height);
        g.DrawRectangle(border, x, 0, Math.Max(0, end - x - 1), Math.Max(0, PanDrawSamples.ClientSize.Height - 1));
    }
    private void SamplesChanged()
    {
        waveformPeaks = null;
        OnChanged();
    }
    private void OnChanged()
    {
        PanDrawSamples?.Invalidate();
        Changed?.Invoke(this, EventArgs.Empty);
    }
    public void Reverse()
    {
        var range = EditFrames;
        for (int left = range.First, right = range.End - 1; left < right; left++, right--)
        {
            for (int channel = 0; channel < channels; channel++)
                (samples[left * channels + channel], samples[right * channels + channel]) =
                    (samples[right * channels + channel], samples[left * channels + channel]);
        }
        SamplesChanged();
    }
    public void Flip()
    {
        var range = EditFrames;
        for (int i = range.First * channels; i < range.End * channels; i++)
        {
            samples[i] *= -1;
        }
        SamplesChanged();
    }

    // get the average volume of the track (absolute value to get rid of negatives)
    // and apply it universally
	public void Normalize()
	{
		var range = EditFrames;
		int first = range.First * channels;
		int end = range.End * channels;
		int count = end - first;

		if (count == 0) return;

		double total = 0;
		for (int i = first; i < end; i++)
			total += Math.Abs(samples[i]);

		float average = (float)(total / count);

		for (int i = first; i < end; i++)
			samples[i] = Math.Sign(samples[i]) * average;

		SamplesChanged();
	}

	internal const int DefaultPhaserStages = 4;
	internal const decimal DefaultPhaserRateHz = 0.4m;
	internal const int DefaultPhaserMinFrequency = 300;
	internal const int DefaultPhaserMaxFrequency = 1800;
	internal const int DefaultPhaserMixPercent = 50;

	public void Phaser() => Phaser(DefaultPhaserStages, (double)DefaultPhaserRateHz,
		DefaultPhaserMinFrequency, DefaultPhaserMaxFrequency, DefaultPhaserMixPercent / 100.0);

	internal void Phaser(int stages, double rateHz, double minFrequencyHz, double maxFrequencyHz, double mix)
	{
		var range = EditFrames;
		if (range.End <= range.First) return;

		double minFrequency = Math.Min(minFrequencyHz, sampleRate * 0.05);
		double maxFrequency = Math.Min(maxFrequencyHz, sampleRate * 0.4);

		double[,] previousInput = new double[channels, stages];
		double[,] previousOutput = new double[channels, stages];

		for (int frame = range.First; frame < range.End; frame++)
		{
			double time = (double)(frame - range.First) / sampleRate;
			double sweep = 0.5 + 0.5 * Math.Sin(2 * Math.PI * rateHz * time);
			double frequency = minFrequency *
				Math.Pow(maxFrequency / minFrequency, sweep);

			double tangent = Math.Tan(Math.PI * frequency / sampleRate);
			double coefficient = (tangent - 1) / (tangent + 1);

			for (int channel = 0; channel < channels; channel++)
			{
				int index = frame * channels + channel;
				double dry = samples[index];
				double wet = dry;

				for (int stage = 0; stage < stages; stage++)
				{
					double output =
						coefficient * wet +
						previousInput[channel, stage] -
						coefficient * previousOutput[channel, stage];

					previousInput[channel, stage] = wet;
					previousOutput[channel, stage] = output;
					wet = output;
				}

				samples[index] = (float)(dry * (1 - mix) + wet * mix);
			}
		}

		SamplesChanged();
	}

	internal const decimal DefaultChorusRateHz = 0.8m;
	internal const int DefaultChorusDelayMs = 20;
	internal const int DefaultChorusDepthMs = 5;
	internal const int DefaultChorusMixPercent = 35;

	public void Chorus() => Chorus((double)DefaultChorusRateHz, DefaultChorusDelayMs,
		DefaultChorusDepthMs, DefaultChorusMixPercent / 100.0);

	internal void Chorus(double rateHz, double baseDelayMs, double depthMs, double mix)
	{
		var range = EditFrames;
		if (range.End <= range.First) return;

		int bufferFrames = Math.Max(2,
			(int)Math.Ceiling(
				(baseDelayMs + depthMs) * sampleRate / 1000.0) + 2);

		float[] delayBuffer = new float[bufferFrames * channels];

		for (int frame = range.First; frame < range.End; frame++)
		{
			int localFrame = frame - range.First;
			int writeFrame = localFrame % bufferFrames;

			double time = (double)localFrame / sampleRate;
			double delayMs = baseDelayMs +
				depthMs * Math.Sin(2 * Math.PI * rateHz * time);

			double readPosition =
				writeFrame - delayMs * sampleRate / 1000.0;

			while (readPosition < 0)
				readPosition += bufferFrames;

			int first = (int)Math.Floor(readPosition);
			int second = (first + 1) % bufferFrames;
			double fraction = readPosition - first;

			for (int channel = 0; channel < channels; channel++)
			{
				int index = frame * channels + channel;
				float dry = samples[index];

				delayBuffer[writeFrame * channels + channel] = dry;

				double wet =
					delayBuffer[first * channels + channel] * (1 - fraction) +
					delayBuffer[second * channels + channel] * fraction;

				samples[index] = (float)(dry * (1 - mix) + wet * mix);
			}
		}

		SamplesChanged();
	}

	public void AlterTimeScale(float scale)
    {
        // sample rate times scale?
    }
    public void SetVolume(float newVolume)
    {
        if (HasSelection)
        {
            for (int i = SelectionStartFrame * channels; i < SelectionEndFrame * channels; i++)
                samples[i] *= newVolume;
            SamplesChanged();
            return;
        }
        Volume = newVolume;
        OnChanged();
    }
    public void EliminateSilence(SampleRegion region, float minDuration, float thres)
    {
        var range = EditFrames;
        bool selected = HasSelection;
        var kept = new List<float>();
        int frame = range.First;
        while (frame < range.End)
        {
            int first = frame;
            bool silent = IsSilentFrame(frame, thres);
            if (silent)
            {
                while (frame < range.End && IsSilentFrame(frame, thres)) frame++;
            }
            else frame++;
            bool eligible = region == SampleRegion.ANYWHERE ||
                (first == range.First && region is SampleRegion.BEGINNING_ONLY or SampleRegion.BEGINNING_AND_END) ||
                (frame == range.End && region is SampleRegion.END_ONLY or SampleRegion.BEGINNING_AND_END);
            bool remove = silent && eligible && (float)(frame - first) / sampleRate >= minDuration;
            if (!remove)
                for (int i = first * channels; i < frame * channels; i++) kept.Add(samples[i]);
        }
        int firstValue = range.First * channels, endValue = range.End * channels;
        var replacement = new float[samples.Length - (endValue - firstValue) + kept.Count];
        Array.Copy(samples, 0, replacement, 0, firstValue);
        kept.CopyTo(replacement, firstValue);
        Array.Copy(samples, endValue, replacement, firstValue + kept.Count, samples.Length - endValue);
        samples = replacement;
        if (selected) SelectionEndFrame = SelectionStartFrame + kept.Count / channels;
        SamplesChanged();
    }

    private bool IsSilentFrame(int frame, float threshold)
    {
        for (int channel = 0; channel < channels; channel++)
            if (Math.Abs(samples[frame * channels + channel]) > threshold) return false;
        return true;
    }

    // play
    public void Play(double startTime = 0, Func<IWavePlayer> createOutput = null)
    {
        if (State == PlaybackState.Paused)
        {
            outputDevice.Play();
            return;
        }
        ReleasePlayback();
        try
        {
            SaveToTempFile();
            fileReader = new AudioFileReader(tempFilePath);
            SeekPlayback(startTime, createOutput);
        }
        catch
        {
            ReleasePlayback();
            throw;
        }
    }
    public void Pause()
    {
        outputDevice?.Pause();
    }

    internal void SeekPlayback(double seconds, Func<IWavePlayer> createOutput)
    {
        ReleaseOutput();
        try
        {
            long frame = (long)Math.Round(Math.Clamp(seconds, 0, Duration) * sampleRate);
            fileReader.Position = frame * fileReader.WaveFormat.BlockAlign;
            playbackStartTime = (double)fileReader.Position / fileReader.WaveFormat.AverageBytesPerSecond;
            outputDevice = createOutput?.Invoke() ?? new WaveOut();
            outputDevice.Init(fileReader);
            outputDevice.Play();
        }
        catch
        {
            ReleasePlayback();
            throw;
        }
    }
    public void Reset()
    {
        // set offset to 0
        // add a button for this
    }
    public double GetPlaybackTime()
    {
        if (outputDevice is not IWavePosition position) return 0;
        return playbackStartTime + (double)position.GetPosition() / position.OutputWaveFormat.AverageBytesPerSecond;
    }
    public void ToggleDisabled()
    {
        IsDisabled = !IsDisabled;
        OnChanged();
    }

    // file i/o

    // could add our own logo

    private void SaveToTempFile()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(tempFilePath));
        WriteWaveFile(tempFilePath);
    }
    public void SaveToFile(string path)
    {
        string directory = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(directory)) Directory.CreateDirectory(directory);
        WriteWaveFile(path);
    }
    private void WriteWaveFile(string path)
    {
        using var writer = new WaveFileWriter(path, WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channels));
        // Write in chunks rather than allocating another full copy of a long mix.
        float[] buffer = new float[8192];
        long padding = checked((long)Math.Round(StartTime * sampleRate) * channels);
        while (padding > 0)
        {
            int count = (int)Math.Min(padding, buffer.Length);
            writer.WriteSamples(buffer, 0, count);
            padding -= count;
        }
        for (int position = 0; position < samples.Length;)
        {
            int count = Math.Min(buffer.Length, samples.Length - position);
            for (int i = 0; i < count; i++) buffer[i] = samples[position + i] * Volume;
            writer.WriteSamples(buffer, 0, count);
            position += count;
        }
    }
    public float[] GetAdjustedSamples()
    {
        int padding = checked((int)Math.Round(StartTime * sampleRate) * channels);
        float[] adjSamples = new float[checked(padding + samples.Length)];
        for (int s = 0; s < samples.Length; s++)
        {
            adjSamples[padding + s] = samples[s] * Volume;
        }
        return adjSamples;
    }

    internal ISampleProvider CreateSampleProvider() => new TrackSampleProvider(samples, sampleRate, channels, Volume);

    private sealed class TrackSampleProvider : ISampleProvider
    {
        private readonly float[] data;
        private readonly float volume;
        private int position;
        public WaveFormat WaveFormat { get; }
        public TrackSampleProvider(float[] data, int rate, int channels, float volume)
        {
            this.data = data;
            this.volume = volume;
            WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(rate, channels);
        }
        public int Read(Span<float> buffer)
        {
            int count = Math.Min(buffer.Length, data.Length - position);
            for (int i = 0; i < count; i++) buffer[i] = data[position + i] * volume;
            position += count;
            return count;
        }
    }

    // render
    public void ShiftSampleOver(double seconds)
    {
        StartTime = Math.Max(0, StartTime + seconds);
        OnChanged();
    }
    public void DrawSamples(Graphics g, TimelineViewport viewport)
    {
        if (PanDrawSamples is null || samples.Length == 0) return;
        int frames = samples.Length / channels;
        float center = PanDrawSamples.ClientSize.Height / 2f;
        float height = Math.Max(0, center - 1);
        using Pen pen = IsDisabled ? new Pen(SAMPLES_DRAW_COLOR_DISABLED) : new Pen(WaveformPen.Color);

        // waveform was 
        if (viewport.SecondsPerPixel * sampleRate < 1)
        {
            // Preserve individual channel traces when zoomed closer than one frame/pixel.
            int first = (int)Math.Clamp(Math.Floor((viewport.Start - StartTime) * sampleRate) - 1, 0, frames);
            int end = (int)Math.Clamp(Math.Ceiling((viewport.TimeAt(viewport.Width) - StartTime) * sampleRate) + 1, 0, frames);
            for (int channel = 0; channel < channels; channel++)
            {
                PointF? previous = null;
                for (int frame = first; frame < end; frame++)
                {
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
        for (int x = 0; x < PanDrawSamples.ClientSize.Width; x++)
        {
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
    private void ReleaseOutput()
    {
        outputDevice?.Stop();
        outputDevice?.Dispose();
        outputDevice = null;
    }

    private void ReleasePlayback()
    {
        ReleaseOutput();
        fileReader?.Dispose();
        fileReader = null;
        playbackStartTime = 0;
    }
    public void Dispose()
    {
        ReleasePlayback();
        if (File.Exists(tempFilePath)) File.Delete(tempFilePath);
    }
}
