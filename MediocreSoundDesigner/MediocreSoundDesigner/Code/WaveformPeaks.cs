namespace MediocreSoundDesigner;

// store the max and min range of an interval of samples
// this allows the waveform to capture the largest/smallest no matter the zoom
internal sealed class WaveformPeaks {
  // resolution
  private const int BlockSize = 256;
  private readonly float[] samples;
  private readonly int channels;
  private readonly List<(float Min, float Max)[]> levels = new();

  public WaveformPeaks(float[] samples, int channels) {
	this.samples = samples;
	this.channels = channels;
	// # of pairs of samples (if stereo)
	int frames = samples.Length / channels;
	// block = set of 256 frames
	var blocks = new (float Min, float Max)[(frames + BlockSize - 1) / BlockSize];

	// call scan on each frame of each block to get min/max
	for (int b = 0; b < blocks.Length; b++)
	  blocks[b] = Scan(b * BlockSize, Math.Min(frames, (b + 1) * BlockSize));
	levels.Add(blocks);

	// repeatedly combine blocks until one holds all frames
	while (blocks.Length > 1) {
	  var parents = new (float Min, float Max)[(blocks.Length + 1) / 2];
	  for (int i = 0; i < parents.Length; i++) {

		var a = blocks[i * 2];											// left
		var b = blocks[Math.Min(i * 2 + 1, blocks.Length - 1)];			// right
		parents[i] = (Math.Min(a.Min, b.Min), Math.Max(a.Max, b.Max));
	  }
	  levels.Add(parents);
	  // the blocks are now the parents
	  blocks = parents;
	}
  }
  // get the maximum and minimum amplitudes in the given range of frames
  // return as tuple
  private (float Min, float Max) Scan(int first, int end) {
	float min = float.PositiveInfinity, max = float.NegativeInfinity;
	for (int i = first * channels; i < end * channels; i++) {
	  min = Math.Min(min, samples[i]);
	  max = Math.Max(max, samples[i]);
	}
	return (min, max);
  }

  // handler; dynamically decides what level to scan at depending on the number of samples
  // return tuple
  public (float Min, float Max) Query(int first, int end) {
	float min = float.PositiveInfinity, max = float.NegativeInfinity;
	while (first < end) {
	  (float Min, float Max) peak;

	  // is 
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
