using NAudio.Wave;

namespace MediocreSoundDesigner; 

public class TrackManager {
  public List<Track> Tracks { get; private set; }
  public PlaybackState State => pseudoCombinedTrack?.State ?? PlaybackState.Stopped;
	public Track SelectedTrack { get; private set; }
  private Track pseudoCombinedTrack;
  private Panel panContainer;

  public TrackManager(Panel panContainer) {
		Tracks = new();
    SelectedTrack = null;
    pseudoCombinedTrack = null;
    this.panContainer = panContainer;
  }

	public void AddTrack(Track track) {
		Tracks.Add(track);
	}

	public void RemTrack(Track track) {
		Tracks.Remove(track);
	}

	public void SelectTrack(Track track) {
		SelectedTrack = track;
    Tracks.ForEach(track => {
      track.PanDrawSamples.BorderStyle = BorderStyle.None;
      track.PanDrawSamples.BackColor = Color.DimGray;
    });
    track.PanDrawSamples.BorderStyle = BorderStyle.Fixed3D;
    track.PanDrawSamples.BackColor = Color.Gray;
  }

	public void PlayAll() {
    pseudoCombinedTrack?.Dispose();
    List<float> combinedSamples = new();
    foreach (Track track in Tracks) {
      if (track.IsDisabled) {
        continue;
      }
      float[] trackSamples = track.CloneSamples();
      for (int i = 0; i < trackSamples.Length; i++) {
        if (combinedSamples.Count <= i) {
          combinedSamples.Add(0);
        }
        combinedSamples[i] += trackSamples[i];
      }
    }
    pseudoCombinedTrack = new(combinedSamples.ToArray());
    pseudoCombinedTrack.LinkPanels(null, panContainer);
    pseudoCombinedTrack.Play();
  }
  public void PauseAll() {
    pseudoCombinedTrack?.Pause();
  }
  public int GetPlayheadPos() {
    return pseudoCombinedTrack?.GetPlayheadPos() ?? 0;
  }
}
