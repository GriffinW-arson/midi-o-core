namespace MediocreSoundDesigner {
  public partial class FrmEditMaster : Form {
    protected Sound sound;

    public FrmEditMaster() {
      InitializeComponent();
    }

    private void FrmEditMaster_Load(object sender, EventArgs e) {

    }

    public void SetSound(Sound sound) {
      this.sound = sound;
    }
  }
}
