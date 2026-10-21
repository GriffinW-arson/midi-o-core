using MediocreSoundDesigner.Properties;

namespace MediocreSoundDesigner;

public partial class FrmMain : Form {
  private Sound sound;
  private bool isPlaying;

  public FrmMain() {
    InitializeComponent();
    Setup();
  }

  private void Setup() {
    DoubleBuffered = true;
    sound = null;
    btnPlay.BackgroundImage = Resources.play;
    isPlaying = false;
  }

  private void panWaveform_Paint(object sender, PaintEventArgs e) {
    base.OnPaint(e);
    sound?.DrawSamples(e.Graphics, sender as Panel);
  }

  private void btnPlay_Click(object sender, EventArgs e) {
    if (sound.State is NAudio.Wave.PlaybackState.Stopped or NAudio.Wave.PlaybackState.Paused) {
      sound?.Play();
      btnPlay.BackgroundImage = Resources.pause;
    }
    else {
      sound?.Pause();
      btnPlay.BackgroundImage = Resources.play;
    }
  }

  private void btnVolUp_Click(object sender, EventArgs e) {
    sound?.AdjustVolume(+0.15f);
    panWaveform.Invalidate();
  }

  private void btnVolDown_Click(object sender, EventArgs e) {
    sound?.AdjustVolume(-0.15f);
    panWaveform.Invalidate();
  }

  private void menuEditReverse_Click(object sender, EventArgs e) {
    sound?.Reverse();
    panWaveform.Invalidate();
  }

  private void menuFileOpen_Click(object sender, EventArgs e) {
    ofdLoadFile.DefaultExt = "wav";
    if (ofdLoadFile.ShowDialog() == DialogResult.OK) {
      sound?.Dispose();
      sound = new(ofdLoadFile.FileName);
      panWaveform.Invalidate();
    }
  }

  private void tmrUpdatePlaybackState_Tick(object sender, EventArgs e) {
    if (sound is not null) {
      btnPlay.BackgroundImage = sound.State == NAudio.Wave.PlaybackState.Playing ? Resources.pause : Resources.play;
      btnPlay.Visible = true;
    }
    else {
      btnPlay.BackgroundImage = null;
      btnPlay.Visible = false;
    }
  }

  private void menuGenNoise_Click(object sender, EventArgs e) {
    const int NUM_SAMPLES = 100_000;
    float[] samples = new float[NUM_SAMPLES];
    Random rand = new();
    for (int i = 0; i < NUM_SAMPLES; i++) {
      samples[i] = rand.NextSingle() * 2.0f - 1.0f;
    }
    sound?.Dispose();
    sound = new(samples);
    panWaveform.Invalidate();
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
    sound?.Dispose();
    sound = new(samples);
    panWaveform.Invalidate();
  }

  private void menuEditFlip_Click(object sender, EventArgs e) {
    sound.Flip();
    panWaveform.Invalidate();
  }

  private void menuFileSave_Click(object sender, EventArgs e) {
    sfdSaveFile.DefaultExt = "wav";
    if (sfdSaveFile.ShowDialog() == DialogResult.OK) {
      sound.SaveToFile(sfdSaveFile.FileName);
    }
  }

  private void menuEditVolume_Click(object sender, EventArgs e) {
    FrmEditVolume frmEditVolume = new();
    frmEditVolume.setSound(sound);
    frmEditVolume.ShowDialog();
    panWaveform.Invalidate();
  }
}
