using NAudio.Wave;

namespace MediocreSoundDesigner;

public partial class FrmGenRecord : FrmEditMaster {
  private WaveIn waveSource = null;
  private List<float> recordedSamples;

  public FrmGenRecord() {
    InitializeComponent();
  }

  protected override void Init() {
    recordedSamples = new();
  }

  protected override void Apply() {
    track.ReplaceSamples(recordedSamples.ToArray());
  }

  private void btnRecord_Click(object sender, EventArgs e) {
    try {
      waveSource = new();
      waveSource.WaveFormat = new WaveFormat(44100, 1);

      waveSource.DataAvailable += WaveSource_DataAvailable;
      waveSource.RecordingStopped += WaveSource_RecordingStopped;

      waveSource.StartRecording();

      // UI management
      //btnStart.Enabled = false;
      //btnStop.Enabled = true;
    }
    catch (Exception ex) {
      MessageBox.Show($"Failed to start recording: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
  }

  private void btnStop_Click(object sender, EventArgs e) {
    // UI management
    //btnStop.Enabled = false;
    waveSource?.StopRecording();
  }

  private void WaveSource_DataAvailable(object sender, WaveInEventArgs e) {
    // 16-bit mono audio means 2 bytes per sample
    int sampleCount = e.BytesRecorded / 2;

    for (int i = 0; i < sampleCount; i++) {
      short sample = BitConverter.ToInt16(e.Buffer, i * 2);
      recordedSamples.Add(sample / 32768f);
    }
  }

  // Executed automatically after StopRecording() has completed
  private void WaveSource_RecordingStopped(object sender, StoppedEventArgs e) {
    // Safely dispose of resources in the correct order
    if (waveSource != null) {
      waveSource.Dispose();
      waveSource = null;
    }

    //btnStart.Enabled = true;
    MessageBox.Show("Recording saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
  }
}
