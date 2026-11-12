namespace MediocreSoundDesigner;

public abstract class FrmEditModulation : FrmEditMaster {
  private const int FirstRowTop = 16;
  private const int RowHeight = 48;
  private const int InputLeft = 388;
  private int parameterCount;

  protected FrmEditModulation(string title, int rowCount) {
    Text = title;
    int buttonTop = FirstRowTop + rowCount * RowHeight + 16;
    ClientSize = new Size(546, buttonTop + 63);

    Button cancel = AddButton("Cancel", Color.Red, new Point(245, buttonTop), 111, rowCount + 1);
    Button apply = AddButton("Apply", Color.Lime, new Point(InputLeft, buttonTop), 103, rowCount);
    cancel.CausesValidation = false;
    cancel.Click += btnCancel_Click;
    apply.Click += btnApply_Click;
    CancelButton = cancel;
    AcceptButton = apply;
  }

  protected override void Init() {
    if (track.HasSelection) Text += " — selected region";
  }

  protected NumericUpDown AddParameter(string caption, decimal value, decimal minimum, decimal maximum,
      int decimalPlaces = 0, decimal increment = 1) {
    int top = FirstRowTop + parameterCount * RowHeight;
    var input = new NumericUpDown {
      AccessibleName = caption,
      Location = new Point(InputLeft, top),
      Width = 142,
      Minimum = minimum,
      Maximum = maximum,
      DecimalPlaces = decimalPlaces,
      Increment = increment,
      Value = value,
      TabIndex = parameterCount++
    };
    Controls.Add(input);
    var label = new Label {
      Text = caption,
      TextAlign = ContentAlignment.MiddleRight,
      Location = new Point(12, top),
      Size = new Size(InputLeft - 18, input.Height)
    };
    Controls.Add(label);
    return input;
  }

  private Button AddButton(string caption, Color color, Point location, int width, int tabIndex) {
    var button = new Button {
      AutoSize = true,
      BackColor = Color.Black,
      ForeColor = color,
      Location = location,
      Size = new Size(width, 47),
      TabIndex = tabIndex,
      Text = caption,
      UseVisualStyleBackColor = false
    };
    Controls.Add(button);
    return button;
  }
}

public sealed class FrmEditPhaser : FrmEditModulation {
  private readonly NumericUpDown stages;
  private readonly NumericUpDown rate;
  private readonly NumericUpDown minFrequency;
  private readonly NumericUpDown maxFrequency;
  private readonly NumericUpDown mix;

  public FrmEditPhaser() : base("Phaser", 5) {
    stages = AddParameter("Stages", Track.DefaultPhaserStages, 1, 12);
    rate = AddParameter("Rate (Hz)", Track.DefaultPhaserRateHz, 0.01m, 20, 2, 0.05m);
    minFrequency = AddParameter("Min frequency (Hz)", Track.DefaultPhaserMinFrequency, 0.05m, 20_000, 2, 10);
    maxFrequency = AddParameter("Max frequency (Hz)", Track.DefaultPhaserMaxFrequency, minFrequency.Value, 20_000, 2, 10);
    mix = AddParameter("Mix (%)", Track.DefaultPhaserMixPercent, 0, 100);
    minFrequency.ValueChanged += (_, _) => maxFrequency.Minimum = minFrequency.Value;
  }

  protected override void Init() {
    base.Init();
    minFrequency.Maximum = Math.Min(20_000, track.SampleRate * 0.05m);
    maxFrequency.Maximum = Math.Min(20_000, track.SampleRate * 0.4m);
  }

  protected override void Apply() {
    track.Phaser((int)stages.Value, (double)rate.Value,
      (double)minFrequency.Value, (double)maxFrequency.Value, (double)mix.Value / 100);
  }
}

public sealed class FrmEditChorus : FrmEditModulation {
  private readonly NumericUpDown rate;
  private readonly NumericUpDown delay;
  private readonly NumericUpDown depth;
  private readonly NumericUpDown mix;

  public FrmEditChorus() : base("Chorus", 4) {
    rate = AddParameter("Rate (Hz)", Track.DefaultChorusRateHz, 0.01m, 20, 2, 0.05m);
    delay = AddParameter("Delay (ms)", Track.DefaultChorusDelayMs, 1, 100, 2, 0.5m);
    depth = AddParameter("Depth (ms)", Track.DefaultChorusDepthMs, 0, delay.Value, 2, 0.5m);
    mix = AddParameter("Mix (%)", Track.DefaultChorusMixPercent, 0, 100);
    delay.ValueChanged += (_, _) => depth.Maximum = delay.Value;
  }

  protected override void Apply() {
    track.Chorus((double)rate.Value, (double)delay.Value, (double)depth.Value, (double)mix.Value / 100);
  }
}
