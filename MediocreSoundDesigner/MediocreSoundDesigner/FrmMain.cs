using MediocreSoundDesigner.Properties;

namespace MediocreSoundDesigner;

public partial class FrmMain : Form {
  private TrackManager trackManager;

  public FrmMain() {
    InitializeComponent();
    Setup();
  }

  private void Setup() {
    DoubleBuffered = true;
    trackManager = new(panTracks);
    btnPlay.BackgroundImage = Resources.play;
    UpdateSizes();
  }

  //private void panWaveform_Paint(object sender, PaintEventArgs e) {
  //  base.OnPaint(e);
  //  sound?.DrawSamples(e.Graphics);
  //}

  private void btnPlay_Click(object sender, EventArgs e) {
    if (trackManager.State is NAudio.Wave.PlaybackState.Stopped or NAudio.Wave.PlaybackState.Paused) {
      trackManager.PlayAll();
      btnPlay.BackgroundImage = Resources.pause;
    }
    else {
      trackManager.PauseAll();
      btnPlay.BackgroundImage = Resources.play;
    }
  }

  private void menuEditReverse_Click(object sender, EventArgs e) {
    trackManager.SelectedTrack?.Reverse();
  }

  private void menuFileOpen_Click(object sender, EventArgs e) {
    ofdLoadFile.DefaultExt = "wav";
    if (ofdLoadFile.ShowDialog() == DialogResult.OK) {
      //sound?.Dispose();
      Track newTrack = new(ofdLoadFile.FileName);
      CreateTrackPanel(newTrack);
      trackManager.AddTrack(newTrack);
      UpdateSizes();
      newTrack.PanDrawSamples.Invalidate();
    }
  }

  private Panel CreateTrackPanel(Track track) {
    Panel pan = new() {
      BackColor = Color.DimGray,
      Width = panTracks.Width,
      Left = 0,
      Top = 0,
    };
    pan.Click += (sender, e) => {
      trackManager.SelectTrack(track);

      if (ModifierKeys.HasFlag(Keys.Shift)) {
        if (MessageBox.Show("This will delete the selected track. This is NOT undoable!", "Are You Sure?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation) == DialogResult.Yes) {
          panTracks.Controls.Remove(track.PanDrawSamples);
          track.PanDrawSamples.Dispose();
          track.Dispose();
          trackManager.RemTrack(track);
          UpdateSizes();
        }
      }
    };
    pan.DoubleClick += (sender, e) => {
      track.ToggleDisabled();
      track.PanDrawSamples.Invalidate();
    };
    pan.Paint += (sender, e) => {
      track.DrawSamples(e.Graphics);
    };
    panTracks.Controls.Add(pan);
    track.LinkPanels(pan, panTracks);
    return pan;
  }

  private void tmrUpdatePlaybackState_Tick(object sender, EventArgs e) {
    if (trackManager.Tracks.Count > 0) {
      btnPlay.BackgroundImage = trackManager.State == NAudio.Wave.PlaybackState.Playing ? Resources.pause : Resources.play;
      btnPlay.Visible = true;
      lblPlayhead.Left = trackManager.GetPlayheadPos();
      lblPlayhead.Visible = true;
    }
    else {
      btnPlay.BackgroundImage = null;
      btnPlay.Visible = false;
      lblPlayhead.Visible = false;
    }
  }

  private void menuGenNoise_Click(object sender, EventArgs e) {
    const int NUM_SAMPLES = 100_000;
    float[] samples = new float[NUM_SAMPLES];
    Random rand = new();
    for (int i = 0; i < NUM_SAMPLES; i++) {
      samples[i] = rand.NextSingle() * 2.0f - 1.0f;
    }
    //sound?.Dispose();
    Track newTrack = new(samples);
    CreateTrackPanel(newTrack);
    trackManager.AddTrack(newTrack);
    UpdateSizes();
    newTrack.PanDrawSamples.Invalidate();
  }

  private void menuGenGradientNoise_Click(object sender, EventArgs e) {
    const int NUM_SAMPLES = 250_000;
    float[] samples = new float[NUM_SAMPLES];
    Random rand = new();
    float cur = 0;
    float inc = 0.005f;
    int dir = +1;
    for (int i = 0; i < NUM_SAMPLES; i++) {
      cur += (inc * dir);
      if (rand.NextSingle() < 0.5f) {
        dir *= -1;
      }
      if (cur > 0.9f) {
        dir = -1;
      }
      if (cur < -0.9f) {
        dir = +1;
      }
      samples[i] = cur;
    }
    //sound?.Dispose();
    Track newTrack = new(samples);
    CreateTrackPanel(newTrack);
    trackManager.AddTrack(newTrack);
    UpdateSizes();
    newTrack.PanDrawSamples.Invalidate();
  }

  private void menuEditFlip_Click(object sender, EventArgs e) {
    trackManager.SelectedTrack?.Flip();
  }

  private void menuFileSave_Click(object sender, EventArgs e) {
    if (trackManager.SelectedTrack is not null) {
      sfdSaveFile.DefaultExt = "wav";
      if (sfdSaveFile.ShowDialog() == DialogResult.OK) {
        trackManager.SelectedTrack.SaveToFile(sfdSaveFile.FileName);
      }
    }
  }

  private void menuEditVolume_Click(object sender, EventArgs e) {
    if (trackManager.SelectedTrack is not null) {
      FrmEditVolume frm = new();
      frm.SetSound(trackManager.SelectedTrack);
      frm.ShowDialog();
    }
  }

  private void menuEditEliminateSilence_Click(object sender, EventArgs e) {
    if (trackManager.SelectedTrack is not null) {
      FrmEditEliminateSilence frm = new();
      frm.SetSound(trackManager.SelectedTrack);
      frm.ShowDialog();
    }
  }

  private void UpdateSizes() {
    const int PADDING = 15;

    panTracks.Left = PADDING;
    panTracks.Width = ClientSize.Width - (PADDING * 2);

    int top = 0;
    foreach (Track track in trackManager.Tracks) {
      track.PanDrawSamples.Left = 0;
      track.PanDrawSamples.Width = panTracks.Width;
      track.PanDrawSamples.Height = (panTracks.Height / trackManager.Tracks.Count) - 1;
      track.PanDrawSamples.Top = top;
      track.PanDrawSamples.Invalidate();
      top += track.PanDrawSamples.Height;
    }
    lblPlayhead.Left = 0;
    lblPlayhead.Height = panTracks.Height;
    lblPlayhead.BringToFront();
  }

  private void FrmMain_Resize(object sender, EventArgs e) {
    UpdateSizes();
  }
}
