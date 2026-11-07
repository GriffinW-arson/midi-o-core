namespace MediocreSoundDesigner;

// Exact min/max queries with cached blocks, including peaks between display pixels.
internal sealed class WaveformPeaks {
  private const int BlockSize = 256;
  private readonly float[] samples;
  private readonly int channels;
  private readonly List<(float Min, float Max)[]> levels = new();

  public WaveformPeaks(float[] samples, int channels) {
    this.samples = samples;
    this.channels = channels;
    int frames = samples.Length / channels;
    var blocks = new (float Min, float Max)[(frames + BlockSize - 1) / BlockSize];
    for (int b = 0; b < blocks.Length; b++)
      blocks[b] = Scan(b * BlockSize, Math.Min(frames, (b + 1) * BlockSize));
    levels.Add(blocks);
    while (blocks.Length > 1) {
      var parents = new (float Min, float Max)[(blocks.Length + 1) / 2];
      for (int i = 0; i < parents.Length; i++) {
        var a = blocks[i * 2];
        var b = blocks[Math.Min(i * 2 + 1, blocks.Length - 1)];
        parents[i] = (Math.Min(a.Min, b.Min), Math.Max(a.Max, b.Max));
      }
      levels.Add(parents);
      blocks = parents;
    }
  }

  private (float Min, float Max) Scan(int first, int end) {
    float min = float.PositiveInfinity, max = float.NegativeInfinity;
    for (int i = first * channels; i < end * channels; i++) {
      min = Math.Min(min, samples[i]);
      max = Math.Max(max, samples[i]);
    }
    return (min, max);
  }

  public (float Min, float Max) Query(int first, int end) {
    float min = float.PositiveInfinity, max = float.NegativeInfinity;
    while (first < end) {
      (float Min, float Max) peak;
      if (first % BlockSize != 0 || end - first < BlockSize) {
        int next = Math.Min(end, (first / BlockSize + 1) * BlockSize);
        peak = Scan(first, next);
        first = next;
      }
      else {
        int level = 0, size = BlockSize;
        while (level + 1 < levels.Count && first % (size * 2) == 0 &&
               (long)size * 2 <= end - first) {
          level++;
          size *= 2;
        }
        peak = levels[level][first / size];
        first += size;
      }
      min = Math.Min(min, peak.Min);
      max = Math.Max(max, peak.Max);
    }
    return (min, max);
  }
}
