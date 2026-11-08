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
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
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
            menuEditFlip = new ToolStripMenuItem();
            menuEditReverse = new ToolStripMenuItem();
            menuEditVolume = new ToolStripMenuItem();
            menuEditEliminateSilence = new ToolStripMenuItem();
            menuGen = new ToolStripMenuItem();
            menuGenRecord = new ToolStripMenuItem();
            menuGenNoise = new ToolStripMenuItem();
            menuGenGradientNoise = new ToolStripMenuItem();
            viewToolStripMenuItem = new ToolStripMenuItem();
            zoomToolStripMenuItem = new ToolStripMenuItem();
            fitToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripMenuItem();
            toolStripMenuItem3 = new ToolStripMenuItem();
            toolStripMenuItem4 = new ToolStripMenuItem();
            toolStripMenuItem5 = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            followPlayheadToolStripMenuItem = new ToolStripMenuItem();
            appearanceToolStripMenuItem = new ToolStripMenuItem();
            defaultToolStripMenuItem = new ToolStripMenuItem();
            redToolStripMenuItem = new ToolStripMenuItem();
            greenToolStripMenuItem = new ToolStripMenuItem();
            blueToolStripMenuItem = new ToolStripMenuItem();
            darkToolStripMenuItem = new ToolStripMenuItem();
            lightToolStripMenuItem = new ToolStripMenuItem();
            btnPlay = new Button();
            tmrUpdate = new System.Windows.Forms.Timer(components);
            sfdSaveFile = new SaveFileDialog();
            trkDrawTimescale = new TrackBar();
            timelineScroll = new HScrollBar();
            btnFitSong = new Button();
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
            // 
            // lblPlayhead
            // 
            lblPlayhead.BackColor = Color.White;
            lblPlayhead.Location = new Point(39, -2);
            lblPlayhead.Name = "lblPlayhead";
            lblPlayhead.Size = new Size(5, 385);
            lblPlayhead.TabIndex = 9;
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
            menuEdit.DropDownItems.AddRange(new ToolStripItem[] { menuEditFlip, menuEditReverse, menuEditVolume, menuEditEliminateSilence });
            menuEdit.Name = "menuEdit";
            menuEdit.Size = new Size(39, 20);
            menuEdit.Text = "E&dit";
            // 
            // menuEditFlip
            // 
            menuEditFlip.Name = "menuEditFlip";
            menuEditFlip.Size = new Size(172, 22);
            menuEditFlip.Text = "Flip";
            menuEditFlip.Click += menuEditFlip_Click;
            // 
            // menuEditReverse
            // 
            menuEditReverse.Name = "menuEditReverse";
            menuEditReverse.Size = new Size(172, 22);
            menuEditReverse.Text = "Reverse";
            menuEditReverse.Click += menuEditReverse_Click;
            // 
            // menuEditVolume
            // 
            menuEditVolume.Name = "menuEditVolume";
            menuEditVolume.Size = new Size(172, 22);
            menuEditVolume.Text = "Alter Volume...";
            menuEditVolume.Click += menuEditVolume_Click;
            // 
            // menuEditEliminateSilence
            // 
            menuEditEliminateSilence.Name = "menuEditEliminateSilence";
            menuEditEliminateSilence.Size = new Size(172, 22);
            menuEditEliminateSilence.Text = "Eliminate Silence...";
            menuEditEliminateSilence.Click += menuEditEliminateSilence_Click;
            // 
            // menuGen
            // 
            menuGen.DropDownItems.AddRange(new ToolStripItem[] { menuGenRecord, menuGenNoise, menuGenGradientNoise });
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
            viewToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { zoomToolStripMenuItem, toolStripSeparator1, followPlayheadToolStripMenuItem, appearanceToolStripMenuItem });
            viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            viewToolStripMenuItem.Size = new Size(44, 20);
            viewToolStripMenuItem.Text = "View";
            // 
            // zoomToolStripMenuItem
            // 
            zoomToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { fitToolStripMenuItem, toolStripMenuItem2, toolStripMenuItem3, toolStripMenuItem4, toolStripMenuItem5 });
            zoomToolStripMenuItem.Name = "zoomToolStripMenuItem";
            zoomToolStripMenuItem.Size = new Size(180, 22);
            zoomToolStripMenuItem.Text = "Zoom";
            // 
            // fitToolStripMenuItem
            // 
            fitToolStripMenuItem.Name = "fitToolStripMenuItem";
            fitToolStripMenuItem.Size = new Size(126, 22);
            fitToolStripMenuItem.Text = "100% (Fit)";
            fitToolStripMenuItem.Click += btnFitSong_Click;
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
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(177, 6);
            // 
            // followPlayheadToolStripMenuItem
            // 
            followPlayheadToolStripMenuItem.Name = "followPlayheadToolStripMenuItem";
            followPlayheadToolStripMenuItem.Size = new Size(180, 22);
            followPlayheadToolStripMenuItem.Text = "Follow Playhead";
            followPlayheadToolStripMenuItem.Click += followPlayheadToolStripMenuItem_Click;
            // 
            // appearanceToolStripMenuItem
            // 
            appearanceToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { defaultToolStripMenuItem, redToolStripMenuItem, greenToolStripMenuItem, blueToolStripMenuItem, darkToolStripMenuItem, lightToolStripMenuItem });
            appearanceToolStripMenuItem.Name = "appearanceToolStripMenuItem";
            appearanceToolStripMenuItem.Size = new Size(180, 22);
            appearanceToolStripMenuItem.Text = "Appearance";
            appearanceToolStripMenuItem.Click += appearanceToolStripMenuItem_Click;
            // 
            // defaultToolStripMenuItem
            // 
            defaultToolStripMenuItem.Name = "defaultToolStripMenuItem";
            defaultToolStripMenuItem.Size = new Size(180, 22);
            defaultToolStripMenuItem.Text = "Default";
            defaultToolStripMenuItem.Click += defaultToolStripMenuItem_Click;
            // 
            // redToolStripMenuItem
            // 
            redToolStripMenuItem.Name = "redToolStripMenuItem";
            redToolStripMenuItem.Size = new Size(180, 22);
            redToolStripMenuItem.Text = "Dark";
            redToolStripMenuItem.Click += redToolStripMenuItem_Click;
            // 
            // greenToolStripMenuItem
            // 
            greenToolStripMenuItem.Name = "greenToolStripMenuItem";
            greenToolStripMenuItem.Size = new Size(180, 22);
            greenToolStripMenuItem.Text = "Light";
            greenToolStripMenuItem.Click += greenToolStripMenuItem_Click;
            // 
            // blueToolStripMenuItem
            // 
            blueToolStripMenuItem.Name = "blueToolStripMenuItem";
            blueToolStripMenuItem.Size = new Size(180, 22);
            blueToolStripMenuItem.Text = "Red";
            blueToolStripMenuItem.Click += blueToolStripMenuItem_Click;
            // 
            // darkToolStripMenuItem
            // 
            darkToolStripMenuItem.Name = "darkToolStripMenuItem";
            darkToolStripMenuItem.Size = new Size(180, 22);
            darkToolStripMenuItem.Text = "Green";
            darkToolStripMenuItem.Click += darkToolStripMenuItem_Click;
            // 
            // lightToolStripMenuItem
            // 
            lightToolStripMenuItem.Name = "lightToolStripMenuItem";
            lightToolStripMenuItem.Size = new Size(180, 22);
            lightToolStripMenuItem.Text = "Blue";
            lightToolStripMenuItem.Click += lightToolStripMenuItem_Click;
            // 
            // btnPlay
            // 
            btnPlay.Anchor = AnchorStyles.Bottom;
            btnPlay.AutoSize = true;
            btnPlay.BackColor = Color.Transparent;
            btnPlay.BackgroundImage = Properties.Resources.play;
            btnPlay.BackgroundImageLayout = ImageLayout.Stretch;
            btnPlay.FlatAppearance.BorderSize = 0;
            btnPlay.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnPlay.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnPlay.FlatStyle = FlatStyle.Flat;
            btnPlay.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnPlay.Location = new Point(752, 609);
            btnPlay.Name = "btnPlay";
            btnPlay.Size = new Size(100, 100);
            btnPlay.TabIndex = 6;
            btnPlay.UseVisualStyleBackColor = false;
            btnPlay.Click += btnPlay_Click;
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
            // btnFitSong
            // 
            btnFitSong.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnFitSong.AutoSize = true;
            btnFitSong.BackColor = SystemColors.Control;
            btnFitSong.ForeColor = SystemColors.ControlText;
            btnFitSong.Location = new Point(15, 609);
            btnFitSong.Name = "btnFitSong";
            btnFitSong.Size = new Size(75, 27);
            btnFitSong.TabIndex = 11;
            btnFitSong.Text = "Fit song";
            btnFitSong.UseVisualStyleBackColor = true;
            btnFitSong.Click += btnFitSong_Click;
            // 
            // chkFollowPlayback
            // 
            chkFollowPlayback.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chkFollowPlayback.AutoSize = true;
            chkFollowPlayback.ForeColor = Color.White;
            chkFollowPlayback.Location = new Point(105, 613);
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
            Controls.Add(btnFitSong);
            Controls.Add(timelineScroll);
            Controls.Add(trkDrawTimescale);
            Controls.Add(btnPlay);
            Controls.Add(lblDebug);
            Controls.Add(panTracks);
            Controls.Add(menu);
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
        private Button btnFitSong;
        private CheckBox chkFollowPlayback;
    private Label lblDebug;
    private OpenFileDialog ofdLoadFile;
    private MenuStrip menu;
    private ToolStripMenuItem menuEdit;
    private Button btnPlay;
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
		private ToolStripSeparator toolStripSeparator1;
		private ToolStripMenuItem followPlayheadToolStripMenuItem;
        private ToolStripMenuItem appearanceToolStripMenuItem;
        private ToolStripMenuItem defaultToolStripMenuItem;
        private ToolStripMenuItem redToolStripMenuItem;
        private ToolStripMenuItem greenToolStripMenuItem;
        private ToolStripMenuItem blueToolStripMenuItem;
        private ToolStripMenuItem darkToolStripMenuItem;
        private ToolStripMenuItem lightToolStripMenuItem;
    }
}
