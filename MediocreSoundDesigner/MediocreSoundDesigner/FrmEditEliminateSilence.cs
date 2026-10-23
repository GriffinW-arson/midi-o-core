namespace MediocreSoundDesigner;

public partial class FrmEditEliminateSilence : FrmEditMaster {
  public FrmEditEliminateSilence() {
    InitializeComponent();
  }

  protected override void Init() {

  }

  protected override void Apply() {
    SampleRegion region = SampleRegion.NONE;
    if (chkRegionBeginning.Checked) {
      region |= SampleRegion.BEGINNING;
    }
    if (chkRegionMiddle.Checked) {
      region |= SampleRegion.MIDDLE;
    }
    if (chkRegionEnd.Checked) {
      region |= SampleRegion.END;
    }
    track.EliminateSilence(region, (float)nudMinDuration.Value, (float)nudThreshold.Value);
  }
}
