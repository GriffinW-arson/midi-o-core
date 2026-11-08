using MediocreSoundDesigner.Properties;

namespace MediocreSoundDesigner;

public partial class FrmMain : Form, IMessageFilter
{
	// Color Profile Definitions
	public record ColorProfile(Color MainPanelColor, Color DefaultWaveformColor, List<Color> AccentWaveformPalette, Color SelectedPanelColor);

	public static readonly Dictionary<string, ColorProfile> ColorProfiles = new()
	{
		{
			"Default", new ColorProfile(
				Color.DimGray,
				Color.Cyan,
				new List<Color> { Color.Cyan, Color.DodgerBlue, Color.LimeGreen, Color.Crimson, Color.MediumOrchid, Color.DeepSkyBlue, Color.Orange, Color.Teal, Color.Gold, Color.HotPink, Color.Coral },
				Color.Gray
			)
		},
		{
			"Red", new ColorProfile(
				Color.FromArgb(45, 10, 10),
				Color.Crimson,
				new List<Color> { Color.Crimson, Color.Coral, Color.OrangeRed, Color.HotPink, Color.IndianRed, Color.LightCoral, Color.DarkOrange },
				Color.FromArgb(75, 25, 25)
			)
		},
		{
			"Green", new ColorProfile(
				Color.FromArgb(10, 35, 10),
				Color.LimeGreen,
				new List<Color> { Color.LimeGreen, Color.Teal, Color.SpringGreen, Color.MediumSpringGreen, Color.Chartreuse, Color.LightSeaGreen, Color.DarkSeaGreen },
				Color.FromArgb(25, 65, 25)
			)
		},
		{
			"Blue", new ColorProfile(
				Color.FromArgb(10, 15, 45),
				Color.DodgerBlue,
				new List<Color> { Color.DodgerBlue, Color.Cyan, Color.DeepSkyBlue, Color.RoyalBlue, Color.SteelBlue, Color.SkyBlue, Color.MediumSlateBlue },
				Color.FromArgb(25, 35, 80)
			)
		},
		{
			"Black", new ColorProfile(
				Color.FromArgb(20, 20, 20),
				Color.White,
				new List<Color> { Color.White, Color.LightGray, Color.Silver, Color.Aqua, Color.SpringGreen, Color.Magenta, Color.Yellow },
				Color.FromArgb(48, 48, 48)
			)
		},
		{
			"White", new ColorProfile(
				Color.FromArgb(240, 240, 240),
				Color.MidnightBlue,
				new List<Color> { Color.MidnightBlue, Color.DarkRed, Color.DarkGreen, Color.Purple, Color.DarkSlateBlue, Color.SaddleBrown, Color.DarkMagenta },
				Color.FromArgb(210, 225, 245)
			)
		}
	};

	public static string CurrentProfileKey { get; private set; } = "Default";
	public static ColorProfile ActiveProfile => ColorProfiles[CurrentProfileKey];

	// fields
	private TrackManager trackManager;
	private const int ScrollSteps = 1_000_000;
	private Button pressedTransportButton;
	private Track regionDragTrack;
	private double regionAnchorTime;
	private int regionAnchorX;
	private bool regionDragMoved;
	private static readonly Image ResetIcon = Resources.transportReset;
	private static readonly Image ResetPressedIcon = Resources.transportReset2;
	private static readonly Image PlayIcon = Resources.transportPlay;
	private static readonly Image PlayPressedIcon = Resources.transportPlay2;
	private static readonly Image PauseIcon = Resources.transportPause;
	private static readonly Image PausePressedIcon = Resources.transportPause2;
	private static readonly Image EndIcon = Resources.transportEnd;
	private static readonly Image EndPressedIcon = Resources.transportEnd2;
	private static readonly Image SaveIcon = Resources.transportSave;
	private static readonly Image SavePressedIcon = Resources.transportSave2;
	private static readonly Image LoadIcon = Resources.transportLoad;
	private static readonly Image LoadPressedIcon = Resources.transportLoad2;

	// ctor
	public FrmMain()
	{
		InitializeComponent();
		Setup();
	}

	// setup
	private void Setup()
	{
		DoubleBuffered = true;
		trackManager = new(panTracks);
		trackManager.TimelineChanged += (sender, e) => RefreshTimelineView();
		Application.AddMessageFilter(this);
		followPlayheadToolStripMenuItem.Checked = chkFollowPlayback.Checked;
		//BuildThemeMenu();
		UpdateSizes();
	}

	//private void BuildThemeMenu()
	//{
	//    ToolStripMenuItem menuTheme = new("Theme");
	//    foreach (var key in ColorProfiles.Keys)
	//    {
	//        ToolStripMenuItem item = new(key)
	//        {
	//            Checked = key == CurrentProfileKey
	//        };
	//        item.Click += (sender, e) => SetColorProfile(key);
	//        menuTheme.DropDownItems.Add(item);
	//    }
	//    menu.Items.Add(menuTheme);
	//}

	public void SetColorProfile(string profileKey)
	{
		if (!ColorProfiles.ContainsKey(profileKey)) return;

		CurrentProfileKey = profileKey;
		ColorProfile profile = ActiveProfile;

		// 1. Update Menu Checkmarks
		foreach (ToolStripItem item in menu.Items)
		{
			if (item is ToolStripMenuItem topMenu && topMenu.Text == "Theme")
			{
				foreach (ToolStripMenuItem subItem in topMenu.DropDownItems)
				{
					subItem.Checked = subItem.Text == profileKey;
				}
			}
		}

		// 2. Update Container Background
		panTracks.BackColor = profile.MainPanelColor;

		// 3. Update Loaded Tracks & Redraw
		if (trackManager != null)
		{
			foreach (Track track in trackManager.Tracks)
			{
				track.ApplyTheme(profile);
				if (track.PanDrawSamples != null)
				{
					track.PanDrawSamples.BackColor = track == trackManager.SelectedTrack
						? profile.SelectedPanelColor : profile.MainPanelColor;
					track.PanDrawSamples.Invalidate();
				}
			}
		}

		panTracks.Invalidate();
	}

	private void timelineScroll_Scroll(object sender, ScrollEventArgs e)
	{
		if (trackManager is null) return;
		// Manual exploration stays where the user puts it during playback.
		chkFollowPlayback.Checked = false;
		int range = timelineScroll.Maximum - timelineScroll.LargeChange + 1;
		trackManager.Viewport.SetStart(trackManager.Viewport.MaximumStart * e.NewValue / Math.Max(1, range));
		RefreshTimelineView();
	}

	private void fitZoom_Click(object sender, EventArgs e)
	{
		if (trackManager is null) return;
		trackManager.Viewport.Fit();
		RefreshTimelineView();
	}

	private void panTracks_SizeChanged(object sender, EventArgs e)
	{
		UpdateSizes();
	}

	// GUI controls
	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		if (trackManager.SelectedTrack is not null)
		{
			if (keyData == Keys.Escape)
			{
				EndRegionDrag();
				trackManager.SelectedTrack.ClearSelection();
				return true;
			}
			if (keyData == Keys.Left)
			{
				trackManager.SelectedTrack.ShiftSampleOver(-5 * trackManager.Viewport.SecondsPerPixel);
				trackManager.SelectedTrack.PanDrawSamples.Invalidate();
				return true;
			}
			else if (keyData == Keys.Right)
			{
				trackManager.SelectedTrack.ShiftSampleOver(5 * trackManager.Viewport.SecondsPerPixel);
				trackManager.SelectedTrack.PanDrawSamples.Invalidate();
				return true;
			}
			else if (keyData == Keys.Delete)
			{
				menuEditDelete_Click(this, EventArgs.Empty);
				return true;
			}
		}
		return base.ProcessCmdKey(ref msg, keyData);
	}

	private void FrmMain_SizeChanged(object sender, EventArgs e)
	{
		UpdateSizes();
	}

	private void btnPlay_Click(object sender, EventArgs e)
	{
		if (trackManager.State is NAudio.Wave.PlaybackState.Stopped or NAudio.Wave.PlaybackState.Paused)
		{
			trackManager.PlayAll();
		}
		else
		{
			trackManager.PauseAll();
		}
		FlashTransportButton(btnPlay);
		UpdatePlayhead();
	}

	private void btnReset_Click(object sender, EventArgs e)
	{
		trackManager.ResetToStart();
		FlashTransportButton(btnReset);
	}

	private void btnEnd_Click(object sender, EventArgs e)
	{
		trackManager.GoToEnd();
		FlashTransportButton(btnEnd);
	}

	private void transportButton_MouseDown(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left) FlashTransportButton((Button)sender);
	}

	private void FlashTransportButton(Button button)
	{
		pressedTransportButton = button;
		transportFeedback.Stop();
		transportFeedback.Start();
		UpdateTransportButtons();
	}

	private void transportFeedback_Tick(object sender, EventArgs e)
	{
		transportFeedback.Stop();
		pressedTransportButton = null;
		UpdateTransportButtons();
	}

	private void UpdateTransportButtons()
	{
		bool hasAudio = trackManager.Tracks.Count > 0 && trackManager.Viewport.Duration > 0;
		btnReset.Enabled = btnPlay.Enabled = btnEnd.Enabled = hasAudio;
		btnReset.BackgroundImage = pressedTransportButton == btnReset ? ResetPressedIcon : ResetIcon;
		btnEnd.BackgroundImage = pressedTransportButton == btnEnd ? EndPressedIcon : EndIcon;
		btnSave.BackgroundImage = pressedTransportButton == btnSave ? SavePressedIcon : SaveIcon;
		btnLoad.BackgroundImage = pressedTransportButton == btnLoad ? LoadPressedIcon : LoadIcon;
		bool playing = trackManager.State == NAudio.Wave.PlaybackState.Playing;
		btnPlay.BackgroundImage = playing ? (pressedTransportButton == btnPlay ? PausePressedIcon : PauseIcon) :
			pressedTransportButton == btnPlay ? PlayPressedIcon : PlayIcon;
		btnPlay.AccessibleName = playing ? "Pause" : "Play";
	}

	private void menuFileOpen_Click(object sender, EventArgs e)
	{
		ofdLoadFile.DefaultExt = "wav";
		if (ofdLoadFile.ShowDialog() == DialogResult.OK)
		{
			Track newTrack = new(ofdLoadFile.FileName);
			CreateTrackPanel(newTrack);
			trackManager.AddTrack(newTrack);
			UpdateSizes();
			newTrack.PanDrawSamples.Invalidate();
		}
	}

	private void btnSave_Click(object sender, EventArgs e)
	{
		FlashTransportButton(btnSave);
		menuFileSave_Click(sender, e);
	}

	private void btnLoad_Click(object sender, EventArgs e)
	{
		FlashTransportButton(btnLoad);
		menuFileOpen_Click(sender, e);
	}

	// Fix this to save to the same file every time

	private void menuFileSave_Click(object sender, EventArgs e)
	{
		sfdSaveFile.DefaultExt = "wav";
		sfdSaveFile.Filter = "WAV files (*.wav)|*.wav|MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*";
		sfdSaveFile.FileName = "NewAudio";
		if (sfdSaveFile.ShowDialog() == DialogResult.OK)
		{
			try
			{
				string filePath = sfdSaveFile.FileName;
				trackManager.SaveToFile(filePath);
				MessageBox.Show($"Saved to {filePath}", "Save Succeed", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"There was an error during the save!\n{ex}", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
			
		}


	}

	private void menuEditFlip_Click(object sender, EventArgs e)
	{
		trackManager.SelectedTrack?.Flip();
	}

	private void menuEditDelete_Click(object sender, EventArgs e)
	{
		Track track = trackManager?.SelectedTrack;
		if (track is null) return;
		EndRegionDrag();
		bool deleteRegion = track.HasSelection;
		string message = deleteRegion
			? "You are about to delete the selected region of this track! Are you sure you want to proceed?"
			: "You are about to delete a track! Are you sure you want to proceed?";
		if (MessageBox.Show(this, message, deleteRegion ? "Delete Region" : "Delete Track",
			MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation) != DialogResult.Yes) return;

		if (deleteRegion) track.DeleteSelection();
		else
		{
			panTracks.Controls.Remove(track.PanDrawSamples);
			trackManager.RemTrack(track);
			track.PanDrawSamples.Dispose();
			track.Dispose();
			UpdateSizes();
		}
	}
	private void menuEditReverse_Click(object sender, EventArgs e)
	{
		trackManager.SelectedTrack?.Reverse();
	}
	private void menuEditVolume_Click(object sender, EventArgs e)
	{
		if (trackManager.SelectedTrack is not null)
		{
			FrmEditVolume frm = new();
			frm.SetSound(trackManager.SelectedTrack);
			frm.ShowDialog();
		}
	}
	private void menuEditEliminateSilence_Click(object sender, EventArgs e)
	{
		if (trackManager.SelectedTrack is not null)
		{
			FrmEditEliminateSilence frm = new();
			frm.SetSound(trackManager.SelectedTrack);
			frm.ShowDialog();
		}
	}

	private void menuGenRecord_Click(object sender, EventArgs e)
	{
		Track newTrack = new(new float[0]);
		CreateTrackPanel(newTrack);
		trackManager.AddTrack(newTrack);
		UpdateSizes();
		newTrack.PanDrawSamples.Invalidate();

		FrmGenRecord frm = new();
		frm.SetSound(trackManager.SelectedTrack);
		if (frm.ShowDialog() == DialogResult.Cancel)
		{
			trackManager.RemTrack(newTrack);
			panTracks.Controls.Remove(newTrack.PanDrawSamples);
			newTrack.PanDrawSamples.Dispose();
			newTrack.Dispose();
			UpdateSizes();
		}
	}
	private void menuGenNoise_Click(object sender, EventArgs e)
	{
		const int NUM_SAMPLES = 100_000;
		float[] samples = new float[NUM_SAMPLES];
		Random rand = new();
		for (int i = 0; i < NUM_SAMPLES; i++)
		{
			samples[i] = rand.NextSingle() * 2.0f - 1.0f;
		}
		Track newTrack = new(samples);
		CreateTrackPanel(newTrack);
		trackManager.AddTrack(newTrack);
		UpdateSizes();
		newTrack.PanDrawSamples.Invalidate();
	}
	private void menuGenGradientNoise_Click(object sender, EventArgs e)
	{
		const int NUM_SAMPLES = 250_000;
		float[] samples = new float[NUM_SAMPLES];
		Random rand = new();
		float cur = 0;
		float inc = 0.005f;
		int dir = +1;
		for (int i = 0; i < NUM_SAMPLES; i++)
		{
			cur += (inc * dir);
			if (rand.NextSingle() < 0.5f)
			{
				dir *= -1;
			}
			if (cur > 0.9f)
			{
				dir = -1;
			}
			if (cur < -0.9f)
			{
				dir = +1;
			}
			samples[i] = cur;
		}
		Track newTrack = new(samples);
		CreateTrackPanel(newTrack);
		trackManager.AddTrack(newTrack);
		UpdateSizes();
		newTrack.PanDrawSamples.Invalidate();
	}

	private void tmrUpdatePlaybackState_Tick(object sender, EventArgs e)
	{
		if (trackManager is null) return;
		if (trackManager.Tracks.Count > 0)
		{
			UpdateTransportButtons();
			if (regionDragTrack is null && chkFollowPlayback.Checked && trackManager.State == NAudio.Wave.PlaybackState.Playing &&
				trackManager.Viewport.Follow(trackManager.PlaybackTime))
				RefreshTimelineView();
			UpdatePlayhead();
		}
		else
		{
			UpdateTransportButtons();
			lblPlayhead.Visible = false;
		}
	}

	private void trkDrawTimescale_Scroll(object sender, EventArgs e)
	{
		if (trackManager is null) return;
		double fraction = (double)(trkDrawTimescale.Value - trkDrawTimescale.Minimum) /
			(trkDrawTimescale.Maximum - trkDrawTimescale.Minimum);
		SetZoomLevel(fraction);
	}

	private void SetZoomLevel(double fraction)
	{
		if (trackManager is null) return;
		trackManager.Viewport.SetZoom(fraction);
		RefreshTimelineView();
	}

	private void waveform_MouseDown(object sender, MouseEventArgs e)
	{
		if (trackManager is null) return;
		var source = (Control)sender;
		// Waveform rows and the playhead label are direct children of panTracks.
		int x = e.X + (source == panTracks ? 0 : source.Left);
		if (e.Button == MouseButtons.Right)
		{
			trackManager.Seek(trackManager.Viewport.TimeAt(x));
			return;
		}
		if (e.Button != MouseButtons.Left || e.Clicks > 1) return;
		int y = e.Y + (source == panTracks ? 0 : source.Top);
		var track = source.Tag as Track ?? trackManager.Tracks.FirstOrDefault(item => item.PanDrawSamples.Bounds.Contains(x, y));
		if (track is null) return;
		EndRegionDrag();
		trackManager.SelectTrack(track);
		track.ClearSelection();
		regionDragTrack = track;
		regionAnchorX = x;
		regionAnchorTime = trackManager.Viewport.TimeAt(x);
		regionDragMoved = false;
		track.PanDrawSamples.Capture = true;
	}

	private void waveform_MouseMove(object sender, MouseEventArgs e)
	{
		if (regionDragTrack?.PanDrawSamples != sender) return;
		UpdateRegionDrag(e.X);
	}

	private void waveform_MouseUp(object sender, MouseEventArgs e)
	{
		if (e.Button != MouseButtons.Left || regionDragTrack?.PanDrawSamples != sender) return;
		UpdateRegionDrag(e.X);
		EndRegionDrag();
	}

	private void UpdateRegionDrag(int x)
	{
		if (regionDragTrack is null) return;
		regionDragMoved |= Math.Abs(x - regionAnchorX) >= 3;
		if (!regionDragMoved) return;
		x = Math.Clamp(x, 0, regionDragTrack.PanDrawSamples.ClientSize.Width);
		regionDragTrack.SetSelection(regionAnchorTime, trackManager.Viewport.TimeAt(x));
	}

	private void EndRegionDrag()
	{
		Panel panel = regionDragTrack?.PanDrawSamples;
		regionDragTrack = null;
		if (panel != null && !panel.IsDisposed) panel.Capture = false;
	}

	// Receive wheel messages even when a toolbar control has keyboard focus.
	public bool PreFilterMessage(ref Message message)
	{
		const int MouseWheelMessage = 0x020A;
		if (message.Msg != MouseWheelMessage || trackManager is null || trackManager.Viewport.Duration <= 0)
			return false;
		if (Control.FromChildHandle(message.HWnd)?.FindForm() != this) return false;
		long coordinates = message.LParam.ToInt64();
		Point screenPoint = new(unchecked((short)coordinates), unchecked((short)(coordinates >> 16)));
		Point point = panTracks.PointToClient(screenPoint);
		if (!panTracks.ClientRectangle.Contains(point)) return false;
		int delta = unchecked((short)(message.WParam.ToInt64() >> 16));
		var viewport = trackManager.Viewport;
		viewport.SetZoom(viewport.ZoomFraction - delta / 120.0 * 0.04, point.X);
		RefreshTimelineView();
		return true;
	}

	private void zoomHalf_Click(object sender, EventArgs e) => SetZoomLevel(0.5);
	private void zoomQuarter_Click(object sender, EventArgs e) => SetZoomLevel(0.25);
	private void zoomTenth_Click(object sender, EventArgs e) => SetZoomLevel(0.1);
	private void zoomHundredth_Click(object sender, EventArgs e) => SetZoomLevel(0.01);

	// Helper methods
	private Panel CreateTrackPanel(Track track)
	{
		Panel pan = new WaveformPanel()
		{
			BackColor = ActiveProfile.MainPanelColor,
			Width = panTracks.Width,
			Left = 0,
			Top = 0,
			Tag = track,
		};
		pan.MouseClick += (sender, e) =>
		{
			if (e.Button == MouseButtons.Left) trackManager.SelectTrack(track);
		};
		pan.MouseDoubleClick += (sender, e) =>
		{
			if (e.Button != MouseButtons.Left) return;
			track.ToggleDisabled();
			track.PanDrawSamples.Invalidate();
		};
		pan.MouseDown += waveform_MouseDown;
		pan.MouseMove += waveform_MouseMove;
		pan.MouseUp += waveform_MouseUp;
		pan.MouseCaptureChanged += (sender, e) =>
		{
			if (regionDragTrack == track && !pan.Capture) EndRegionDrag();
		};
		pan.Paint += (sender, e) =>
		{
			track.DrawSamples(e.Graphics, trackManager.Viewport);
			if (trackManager.SelectedTrack == track) track.DrawSelection(e.Graphics, trackManager.Viewport);
		};
		panTracks.Controls.Add(pan);
		track.LinkPanels(pan, panTracks);
		trackManager.SelectTrack(track);

		return pan;
	}

	private void UpdateSizes()
	{
		if (trackManager is null) return;
		int top = 0;
		foreach (Track track in trackManager.Tracks)
		{
			track.PanDrawSamples.Left = 0;
			track.PanDrawSamples.Width = panTracks.ClientSize.Width;
			track.PanDrawSamples.Height = Math.Max(1, panTracks.ClientSize.Height / trackManager.Tracks.Count - 1);
			track.PanDrawSamples.Top = top;
			track.PanDrawSamples.Invalidate();
			top += track.PanDrawSamples.Height;
		}
		lblPlayhead.Top = 0;
		lblPlayhead.Height = panTracks.ClientSize.Height;
		lblPlayhead.BringToFront();

		trackManager.RefreshTimeline();
	}

	private void RefreshTimelineView()
	{
		var viewport = trackManager.Viewport;
		trkDrawTimescale.Value = trkDrawTimescale.Minimum + (int)Math.Round(viewport.ZoomFraction *
			(trkDrawTimescale.Maximum - trkDrawTimescale.Minimum));
		int page = viewport.Duration <= 0 ? ScrollSteps + 1 :
			Math.Clamp((int)Math.Round((ScrollSteps + 1) * Math.Min(1, viewport.VisibleDuration / viewport.Duration)), 1, ScrollSteps + 1);
		timelineScroll.Minimum = 0;
		timelineScroll.Maximum = ScrollSteps;
		timelineScroll.LargeChange = page;
		timelineScroll.SmallChange = Math.Max(1, page / 10);
		int range = ScrollSteps - page + 1;
		timelineScroll.Value = viewport.MaximumStart <= 0 ? 0 :
			Math.Clamp((int)Math.Round(range * viewport.Start / viewport.MaximumStart), 0, range);
		timelineScroll.Enabled = viewport.MaximumStart > 0;
		zoomToolStripMenuItem.Enabled = viewport.Duration > 0;
		menuEditDelete.Enabled = trackManager.SelectedTrack != null;
		// Allow for rounding the continuous zoom scale to the slider's integer steps.
		double tolerance = 0.5 / (trkDrawTimescale.Maximum - trkDrawTimescale.Minimum) + 1e-9;
		fitToolStripMenuItem.Checked = viewport.IsFit;
		toolStripMenuItem2.Checked = Math.Abs(viewport.ZoomFraction - 0.5) <= tolerance;
		toolStripMenuItem3.Checked = Math.Abs(viewport.ZoomFraction - 0.25) <= tolerance;
		toolStripMenuItem4.Checked = Math.Abs(viewport.ZoomFraction - 0.1) <= tolerance;
		toolStripMenuItem5.Checked = Math.Abs(viewport.ZoomFraction - 0.01) <= tolerance;
		lblDebug.Text = $"{viewport.Start:0.00}s – {viewport.TimeAt(viewport.Width):0.00}s  /  {viewport.Duration:0.00}s";
		trackManager.RedrawAll();
		UpdateTransportButtons();
		UpdatePlayhead();
	}

	private void UpdatePlayhead()
	{
		lblPlayheadTime.Text = TimeSpan.FromSeconds(trackManager.PlaybackTime).ToString(@"hh\:mm\:ss\.fff");
		double x = trackManager.Viewport.XAt(trackManager.PlaybackTime);
		if (trackManager.Viewport.Duration > 0 && trackManager.PlaybackTime >= trackManager.Viewport.Duration)
			x = Math.Min(x, panTracks.ClientSize.Width - lblPlayhead.Width);
		bool inView = trackManager.Tracks.Count > 0 && x >= 0 && x < panTracks.ClientSize.Width;
		lblPlayhead.Visible = inView;
		if (inView) lblPlayhead.Left = (int)x;
	}

	private sealed class WaveformPanel : Panel
	{
		public WaveformPanel()
		{
			DoubleBuffered = true;
			ResizeRedraw = true;
		}
	}

	private void panTracks_Paint(object sender, PaintEventArgs e)
	{

	}

	private void followPlayheadToolStripMenuItem_Click(object sender, EventArgs e)
	{
		chkFollowPlayback.Checked = !chkFollowPlayback.Checked;
	}

	private void chkFollowPlayback_CheckedChanged(object sender, EventArgs e)
	{
		followPlayheadToolStripMenuItem.Checked = chkFollowPlayback.Checked;
	}

	private void appearanceToolStripMenuItem_Click(object sender, EventArgs e)
	{

	}

	private void defaultToolStripMenuItem_Click(object sender, EventArgs e)
	{
		SetColorProfile("Default");
	}

	private void redToolStripMenuItem_Click(object sender, EventArgs e)
	{
		SetColorProfile("Black");
	}

	private void greenToolStripMenuItem_Click(object sender, EventArgs e)
	{
		SetColorProfile("White");
	}

	private void blueToolStripMenuItem_Click(object sender, EventArgs e)
	{
		SetColorProfile("Red");
	}

	private void darkToolStripMenuItem_Click(object sender, EventArgs e)
	{
		SetColorProfile("Green");
	}

	private void lightToolStripMenuItem_Click(object sender, EventArgs e)
	{
		SetColorProfile("Blue");
	}

	private void normalizeToolStripMenuItem_Click(object sender, EventArgs e)
	{
		trackManager.SelectedTrack?.Normalize();
	}

	private void phaserToolStripMenuItem_Click(object sender, EventArgs e)
	{
		trackManager.SelectedTrack?.Phaser();
	}

	private void chorusToolStripMenuItem_Click(object sender, EventArgs e)
	{
		trackManager.SelectedTrack?.Chorus();
	}
}
