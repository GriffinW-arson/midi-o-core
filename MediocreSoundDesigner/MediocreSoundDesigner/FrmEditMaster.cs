namespace MediocreSoundDesigner;

public partial class FrmEditMaster : Form {
  // fields
  protected Sound sound;

  // ctor
  public FrmEditMaster() {
    InitializeComponent();
  }

  // sound
  public virtual void SetSound(Sound sound) {
    this.sound = sound;
    Init();
  }

  // gui
  public void btnCancel_Click(object sender, EventArgs e) {
    Cancel();
    DialogResult = DialogResult.Cancel;
    Close();
  }

  public void btnApply_Click(object sender, EventArgs e) {
    Apply();
    DialogResult = DialogResult.OK;
    Close();
  }

  // override in subclass if needed
  protected virtual void Init() { }
  protected virtual void Apply() { }
  protected virtual void Cancel() { }
}
