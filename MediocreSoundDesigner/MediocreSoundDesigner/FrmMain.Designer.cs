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
			buttonToolTips = new ToolTip(components);
			btnPlay = new Button();
			btnReset = new Button();
			btnEnd = new Button();
			btnSave = new Button();
			btnLoad = new Button();
			btnCut = new Button();
			btnCopy = new Button();
			btnPaste = new Button();
			btnPasteNewTrack = new Button();
			btnDelete = new Button();
			panTracks = new Panel();
			lblPlayhead = new Label();
			lblTimelineLeft = new Label();
			lblTimelineRight = new Label();
			ofdLoadFile = new OpenFileDialog();
			menu = new MenuStrip();
			menuFile = new ToolStripMenuItem();
			menuFileOpen = new ToolStripMenuItem();
			menuFileSave = new ToolStripMenuItem();
			menuFileExit = new ToolStripMenuItem();
			menuEdit = new ToolStripMenuItem();
			menuEditCut = new ToolStripMenuItem();
			menuEditCopy = new ToolStripMenuItem();
			menuEditPaste = new ToolStripMenuItem();
			menuEditPasteNewTrack = new ToolStripMenuItem();
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
			buttonToolTips.SetToolTip(btnPlay, "Play");
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
			buttonToolTips.SetToolTip(btnReset, "Reset");
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
			buttonToolTips.SetToolTip(btnEnd, "End");
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
			buttonToolTips.SetToolTip(btnSave, "Save");
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
			buttonToolTips.SetToolTip(btnLoad, "Open");
			btnLoad.UseVisualStyleBackColor = false;
			btnLoad.Click += btnLoad_Click;
			btnLoad.MouseDown += transportButton_MouseDown;
			// 
			// btnCut
			// 
			btnCut.AccessibleName = "Cut";
			btnCut.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			btnCut.BackColor = Color.Transparent;
			btnCut.BackgroundImage = Properties.Resources.editCut;
			btnCut.BackgroundImageLayout = ImageLayout.Zoom;
			btnCut.FlatAppearance.BorderSize = 0;
			btnCut.FlatAppearance.MouseDownBackColor = Color.Transparent;
			btnCut.FlatAppearance.MouseOverBackColor = Color.Transparent;
			btnCut.FlatStyle = FlatStyle.Flat;
			btnCut.Location = new Point(15, 609);
			btnCut.Name = "btnCut";
			btnCut.Size = new Size(60, 60);
			btnCut.TabIndex = 15;
			buttonToolTips.SetToolTip(btnCut, "Cut");
			btnCut.UseVisualStyleBackColor = false;
			btnCut.Click += editButton_Click;
			btnCut.MouseDown += transportButton_MouseDown;
			// 
			// btnCopy
			// 
			btnCopy.AccessibleName = "Copy";
			btnCopy.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			btnCopy.BackColor = Color.Transparent;
			btnCopy.BackgroundImage = Properties.Resources.editCopy;
			btnCopy.BackgroundImageLayout = ImageLayout.Zoom;
			btnCopy.FlatAppearance.BorderSize = 0;
			btnCopy.FlatAppearance.MouseDownBackColor = Color.Transparent;
			btnCopy.FlatAppearance.MouseOverBackColor = Color.Transparent;
			btnCopy.FlatStyle = FlatStyle.Flat;
			btnCopy.Location = new Point(83, 609);
			btnCopy.Name = "btnCopy";
			btnCopy.Size = new Size(60, 60);
			btnCopy.TabIndex = 16;
			buttonToolTips.SetToolTip(btnCopy, "Copy");
			btnCopy.UseVisualStyleBackColor = false;
			btnCopy.Click += editButton_Click;
			btnCopy.MouseDown += transportButton_MouseDown;
			// 
			// btnPaste
			// 
			btnPaste.AccessibleName = "Paste";
			btnPaste.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			btnPaste.BackColor = Color.Transparent;
			btnPaste.BackgroundImage = Properties.Resources.editPaste;
			btnPaste.BackgroundImageLayout = ImageLayout.Zoom;
			btnPaste.FlatAppearance.BorderSize = 0;
			btnPaste.FlatAppearance.MouseDownBackColor = Color.Transparent;
			btnPaste.FlatAppearance.MouseOverBackColor = Color.Transparent;
			btnPaste.FlatStyle = FlatStyle.Flat;
			btnPaste.Location = new Point(151, 609);
			btnPaste.Name = "btnPaste";
			btnPaste.Size = new Size(60, 60);
			btnPaste.TabIndex = 17;
			buttonToolTips.SetToolTip(btnPaste, "Paste");
			btnPaste.UseVisualStyleBackColor = false;
			btnPaste.Click += editButton_Click;
			btnPaste.MouseDown += transportButton_MouseDown;
			// 
			// btnPasteNewTrack
			// 
			btnPasteNewTrack.AccessibleName = "Paste to New Track";
			btnPasteNewTrack.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			btnPasteNewTrack.BackColor = Color.Transparent;
			btnPasteNewTrack.BackgroundImage = Properties.Resources.editPasteNewTrack;
			btnPasteNewTrack.BackgroundImageLayout = ImageLayout.Zoom;
			btnPasteNewTrack.FlatAppearance.BorderSize = 0;
			btnPasteNewTrack.FlatAppearance.MouseDownBackColor = Color.Transparent;
			btnPasteNewTrack.FlatAppearance.MouseOverBackColor = Color.Transparent;
			btnPasteNewTrack.FlatStyle = FlatStyle.Flat;
			btnPasteNewTrack.Location = new Point(219, 609);
			btnPasteNewTrack.Name = "btnPasteNewTrack";
			btnPasteNewTrack.Size = new Size(60, 60);
			btnPasteNewTrack.TabIndex = 18;
			buttonToolTips.SetToolTip(btnPasteNewTrack, "Paste to New Track");
			btnPasteNewTrack.UseVisualStyleBackColor = false;
			btnPasteNewTrack.Click += editButton_Click;
			btnPasteNewTrack.MouseDown += transportButton_MouseDown;
			// 
			// btnDelete
			// 
			btnDelete.AccessibleName = "Delete";
			btnDelete.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			btnDelete.BackColor = Color.Transparent;
			btnDelete.BackgroundImage = Properties.Resources.editDelete;
			btnDelete.BackgroundImageLayout = ImageLayout.Zoom;
			btnDelete.FlatAppearance.BorderSize = 0;
			btnDelete.FlatAppearance.MouseDownBackColor = Color.Transparent;
			btnDelete.FlatAppearance.MouseOverBackColor = Color.Transparent;
			btnDelete.FlatStyle = FlatStyle.Flat;
			btnDelete.Location = new Point(287, 609);
			btnDelete.Name = "btnDelete";
			btnDelete.Size = new Size(60, 60);
			btnDelete.TabIndex = 19;
			buttonToolTips.SetToolTip(btnDelete, "Delete");
			btnDelete.UseVisualStyleBackColor = false;
			btnDelete.Click += editButton_Click;
			btnDelete.MouseDown += transportButton_MouseDown;
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
			// lblTimelineLeft
			// 
			lblTimelineLeft.AccessibleName = "Time at left edge";
			lblTimelineLeft.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
			lblTimelineLeft.ForeColor = Color.White;
			lblTimelineLeft.Location = new Point(15, 76);
			lblTimelineLeft.Name = "lblTimelineLeft";
			lblTimelineLeft.Size = new Size(300, 26);
			lblTimelineLeft.TabIndex = 15;
			lblTimelineLeft.Text = "0.00s";
			lblTimelineLeft.TextAlign = ContentAlignment.MiddleLeft;
			// 
			// lblTimelineRight
			// 
			lblTimelineRight.AccessibleName = "Time at right edge";
			lblTimelineRight.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			lblTimelineRight.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
			lblTimelineRight.ForeColor = Color.White;
			lblTimelineRight.Location = new Point(1289, 76);
			lblTimelineRight.Name = "lblTimelineRight";
			lblTimelineRight.Size = new Size(300, 26);
			lblTimelineRight.TabIndex = 16;
			lblTimelineRight.Text = "0.00s";
			lblTimelineRight.TextAlign = ContentAlignment.MiddleRight;
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
			menuFile.DropDownItems.AddRange(new ToolStripItem[] { menuFileOpen, menuFileSave, menuFileExit });
			menuFile.Name = "menuFile";
			menuFile.Size = new Size(37, 20);
			menuFile.Text = "&File";
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
			// menuFileExit
			// 
			menuFileExit.Name = "menuFileExit";
			menuFileExit.Size = new Size(112, 22);
			menuFileExit.Text = "Exit";
			menuFileExit.Click += menuFileExit_Click;
			// 
			// menuEdit
			// 
			menuEdit.DropDownItems.AddRange(new ToolStripItem[] { menuEditCut, menuEditCopy, menuEditPaste, menuEditPasteNewTrack, menuEditDelete, toolStripSeparator3, menuEditFlip, menuEditReverse, menuEditVolume, menuEditEliminateSilence, toolStripSeparator4, normalizeToolStripMenuItem, phaserToolStripMenuItem, chorusToolStripMenuItem });
			menuEdit.Name = "menuEdit";
			menuEdit.Size = new Size(39, 20);
			menuEdit.Text = "E&dit";
			// 
			// menuEditCut
			// 
			menuEditCut.Name = "menuEditCut";
			menuEditCut.ShortcutKeys = Keys.Control | Keys.X;
			menuEditCut.Size = new Size(174, 22);
			menuEditCut.Text = "Cut";
			menuEditCut.Click += menuEditClipboard_Click;
			// 
			// menuEditCopy
			// 
			menuEditCopy.Name = "menuEditCopy";
			menuEditCopy.ShortcutKeys = Keys.Control | Keys.C;
			menuEditCopy.Size = new Size(174, 22);
			menuEditCopy.Text = "Copy";
			menuEditCopy.Click += menuEditClipboard_Click;
			// 
			// menuEditPaste
			// 
			menuEditPaste.Name = "menuEditPaste";
			menuEditPaste.ShortcutKeys = Keys.Control | Keys.V;
			menuEditPaste.Size = new Size(174, 22);
			menuEditPaste.Text = "Paste";
			menuEditPaste.Click += menuEditClipboard_Click;
			// 
			// menuEditPasteNewTrack
			// 
			menuEditPasteNewTrack.Name = "menuEditPasteNewTrack";
			menuEditPasteNewTrack.Size = new Size(174, 22);
			menuEditPasteNewTrack.Text = "Paste to New Track";
			menuEditPasteNewTrack.Click += menuEditClipboard_Click;
			// 
			// menuEditDelete
			// 
			menuEditDelete.Enabled = false;
			menuEditDelete.Name = "menuEditDelete";
			menuEditDelete.ShortcutKeys = Keys.Delete;
			menuEditDelete.Size = new Size(174, 22);
			menuEditDelete.Text = "Delete";
			menuEditDelete.Click += menuEditDelete_Click;
			// 
			// toolStripSeparator3
			// 
			toolStripSeparator3.Name = "toolStripSeparator3";
			toolStripSeparator3.Size = new Size(171, 6);
			// 
			// menuEditFlip
			// 
			menuEditFlip.Name = "menuEditFlip";
			menuEditFlip.Size = new Size(174, 22);
			menuEditFlip.Text = "Flip";
			menuEditFlip.Click += menuEditFlip_Click;
			// 
			// menuEditReverse
			// 
			menuEditReverse.Name = "menuEditReverse";
			menuEditReverse.Size = new Size(174, 22);
			menuEditReverse.Text = "Reverse";
			menuEditReverse.Click += menuEditReverse_Click;
			// 
			// menuEditVolume
			// 
			menuEditVolume.Name = "menuEditVolume";
			menuEditVolume.Size = new Size(174, 22);
			menuEditVolume.Text = "Alter Volume...";
			menuEditVolume.Click += menuEditVolume_Click;
			// 
			// menuEditEliminateSilence
			// 
			menuEditEliminateSilence.Name = "menuEditEliminateSilence";
			menuEditEliminateSilence.Size = new Size(174, 22);
			menuEditEliminateSilence.Text = "Eliminate Silence...";
			menuEditEliminateSilence.Click += menuEditEliminateSilence_Click;
			// 
			// toolStripSeparator4
			// 
			toolStripSeparator4.Name = "toolStripSeparator4";
			toolStripSeparator4.Size = new Size(171, 6);
			// 
			// normalizeToolStripMenuItem
			// 
			normalizeToolStripMenuItem.Name = "normalizeToolStripMenuItem";
			normalizeToolStripMenuItem.Size = new Size(174, 22);
			normalizeToolStripMenuItem.Text = "Normalize";
			normalizeToolStripMenuItem.Click += normalizeToolStripMenuItem_Click;
			// 
			// phaserToolStripMenuItem
			// 
			phaserToolStripMenuItem.Name = "phaserToolStripMenuItem";
			phaserToolStripMenuItem.Size = new Size(174, 22);
			phaserToolStripMenuItem.Text = "Phaser...";
			phaserToolStripMenuItem.Click += phaserToolStripMenuItem_Click;
			// 
			// chorusToolStripMenuItem
			// 
			chorusToolStripMenuItem.Name = "chorusToolStripMenuItem";
			chorusToolStripMenuItem.Size = new Size(174, 22);
			chorusToolStripMenuItem.Text = "Chorus...";
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
			zoomToolStripMenuItem.Size = new Size(181, 22);
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
			followPlayheadToolStripMenuItem.Size = new Size(181, 22);
			followPlayheadToolStripMenuItem.Text = "Follow Playhead";
			followPlayheadToolStripMenuItem.Click += followPlayheadToolStripMenuItem_Click;
			// 
			// stretchTrackHeightToolStripMenuItem
			// 
			stretchTrackHeightToolStripMenuItem.CheckOnClick = true;
			stretchTrackHeightToolStripMenuItem.Name = "stretchTrackHeightToolStripMenuItem";
			stretchTrackHeightToolStripMenuItem.Size = new Size(181, 22);
			stretchTrackHeightToolStripMenuItem.Text = "Stretch Track Height";
			stretchTrackHeightToolStripMenuItem.CheckedChanged += stretchTrackHeightToolStripMenuItem_CheckedChanged;
			// 
			// toolStripSeparator2
			// 
			toolStripSeparator2.Name = "toolStripSeparator2";
			toolStripSeparator2.Size = new Size(178, 6);
			// 
			// appearanceToolStripMenuItem
			// 
			appearanceToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { defaultToolStripMenuItem, redToolStripMenuItem, greenToolStripMenuItem, blueToolStripMenuItem, darkToolStripMenuItem, lightToolStripMenuItem });
			appearanceToolStripMenuItem.Name = "appearanceToolStripMenuItem";
			appearanceToolStripMenuItem.Size = new Size(181, 22);
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
			trkDrawTimescale.Location = new Point(15, 31);
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
			lblPlayheadTime.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
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
			chkFollowPlayback.Location = new Point(15, 677);
			chkFollowPlayback.Name = "chkFollowPlayback";
			chkFollowPlayback.Size = new Size(111, 19);
			chkFollowPlayback.TabIndex = 20;
			chkFollowPlayback.Text = "Follow Playhead";
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
			Controls.Add(btnCut);
			Controls.Add(btnCopy);
			Controls.Add(btnPaste);
			Controls.Add(btnPasteNewTrack);
			Controls.Add(btnDelete);
			Controls.Add(lblTimelineLeft);
			Controls.Add(lblTimelineRight);
			Controls.Add(panTracks);
			Controls.Add(menu);
			Icon = (Icon)resources.GetObject("$this.Icon");
			KeyPreview = true;
			MainMenuStrip = menu;
			MinimumSize = new Size(940, 500);
			Name = "FrmMain";
			Text = "Mediocore";
			FormClosing += FrmMain_FormClosing;
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
    private Label lblTimelineLeft;
    private Label lblTimelineRight;
    private OpenFileDialog ofdLoadFile;
    private MenuStrip menu;
    private ToolStripMenuItem menuEdit;
    private Button btnPlay;
    private ToolTip buttonToolTips;
    private Button btnReset;
    private Button btnEnd;
    private Button btnSave;
    private Button btnLoad;
    private Button btnCut;
    private Button btnCopy;
    private Button btnPaste;
    private Button btnPasteNewTrack;
    private Button btnDelete;
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
    private ToolStripMenuItem menuEditCut;
    private ToolStripMenuItem menuEditCopy;
    private ToolStripMenuItem menuEditPaste;
    private ToolStripMenuItem menuEditPasteNewTrack;
    private ToolStripMenuItem menuEditDelete;
    private Label lblPlayhead;
    private ToolStripMenuItem menuGenRecord;
        private ToolStripMenuItem menuFileExit;
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
