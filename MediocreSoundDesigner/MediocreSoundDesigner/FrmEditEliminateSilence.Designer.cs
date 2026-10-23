namespace MediocreSoundDesigner {
  partial class FrmEditEliminateSilence {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      btnCancel = new Button();
      btnApply = new Button();
      chkRegionBeginning = new CheckBox();
      chkRegionEnd = new CheckBox();
      chkRegionMiddle = new CheckBox();
      nudMinDuration = new NumericUpDown();
      label1 = new Label();
      label2 = new Label();
      nudThreshold = new NumericUpDown();
      label3 = new Label();
      ((System.ComponentModel.ISupportInitialize)nudMinDuration).BeginInit();
      ((System.ComponentModel.ISupportInitialize)nudThreshold).BeginInit();
      SuspendLayout();
      // 
      // btnCancel
      // 
      btnCancel.AutoSize = true;
      btnCancel.BackColor = Color.Black;
      btnCancel.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
      btnCancel.ForeColor = Color.Red;
      btnCancel.Location = new Point(112, 302);
      btnCancel.Name = "btnCancel";
      btnCancel.Size = new Size(111, 47);
      btnCancel.TabIndex = 5;
      btnCancel.Text = "Cancel";
      btnCancel.UseVisualStyleBackColor = false;
      btnCancel.Click += btnCancel_Click;
      // 
      // btnApply
      // 
      btnApply.AutoSize = true;
      btnApply.BackColor = Color.Black;
      btnApply.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
      btnApply.ForeColor = Color.Lime;
      btnApply.Location = new Point(254, 302);
      btnApply.Name = "btnApply";
      btnApply.Size = new Size(103, 47);
      btnApply.TabIndex = 4;
      btnApply.Text = "Apply";
      btnApply.UseVisualStyleBackColor = false;
      btnApply.Click += btnApply_Click;
      // 
      // chkRegionBeginning
      // 
      chkRegionBeginning.AutoSize = true;
      chkRegionBeginning.ForeColor = Color.White;
      chkRegionBeginning.Location = new Point(17, 66);
      chkRegionBeginning.Name = "chkRegionBeginning";
      chkRegionBeginning.Size = new Size(166, 41);
      chkRegionBeginning.TabIndex = 6;
      chkRegionBeginning.Text = "Beginning";
      chkRegionBeginning.UseVisualStyleBackColor = true;
      // 
      // chkRegionEnd
      // 
      chkRegionEnd.AutoSize = true;
      chkRegionEnd.ForeColor = Color.White;
      chkRegionEnd.Location = new Point(328, 66);
      chkRegionEnd.Name = "chkRegionEnd";
      chkRegionEnd.Size = new Size(83, 41);
      chkRegionEnd.TabIndex = 7;
      chkRegionEnd.Text = "End";
      chkRegionEnd.UseVisualStyleBackColor = true;
      // 
      // chkRegionMiddle
      // 
      chkRegionMiddle.AutoSize = true;
      chkRegionMiddle.ForeColor = Color.White;
      chkRegionMiddle.Location = new Point(189, 66);
      chkRegionMiddle.Name = "chkRegionMiddle";
      chkRegionMiddle.Size = new Size(127, 41);
      chkRegionMiddle.TabIndex = 8;
      chkRegionMiddle.Text = "Middle";
      chkRegionMiddle.UseVisualStyleBackColor = true;
      // 
      // nudMinDuration
      // 
      nudMinDuration.DecimalPlaces = 1;
      nudMinDuration.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
      nudMinDuration.Location = new Point(280, 153);
      nudMinDuration.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
      nudMinDuration.Name = "nudMinDuration";
      nudMinDuration.Size = new Size(86, 43);
      nudMinDuration.TabIndex = 9;
      nudMinDuration.Value = new decimal(new int[] { 5, 0, 0, 65536 });
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new Point(12, 153);
      label1.Name = "label1";
      label1.Size = new Size(262, 37);
      label1.TabIndex = 10;
      label1.Text = "Minimum Duration";
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new Point(129, 218);
      label2.Name = "label2";
      label2.Size = new Size(145, 37);
      label2.TabIndex = 12;
      label2.Text = "Threshold";
      // 
      // nudThreshold
      // 
      nudThreshold.DecimalPlaces = 2;
      nudThreshold.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
      nudThreshold.Location = new Point(280, 216);
      nudThreshold.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
      nudThreshold.Name = "nudThreshold";
      nudThreshold.Size = new Size(86, 43);
      nudThreshold.TabIndex = 11;
      nudThreshold.Value = new decimal(new int[] { 1, 0, 0, 131072 });
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Location = new Point(26, 26);
      label3.Name = "label3";
      label3.Size = new Size(369, 37);
      label3.TabIndex = 13;
      label3.Text = "Find Silent Sections Where?";
      // 
      // FrmEditEliminateSilence
      // 
      AutoScaleDimensions = new SizeF(16F, 37F);
      AutoScaleMode = AutoScaleMode.Font;
      BackColor = Color.Black;
      ClientSize = new Size(436, 372);
      Controls.Add(label3);
      Controls.Add(label2);
      Controls.Add(nudThreshold);
      Controls.Add(label1);
      Controls.Add(nudMinDuration);
      Controls.Add(chkRegionMiddle);
      Controls.Add(chkRegionEnd);
      Controls.Add(chkRegionBeginning);
      Controls.Add(btnCancel);
      Controls.Add(btnApply);
      Name = "FrmEditEliminateSilence";
      Text = "FrmEditEliminateSilence";
      ((System.ComponentModel.ISupportInitialize)nudMinDuration).EndInit();
      ((System.ComponentModel.ISupportInitialize)nudThreshold).EndInit();
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private Button btnCancel;
    private Button btnApply;
    private CheckBox chkRegionBeginning;
    private CheckBox chkRegionEnd;
    private CheckBox chkRegionMiddle;
    private NumericUpDown nudMinDuration;
    private Label label1;
    private Label label2;
    private NumericUpDown nudThreshold;
    private Label label3;
  }
}