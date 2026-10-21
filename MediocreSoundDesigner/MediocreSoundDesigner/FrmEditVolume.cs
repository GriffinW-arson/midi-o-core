namespace MediocreSoundDesigner;

public partial class FrmEditVolume : FrmEditMaster {
  public FrmEditVolume() {
    InitializeComponent();
  }

  protected override void Init() {
    nudVolume.Value = (decimal)(sound.Volume * 100.0f);
  }

  protected override void Apply() {
    sound.SetVolume((float)nudVolume.Value / 100.0f);
  }
}
