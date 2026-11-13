namespace MediocreSoundDesigner;

public partial class FrmEditVolume : FrmEditMaster {
  public FrmEditVolume() {
    InitializeComponent();
  }

  protected override void Init() {
    nudVolume.Value = track.HasSelection ? 100 : (decimal)(track.Volume * 100.0f);
    if (track.HasSelection) Text += " — selected region";
  }

  protected override void Apply() {
    track.SetVolume((float)nudVolume.Value / 100.0f);
  }
}
