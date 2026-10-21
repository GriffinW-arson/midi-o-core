namespace MediocreSoundDesigner
{
    partial class FrmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      components = new System.ComponentModel.Container();
      panWaveform = new Panel();
      lblDebug = new Label();
      ofdLoadFile = new OpenFileDialog();
      menuStrip1 = new MenuStrip();
      fileToolStripMenuItem = new ToolStripMenuItem();
      menuFileOpen = new ToolStripMenuItem();
      menuFileSave = new ToolStripMenuItem();
      menu = new ToolStripMenuItem();
      menuEditPitch = new ToolStripMenuItem();
      menuEditVolume = new ToolStripMenuItem();
      menuEditReverse = new ToolStripMenuItem();
      menuEditFlip = new ToolStripMenuItem();
      generateToolStripMenuItem = new ToolStripMenuItem();
      menuGenNoise = new ToolStripMenuItem();
      menuGenGradientNoise = new ToolStripMenuItem();
      btnPlay = new Button();
      tmrUpdatePlaybackState = new System.Windows.Forms.Timer(components);
      sfdSaveFile = new SaveFileDialog();
      menuStrip1.SuspendLayout();
      SuspendLayout();
      // 
      // panWaveform
      // 
      panWaveform.AutoScroll = true;
      panWaveform.BackColor = Color.DimGray;
      panWaveform.BorderStyle = BorderStyle.Fixed3D;
      panWaveform.Location = new Point(12, 102);
      panWaveform.Name = "panWaveform";
      panWaveform.Size = new Size(1580, 354);
      panWaveform.TabIndex = 0;
      panWaveform.Paint += panWaveform_Paint;
      // 
      // lblDebug
      // 
      lblDebug.Dock = DockStyle.Bottom;
      lblDebug.Font = new Font("Segoe UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
      lblDebug.ForeColor = Color.FromArgb(255, 255, 192);
      lblDebug.Location = new Point(0, 602);
      lblDebug.Name = "lblDebug";
      lblDebug.Size = new Size(1604, 37);
      lblDebug.TabIndex = 1;
      // 
      // ofdLoadFile
      // 
      ofdLoadFile.FileName = "openFileDialog1";
      // 
      // menuStrip1
      // 
      menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, menu, generateToolStripMenuItem });
      menuStrip1.Location = new Point(0, 0);
      menuStrip1.Name = "menuStrip1";
      menuStrip1.Size = new Size(1604, 24);
      menuStrip1.TabIndex = 5;
      menuStrip1.Text = "menuStrip1";
      // 
      // fileToolStripMenuItem
      // 
      fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { menuFileOpen, menuFileSave });
      fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      fileToolStripMenuItem.Size = new Size(37, 20);
      fileToolStripMenuItem.Text = "&File";
      // 
      // menuFileOpen
      // 
      menuFileOpen.Name = "menuFileOpen";
      menuFileOpen.Size = new Size(112, 22);
      menuFileOpen.Text = "Open...";
      menuFileOpen.Click += menuFileOpen_Click;
      // 
      // menuFileSave
      // 
      menuFileSave.Name = "menuFileSave";
      menuFileSave.Size = new Size(112, 22);
      menuFileSave.Text = "Save...";
      menuFileSave.Click += menuFileSave_Click;
      // 
      // menu
      // 
      menu.DropDownItems.AddRange(new ToolStripItem[] { menuEditPitch, menuEditVolume, menuEditReverse, menuEditFlip });
      menu.Name = "menu";
      menu.Size = new Size(39, 20);
      menu.Text = "E&dit";
      // 
      // menuEditPitch
      // 
      menuEditPitch.Name = "menuEditPitch";
      menuEditPitch.Size = new Size(151, 22);
      menuEditPitch.Text = "Alter Pitch...";
      // 
      // menuEditVolume
      // 
      menuEditVolume.Name = "menuEditVolume";
      menuEditVolume.Size = new Size(151, 22);
      menuEditVolume.Text = "Alter Volume...";
      // 
      // menuEditReverse
      // 
      menuEditReverse.Name = "menuEditReverse";
      menuEditReverse.Size = new Size(151, 22);
      menuEditReverse.Text = "Reverse";
      menuEditReverse.Click += menuEditReverse_Click;
      // 
      // menuEditFlip
      // 
      menuEditFlip.Name = "menuEditFlip";
      menuEditFlip.Size = new Size(151, 22);
      menuEditFlip.Text = "Flip";
      menuEditFlip.Click += menuEditFlip_Click;
      // 
      // generateToolStripMenuItem
      // 
      generateToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { menuGenNoise, menuGenGradientNoise });
      generateToolStripMenuItem.Name = "generateToolStripMenuItem";
      generateToolStripMenuItem.Size = new Size(66, 20);
      generateToolStripMenuItem.Text = "&Generate";
      // 
      // menuGenNoise
      // 
      menuGenNoise.Name = "menuGenNoise";
      menuGenNoise.Size = new Size(152, 22);
      menuGenNoise.Text = "Noise";
      menuGenNoise.Click += menuGenNoise_Click;
      // 
      // menuGenGradientNoise
      // 
      menuGenGradientNoise.Name = "menuGenGradientNoise";
      menuGenGradientNoise.Size = new Size(152, 22);
      menuGenGradientNoise.Text = "Gradient Noise";
      menuGenGradientNoise.Click += menuGenGradientNoise_Click;
      // 
      // btnPlay
      // 
      btnPlay.AutoSize = true;
      btnPlay.BackColor = Color.Transparent;
      btnPlay.BackgroundImage = Properties.Resources.play;
      btnPlay.BackgroundImageLayout = ImageLayout.Stretch;
      btnPlay.FlatAppearance.BorderSize = 0;
      btnPlay.FlatAppearance.MouseDownBackColor = Color.Transparent;
      btnPlay.FlatAppearance.MouseOverBackColor = Color.Transparent;
      btnPlay.FlatStyle = FlatStyle.Flat;
      btnPlay.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
      btnPlay.Location = new Point(764, 462);
      btnPlay.Name = "btnPlay";
      btnPlay.Size = new Size(100, 100);
      btnPlay.TabIndex = 6;
      btnPlay.UseVisualStyleBackColor = false;
      btnPlay.Click += btnPlay_Click;
      // 
      // tmrUpdatePlaybackState
      // 
      tmrUpdatePlaybackState.Enabled = true;
      tmrUpdatePlaybackState.Tick += tmrUpdatePlaybackState_Tick;
      // 
      // Form1
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      BackColor = Color.Black;
      ClientSize = new Size(1604, 639);
      Controls.Add(btnPlay);
      Controls.Add(lblDebug);
      Controls.Add(panWaveform);
      Controls.Add(menuStrip1);
      MainMenuStrip = menuStrip1;
      Name = "Form1";
      Text = "Mediocre Sound Designer";
      menuStrip1.ResumeLayout(false);
      menuStrip1.PerformLayout();
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private Panel panWaveform;
    private Label lblDebug;
    private OpenFileDialog ofdLoadFile;
    private MenuStrip menuStrip1;
    private ToolStripMenuItem menu;
    private ToolStripMenuItem menuEditPitch;
    private Button btnPlay;
    private ToolStripMenuItem menuEditVolume;
    private ToolStripMenuItem menuEditReverse;
    private ToolStripMenuItem fileToolStripMenuItem;
    private ToolStripMenuItem menuFileOpen;
    private ToolStripMenuItem menuFileSave;
    private System.Windows.Forms.Timer tmrUpdatePlaybackState;
    private ToolStripMenuItem generateToolStripMenuItem;
    private ToolStripMenuItem menuGenNoise;
    private ToolStripMenuItem menuGenGradientNoise;
    private ToolStripMenuItem menuEditFlip;
    private SaveFileDialog sfdSaveFile;
  }
}
