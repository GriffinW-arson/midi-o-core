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
                Application.RemoveMessageFilter(this);
                trackManager?.Dispose();
                components.Dispose();
            }
            base.Dispose(disposing);
        }

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
			panTracks = new Panel();
			lblPlayhead = new Label();
			lblDebug = new Label();
			ofdLoadFile = new OpenFileDialog();
			menu = new MenuStrip();
			menuFile = new ToolStripMenuItem();
			menuFileOpen = new ToolStripMenuItem();
			menuFileSave = new ToolStripMenuItem();
			exportToToolStripMenuItem = new ToolStripMenuItem();
			menuEdit = new ToolStripMenuItem();
			menuEditDelete = new ToolStripMenuItem();
			toolStripSeparator3 = new ToolStripSeparator();
			menuEditFlip = new ToolStripMenuItem();
			menuEditReverse = new ToolStripMenuItem();
			menuEditVolume = new ToolStripMenuItem();
			menuEditEliminateSilence = new ToolStripMenuItem();
			toolStripSeparator4 = new ToolStripSeparator();
			normalizeToolStripMenuItem = new ToolStripMenuItem();
			phaserToolStripMenuItem = new ToolStripMenuItem();
			chorusToolStripMenuItem = new ToolStripMenuItem();
			menuGen = new ToolStripMenuItem();
			menuGenRecord = new ToolStripMenuItem();
			toolStripSeparator1 = new ToolStripSeparator();
			menuGenNoise = new ToolStripMenuItem();
			menuGenGradientNoise = new ToolStripMenuItem();
			viewToolStripMenuItem = new ToolStripMenuItem();
			zoomToolStripMenuItem = new ToolStripMenuItem();
			fitToolStripMenuItem = new ToolStripMenuItem();
			toolStripMenuItem2 = new ToolStripMenuItem();
			toolStripMenuItem3 = new ToolStripMenuItem();
			toolStripMenuItem4 = new ToolStripMenuItem();
			toolStripMenuItem5 = new ToolStripMenuItem();
			followPlayheadToolStripMenuItem = new ToolStripMenuItem();
			stretchTrackHeightToolStripMenuItem = new ToolStripMenuItem();
			toolStripSeparator2 = new ToolStripSeparator();
			appearanceToolStripMenuItem = new ToolStripMenuItem();
			defaultToolStripMenuItem = new ToolStripMenuItem();
			redToolStripMenuItem = new ToolStripMenuItem();
			greenToolStripMenuItem = new ToolStripMenuItem();
			blueToolStripMenuItem = new ToolStripMenuItem();
			darkToolStripMenuItem = new ToolStripMenuItem();
			lightToolStripMenuItem = new ToolStripMenuItem();
			btnPlay = new Button();
			btnReset = new Button();
			btnEnd = new Button();
			btnSave = new Button();
			btnLoad = new Button();
			transportFeedback = new System.Windows.Forms.Timer(components);
			tmrUpdate = new System.Windows.Forms.Timer(components);
			sfdSaveFile = new SaveFileDialog();
			trkDrawTimescale = new TrackBar();
			timelineScroll = new HScrollBar();
			lblPlayheadTime = new Label();
			chkFollowPlayback = new CheckBox();
			colorDialog = new ColorDialog();
			panTracks.SuspendLayout();
			menu.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)trkDrawTimescale).BeginInit();
			SuspendLayout();
			// 
			// panTracks
			// 
			panTracks.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			panTracks.BackColor = Color.DimGray;
			panTracks.BorderStyle = BorderStyle.Fixed3D;
			panTracks.Controls.Add(lblPlayhead);
			panTracks.Location = new Point(15, 102);
			panTracks.Name = "panTracks";
			panTracks.Size = new Size(1574, 479);
			panTracks.TabIndex = 0;
			panTracks.SizeChanged += panTracks_SizeChanged;
			panTracks.Paint += panTracks_Paint;
			panTracks.MouseDown += waveform_MouseDown;
			// 
			// lblPlayhead
			// 
			lblPlayhead.BackColor = Color.White;
			lblPlayhead.Location = new Point(39, -2);
			lblPlayhead.Name = "lblPlayhead";
			lblPlayhead.Size = new Size(5, 385);
			lblPlayhead.TabIndex = 9;
			lblPlayhead.MouseDown += waveform_MouseDown;
			// 
			// lblDebug
			// 
			lblDebug.Dock = DockStyle.Bottom;
			lblDebug.Font = new Font("Segoe UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
			lblDebug.ForeColor = Color.FromArgb(255, 255, 192);
			lblDebug.Location = new Point(0, 746);
			lblDebug.Name = "lblDebug";
			lblDebug.Size = new Size(1604, 37);
			lblDebug.TabIndex = 1;
			lblDebug.Text = "0.00s – 0.00s  /  0.00s";
			// 
			// ofdLoadFile
			// 
			ofdLoadFile.FileName = "openFileDialog1";
			// 
			// menu
			// 
			menu.Items.AddRange(new ToolStripItem[] { menuFile, menuEdit, menuGen, viewToolStripMenuItem });
			menu.Location = new Point(0, 0);
			menu.Name = "menu";
			menu.Size = new Size(1604, 24);
			menu.TabIndex = 5;
			menu.Text = "menuStrip1";
			// 
			// menuFile
			// 
			menuFile.DropDownItems.AddRange(new ToolStripItem[] { menuFileOpen, menuFileSave, exportToToolStripMenuItem });
			menuFile.Name = "menuFile";
			menuFile.Size = new Size(37, 20);
			menuFile.Text = "&File";
			// 
			// menuFileOpen
			// 
			menuFileOpen.Name = "menuFileOpen";
			menuFileOpen.Size = new Size(132, 22);
			menuFileOpen.Text = "Open...";
			menuFileOpen.Click += menuFileOpen_Click;
			// 
			// menuFileSave
			// 
			menuFileSave.Name = "menuFileSave";
			menuFileSave.Size = new Size(132, 22);
			menuFileSave.Text = "Save...";
			menuFileSave.Click += menuFileSave_Click;
			// 
			// exportToToolStripMenuItem
			// 
			exportToToolStripMenuItem.Name = "exportToToolStripMenuItem";
			exportToToolStripMenuItem.Size = new Size(132, 22);
			exportToToolStripMenuItem.Text = "Export To...";
			// 
			// menuEdit
			// 
			menuEdit.DropDownItems.AddRange(new ToolStripItem[] { menuEditDelete, toolStripSeparator3, menuEditFlip, menuEditReverse, menuEditVolume, menuEditEliminateSilence, toolStripSeparator4, normalizeToolStripMenuItem, phaserToolStripMenuItem, chorusToolStripMenuItem });
			menuEdit.Name = "menuEdit";
			menuEdit.Size = new Size(39, 20);
			menuEdit.Text = "E&dit";
			// 
			// menuEditDelete
			// 
			menuEditDelete.Enabled = false;
			menuEditDelete.Name = "menuEditDelete";
			menuEditDelete.ShortcutKeys = Keys.Delete;
			menuEditDelete.Size = new Size(180, 22);
			menuEditDelete.Text = "Delete";
			menuEditDelete.Click += menuEditDelete_Click;
			// 
			// toolStripSeparator3
			// 
			toolStripSeparator3.Name = "toolStripSeparator3";
			toolStripSeparator3.Size = new Size(177, 6);
			// 
			// menuEditFlip
			// 
			menuEditFlip.Name = "menuEditFlip";
			menuEditFlip.Size = new Size(180, 22);
			menuEditFlip.Text = "Flip";
			menuEditFlip.Click += menuEditFlip_Click;
			// 
			// menuEditReverse
			// 
			menuEditReverse.Name = "menuEditReverse";
			menuEditReverse.Size = new Size(180, 22);
			menuEditReverse.Text = "Reverse";
			menuEditReverse.Click += menuEditReverse_Click;
			// 
			// menuEditVolume
			// 
			menuEditVolume.Name = "menuEditVolume";
			menuEditVolume.Size = new Size(180, 22);
			menuEditVolume.Text = "Alter Volume...";
			menuEditVolume.Click += menuEditVolume_Click;
			// 
			// menuEditEliminateSilence
			// 
			menuEditEliminateSilence.Name = "menuEditEliminateSilence";
			menuEditEliminateSilence.Size = new Size(180, 22);
			menuEditEliminateSilence.Text = "Eliminate Silence...";
			menuEditEliminateSilence.Click += menuEditEliminateSilence_Click;
			// 
			// toolStripSeparator4
			// 
			toolStripSeparator4.Name = "toolStripSeparator4";
			toolStripSeparator4.Size = new Size(177, 6);
			// 
			// normalizeToolStripMenuItem
			// 
			normalizeToolStripMenuItem.Name = "normalizeToolStripMenuItem";
			normalizeToolStripMenuItem.Size = new Size(180, 22);
			normalizeToolStripMenuItem.Text = "Normalize";
			normalizeToolStripMenuItem.Click += normalizeToolStripMenuItem_Click;
			// 
			// phaserToolStripMenuItem
			// 
			phaserToolStripMenuItem.Name = "phaserToolStripMenuItem";
			phaserToolStripMenuItem.Size = new Size(180, 22);
			phaserToolStripMenuItem.Text = "Phaser";
			phaserToolStripMenuItem.Click += phaserToolStripMenuItem_Click;
			// 
			// chorusToolStripMenuItem
			// 
			chorusToolStripMenuItem.Name = "chorusToolStripMenuItem";
			chorusToolStripMenuItem.Size = new Size(180, 22);
			chorusToolStripMenuItem.Text = "Chorus";
			chorusToolStripMenuItem.Click += chorusToolStripMenuItem_Click;
			// 
			// menuGen
			// 
			menuGen.DropDownItems.AddRange(new ToolStripItem[] { menuGenRecord, toolStripSeparator1, menuGenNoise, menuGenGradientNoise });
			menuGen.Name = "menuGen";
			menuGen.Size = new Size(66, 20);
			menuGen.Text = "&Generate";
			// 
			// menuGenRecord
			// 
			menuGenRecord.Name = "menuGenRecord";
			menuGenRecord.Size = new Size(152, 22);
			menuGenRecord.Text = "Record...";
			menuGenRecord.Click += menuGenRecord_Click;
			// 
			// toolStripSeparator1
			// 
			toolStripSeparator1.Name = "toolStripSeparator1";
			toolStripSeparator1.Size = new Size(149, 6);
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
			// viewToolStripMenuItem
			// 
			viewToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { zoomToolStripMenuItem, followPlayheadToolStripMenuItem, stretchTrackHeightToolStripMenuItem, toolStripSeparator2, appearanceToolStripMenuItem });
			viewToolStripMenuItem.Name = "viewToolStripMenuItem";
			viewToolStripMenuItem.Size = new Size(44, 20);
			viewToolStripMenuItem.Text = "View";
			// 
			// zoomToolStripMenuItem
			// 
			zoomToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { fitToolStripMenuItem, toolStripMenuItem2, toolStripMenuItem3, toolStripMenuItem4, toolStripMenuItem5 });
			zoomToolStripMenuItem.Name = "zoomToolStripMenuItem";
			zoomToolStripMenuItem.Size = new Size(160, 22);
			zoomToolStripMenuItem.Text = "Zoom";
			// 
			// fitToolStripMenuItem
			// 
			fitToolStripMenuItem.Name = "fitToolStripMenuItem";
			fitToolStripMenuItem.Size = new Size(126, 22);
			fitToolStripMenuItem.Text = "100% (Fit)";
			fitToolStripMenuItem.Click += fitZoom_Click;
			// 
			// toolStripMenuItem2
			// 
			toolStripMenuItem2.Name = "toolStripMenuItem2";
			toolStripMenuItem2.Size = new Size(126, 22);
			toolStripMenuItem2.Text = "50%";
			toolStripMenuItem2.Click += zoomHalf_Click;
			// 
			// toolStripMenuItem3
			// 
			toolStripMenuItem3.Name = "toolStripMenuItem3";
			toolStripMenuItem3.Size = new Size(126, 22);
			toolStripMenuItem3.Text = "25%";
			toolStripMenuItem3.Click += zoomQuarter_Click;
			// 
			// toolStripMenuItem4
			// 
			toolStripMenuItem4.Name = "toolStripMenuItem4";
			toolStripMenuItem4.Size = new Size(126, 22);
			toolStripMenuItem4.Text = "10%";
			toolStripMenuItem4.Click += zoomTenth_Click;
			// 
			// toolStripMenuItem5
			// 
			toolStripMenuItem5.Name = "toolStripMenuItem5";
			toolStripMenuItem5.Size = new Size(126, 22);
			toolStripMenuItem5.Text = "1%";
			toolStripMenuItem5.Click += zoomHundredth_Click;
			// 
			// followPlayheadToolStripMenuItem
			// 
			followPlayheadToolStripMenuItem.Name = "followPlayheadToolStripMenuItem";
			followPlayheadToolStripMenuItem.Size = new Size(160, 22);
			followPlayheadToolStripMenuItem.Text = "Follow Playhead";
			followPlayheadToolStripMenuItem.Click += followPlayheadToolStripMenuItem_Click;
			//
			// stretchTrackHeightToolStripMenuItem
			//
			stretchTrackHeightToolStripMenuItem.CheckOnClick = true;
			stretchTrackHeightToolStripMenuItem.Name = "stretchTrackHeightToolStripMenuItem";
			stretchTrackHeightToolStripMenuItem.Text = "Stretch Track Height";
			stretchTrackHeightToolStripMenuItem.CheckedChanged += stretchTrackHeightToolStripMenuItem_CheckedChanged;
			// 
			// toolStripSeparator2
			// 
			toolStripSeparator2.Name = "toolStripSeparator2";
			toolStripSeparator2.Size = new Size(157, 6);
			// 
			// appearanceToolStripMenuItem
			// 
			appearanceToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { defaultToolStripMenuItem, redToolStripMenuItem, greenToolStripMenuItem, blueToolStripMenuItem, darkToolStripMenuItem, lightToolStripMenuItem });
			appearanceToolStripMenuItem.Name = "appearanceToolStripMenuItem";
			appearanceToolStripMenuItem.Size = new Size(160, 22);
			appearanceToolStripMenuItem.Text = "Appearance";
			appearanceToolStripMenuItem.Click += appearanceToolStripMenuItem_Click;
			// 
			// defaultToolStripMenuItem
			// 
			defaultToolStripMenuItem.Name = "defaultToolStripMenuItem";
			defaultToolStripMenuItem.Size = new Size(112, 22);
			defaultToolStripMenuItem.Text = "Default";
			defaultToolStripMenuItem.Click += defaultToolStripMenuItem_Click;
			// 
			// redToolStripMenuItem
			// 
			redToolStripMenuItem.Name = "redToolStripMenuItem";
			redToolStripMenuItem.Size = new Size(112, 22);
			redToolStripMenuItem.Text = "Dark";
			redToolStripMenuItem.Click += redToolStripMenuItem_Click;
			// 
			// greenToolStripMenuItem
			// 
			greenToolStripMenuItem.Name = "greenToolStripMenuItem";
			greenToolStripMenuItem.Size = new Size(112, 22);
			greenToolStripMenuItem.Text = "Light";
			greenToolStripMenuItem.Click += greenToolStripMenuItem_Click;
			// 
			// blueToolStripMenuItem
			// 
			blueToolStripMenuItem.Name = "blueToolStripMenuItem";
			blueToolStripMenuItem.Size = new Size(112, 22);
			blueToolStripMenuItem.Text = "Red";
			blueToolStripMenuItem.Click += blueToolStripMenuItem_Click;
			// 
			// darkToolStripMenuItem
			// 
			darkToolStripMenuItem.Name = "darkToolStripMenuItem";
			darkToolStripMenuItem.Size = new Size(112, 22);
			darkToolStripMenuItem.Text = "Green";
			darkToolStripMenuItem.Click += darkToolStripMenuItem_Click;
			// 
			// lightToolStripMenuItem
			// 
			lightToolStripMenuItem.Name = "lightToolStripMenuItem";
			lightToolStripMenuItem.Size = new Size(112, 22);
			lightToolStripMenuItem.Text = "Blue";
			lightToolStripMenuItem.Click += lightToolStripMenuItem_Click;
			// 
			// btnPlay
			// 
			btnPlay.AccessibleName = "Play";
			btnPlay.Anchor = AnchorStyles.Bottom;
			btnPlay.BackColor = Color.Transparent;
			btnPlay.BackgroundImage = Properties.Resources.transportPlay;
			btnPlay.BackgroundImageLayout = ImageLayout.Zoom;
			btnPlay.FlatAppearance.BorderSize = 0;
			btnPlay.FlatAppearance.MouseDownBackColor = Color.Transparent;
			btnPlay.FlatAppearance.MouseOverBackColor = Color.Transparent;
			btnPlay.FlatStyle = FlatStyle.Flat;
			btnPlay.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
			btnPlay.Location = new Point(772, 609);
			btnPlay.Name = "btnPlay";
			btnPlay.Size = new Size(60, 60);
			btnPlay.TabIndex = 6;
			btnPlay.UseVisualStyleBackColor = false;
			btnPlay.Click += btnPlay_Click;
			btnPlay.MouseDown += transportButton_MouseDown;
			// 
			// btnReset
			// 
			btnReset.AccessibleName = "Reset to beginning";
			btnReset.Anchor = AnchorStyles.Bottom;
			btnReset.BackColor = Color.Transparent;
			btnReset.BackgroundImage = Properties.Resources.transportReset;
			btnReset.BackgroundImageLayout = ImageLayout.Zoom;
			btnReset.FlatAppearance.BorderSize = 0;
			btnReset.FlatAppearance.MouseDownBackColor = Color.Transparent;
			btnReset.FlatAppearance.MouseOverBackColor = Color.Transparent;
			btnReset.FlatStyle = FlatStyle.Flat;
			btnReset.Location = new Point(704, 609);
			btnReset.Name = "btnReset";
			btnReset.Size = new Size(60, 60);
			btnReset.TabIndex = 5;
			btnReset.UseVisualStyleBackColor = false;
			btnReset.Click += btnReset_Click;
			btnReset.MouseDown += transportButton_MouseDown;
			// 
			// btnEnd
			// 
			btnEnd.AccessibleName = "Go to end";
			btnEnd.Anchor = AnchorStyles.Bottom;
			btnEnd.BackColor = Color.Transparent;
			btnEnd.BackgroundImage = Properties.Resources.transportEnd;
			btnEnd.BackgroundImageLayout = ImageLayout.Zoom;
			btnEnd.FlatAppearance.BorderSize = 0;
			btnEnd.FlatAppearance.MouseDownBackColor = Color.Transparent;
			btnEnd.FlatAppearance.MouseOverBackColor = Color.Transparent;
			btnEnd.FlatStyle = FlatStyle.Flat;
			btnEnd.Location = new Point(840, 609);
			btnEnd.Name = "btnEnd";
			btnEnd.Size = new Size(60, 60);
			btnEnd.TabIndex = 8;
			btnEnd.UseVisualStyleBackColor = false;
			btnEnd.Click += btnEnd_Click;
			btnEnd.MouseDown += transportButton_MouseDown;
			//
			// btnSave
			//
			btnSave.AccessibleName = "Save audio";
			btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			btnSave.BackColor = Color.Transparent;
			btnSave.BackgroundImage = Properties.Resources.transportSave;
			btnSave.BackgroundImageLayout = ImageLayout.Zoom;
			btnSave.FlatAppearance.BorderSize = 0;
			btnSave.FlatAppearance.MouseDownBackColor = Color.Transparent;
			btnSave.FlatAppearance.MouseOverBackColor = Color.Transparent;
			btnSave.FlatStyle = FlatStyle.Flat;
			btnSave.Location = new Point(1461, 609);
			btnSave.Name = "btnSave";
			btnSave.Size = new Size(60, 60);
			btnSave.TabIndex = 13;
			btnSave.UseVisualStyleBackColor = false;
			btnSave.Click += btnSave_Click;
			btnSave.MouseDown += transportButton_MouseDown;
			//
			// btnLoad
			//
			btnLoad.AccessibleName = "Load audio";
			btnLoad.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			btnLoad.BackColor = Color.Transparent;
			btnLoad.BackgroundImage = Properties.Resources.transportLoad;
			btnLoad.BackgroundImageLayout = ImageLayout.Zoom;
			btnLoad.FlatAppearance.BorderSize = 0;
			btnLoad.FlatAppearance.MouseDownBackColor = Color.Transparent;
			btnLoad.FlatAppearance.MouseOverBackColor = Color.Transparent;
			btnLoad.FlatStyle = FlatStyle.Flat;
			btnLoad.Location = new Point(1529, 609);
			btnLoad.Name = "btnLoad";
			btnLoad.Size = new Size(60, 60);
			btnLoad.TabIndex = 14;
			btnLoad.UseVisualStyleBackColor = false;
			btnLoad.Click += btnLoad_Click;
			btnLoad.MouseDown += transportButton_MouseDown;
			// 
			// transportFeedback
			// 
			transportFeedback.Interval = 80;
			transportFeedback.Tick += transportFeedback_Tick;
			// 
			// tmrUpdate
			// 
			tmrUpdate.Enabled = true;
			tmrUpdate.Interval = 20;
			tmrUpdate.Tick += tmrUpdatePlaybackState_Tick;
			// 
			// trkDrawTimescale
			// 
			trkDrawTimescale.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			trkDrawTimescale.LargeChange = 50;
			trkDrawTimescale.Location = new Point(15, 55);
			trkDrawTimescale.Maximum = 1000;
			trkDrawTimescale.Minimum = 1;
			trkDrawTimescale.Name = "trkDrawTimescale";
			trkDrawTimescale.RightToLeft = RightToLeft.Yes;
			trkDrawTimescale.Size = new Size(1574, 45);
			trkDrawTimescale.SmallChange = 10;
			trkDrawTimescale.TabIndex = 7;
			trkDrawTimescale.TickFrequency = 10;
			trkDrawTimescale.Value = 1000;
			trkDrawTimescale.Scroll += trkDrawTimescale_Scroll;
			// 
			// timelineScroll
			// 
			timelineScroll.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			timelineScroll.Location = new Point(15, 584);
			timelineScroll.Name = "timelineScroll";
			timelineScroll.Size = new Size(1574, 17);
			timelineScroll.TabIndex = 10;
			timelineScroll.Scroll += timelineScroll_Scroll;
			// 
			// lblPlayheadTime
			// 
			lblPlayheadTime.AccessibleName = "Current playhead time";
			lblPlayheadTime.Anchor = AnchorStyles.Bottom;
			lblPlayheadTime.Font = new Font("Consolas", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
			lblPlayheadTime.ForeColor = Color.White;
			lblPlayheadTime.Location = new Point(704, 677);
			lblPlayheadTime.Name = "lblPlayheadTime";
			lblPlayheadTime.Size = new Size(196, 26);
			lblPlayheadTime.TabIndex = 11;
			lblPlayheadTime.Text = "00:00:00.000";
			lblPlayheadTime.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// chkFollowPlayback
			// 
			chkFollowPlayback.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			chkFollowPlayback.AutoSize = true;
			chkFollowPlayback.ForeColor = Color.White;
			chkFollowPlayback.Location = new Point(15, 613);
			chkFollowPlayback.Name = "chkFollowPlayback";
			chkFollowPlayback.Size = new Size(111, 19);
			chkFollowPlayback.TabIndex = 12;
			chkFollowPlayback.Text = "Follow playback";
			chkFollowPlayback.UseVisualStyleBackColor = true;
			chkFollowPlayback.CheckedChanged += chkFollowPlayback_CheckedChanged;
			// 
			// FrmMain
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = Color.Black;
			ClientSize = new Size(1604, 783);
			Controls.Add(chkFollowPlayback);
			Controls.Add(lblPlayheadTime);
			Controls.Add(timelineScroll);
			Controls.Add(trkDrawTimescale);
			Controls.Add(btnPlay);
			Controls.Add(btnReset);
			Controls.Add(btnEnd);
			Controls.Add(btnSave);
			Controls.Add(btnLoad);
			Controls.Add(lblDebug);
			Controls.Add(panTracks);
			Controls.Add(menu);
			Icon = (Icon)resources.GetObject("$this.Icon");
			KeyPreview = true;
			MainMenuStrip = menu;
			MinimumSize = new Size(720, 500);
			Name = "FrmMain";
			Text = "Mediocre Sound Designer";
			SizeChanged += FrmMain_SizeChanged;
			panTracks.ResumeLayout(false);
			menu.ResumeLayout(false);
			menu.PerformLayout();
			((System.ComponentModel.ISupportInitialize)trkDrawTimescale).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Panel panTracks;
        private HScrollBar timelineScroll;
        private Label lblPlayheadTime;
        private CheckBox chkFollowPlayback;
    private Label lblDebug;
    private OpenFileDialog ofdLoadFile;
    private MenuStrip menu;
    private ToolStripMenuItem menuEdit;
    private Button btnPlay;
    private Button btnReset;
    private Button btnEnd;
    private Button btnSave;
    private Button btnLoad;
    private System.Windows.Forms.Timer transportFeedback;
    private ToolStripMenuItem menuEditVolume;
    private ToolStripMenuItem menuEditReverse;
    private ToolStripMenuItem menuFile;
    private ToolStripMenuItem menuFileOpen;
    private ToolStripMenuItem menuFileSave;
    private System.Windows.Forms.Timer tmrUpdate;
    private ToolStripMenuItem menuGen;
    private ToolStripMenuItem menuGenNoise;
    private ToolStripMenuItem menuGenGradientNoise;
    private ToolStripMenuItem menuEditFlip;
    private SaveFileDialog sfdSaveFile;
    private TrackBar trkDrawTimescale;
    private ColorDialog colorDialog;
    private ToolStripMenuItem menuEditEliminateSilence;
    private ToolStripMenuItem menuEditDelete;
    private Label lblPlayhead;
    private ToolStripMenuItem menuGenRecord;
        private ToolStripMenuItem exportToToolStripMenuItem;
		private ToolStripMenuItem viewToolStripMenuItem;
		private ToolStripMenuItem zoomToolStripMenuItem;
		private ToolStripMenuItem fitToolStripMenuItem;
		private ToolStripMenuItem toolStripMenuItem2;
		private ToolStripMenuItem toolStripMenuItem3;
		private ToolStripMenuItem toolStripMenuItem4;
		private ToolStripMenuItem toolStripMenuItem5;
		private ToolStripMenuItem followPlayheadToolStripMenuItem;
		private ToolStripMenuItem stretchTrackHeightToolStripMenuItem;
        private ToolStripMenuItem appearanceToolStripMenuItem;
        private ToolStripMenuItem defaultToolStripMenuItem;
        private ToolStripMenuItem redToolStripMenuItem;
        private ToolStripMenuItem greenToolStripMenuItem;
        private ToolStripMenuItem blueToolStripMenuItem;
        private ToolStripMenuItem darkToolStripMenuItem;
        private ToolStripMenuItem lightToolStripMenuItem;
		private ToolStripSeparator toolStripSeparator2;
		private ToolStripSeparator toolStripSeparator1;
		private ToolStripSeparator toolStripSeparator3;
		private ToolStripMenuItem normalizeToolStripMenuItem;
		private ToolStripSeparator toolStripSeparator4;
		private ToolStripMenuItem phaserToolStripMenuItem;
		private ToolStripMenuItem chorusToolStripMenuItem;
	}
}
