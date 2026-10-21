namespace MediocreSoundDesigner;

public partial class FrmEditVolume : Form {
  private Sound snd;

  public FrmEditVolume() {
    InitializeComponent();
  }
  private void FrmEditVolume_Load(object sender, EventArgs e) {

  }

  public void setSound(Sound snd) {
    this.snd = snd;
    nudVolume.Value = (decimal)(snd.Volume * 100.0f);
  }

  private void btnCancel_Click(object sender, EventArgs e) {
    DialogResult = DialogResult.Cancel;
    Close();
  }

  private void btnApply_Click(object sender, EventArgs e) {
    snd.SetVolume((float)nudVolume.Value / 100.0f);
    DialogResult = DialogResult.OK;
    Close();
  }
}
