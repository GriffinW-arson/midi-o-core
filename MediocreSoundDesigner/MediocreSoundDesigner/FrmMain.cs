using MediocreSoundDesigner.Properties;

namespace MediocreSoundDesigner;

public partial class FrmMain : Form
{
	// fields
	private TrackManager trackManager;
	private const int ScrollSteps = 1_000_000;

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
		FormClosed += (sender, e) => trackManager.Dispose();
		btnPlay.BackgroundImage = Resources.play;
		followPlayheadToolStripMenuItem.Checked = chkFollowPlayback.Checked;
		UpdateSizes();
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

	private void btnFitSong_Click(object sender, EventArgs e)
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
				if (trackManager.SelectedTrack is not null)
				{
					if (MessageBox.Show("You are about to delete a track! Are you sure you want to proceed?", "Delete Track", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation) == DialogResult.Yes)
					{
						panTracks.Controls.Remove(trackManager.SelectedTrack.PanDrawSamples);
						trackManager.SelectedTrack.PanDrawSamples.Dispose();
						trackManager.SelectedTrack.Dispose();
						trackManager.RemTrack(trackManager.SelectedTrack);
						UpdateSizes();
						return true;
					}
				}
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
			btnPlay.BackgroundImage = Resources.pause;
		}
		else
		{
			trackManager.PauseAll();
			btnPlay.BackgroundImage = Resources.play;
		}
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

	// Fix this to save to the same file every time

	private void menuFileSave_Click(object sender, EventArgs e)
	{
		sfdSaveFile.DefaultExt = "wav";
		sfdSaveFile.Filter = "WAV files (*.wav)|*.wav|MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*";
		sfdSaveFile.FileName = "NewAudio";
		if (sfdSaveFile.ShowDialog() == DialogResult.OK)
		{
			string filePath = sfdSaveFile.FileName;
			trackManager.SaveToFile(filePath);
		}
	}

	private void menuEditFlip_Click(object sender, EventArgs e)
	{
		trackManager.SelectedTrack?.Flip();
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
			btnPlay.BackgroundImage = trackManager.State == NAudio.Wave.PlaybackState.Playing ? Resources.pause : Resources.play;
			btnPlay.Visible = true;
			if (chkFollowPlayback.Checked && trackManager.State == NAudio.Wave.PlaybackState.Playing &&
				trackManager.Viewport.Follow(trackManager.PlaybackTime))
				RefreshTimelineView();
			UpdatePlayhead();
		}
		else
		{
			btnPlay.BackgroundImage = null;
			btnPlay.Visible = false;
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

	private void zoomHalf_Click(object sender, EventArgs e) => SetZoomLevel(0.5);
	private void zoomQuarter_Click(object sender, EventArgs e) => SetZoomLevel(0.25);
	private void zoomTenth_Click(object sender, EventArgs e) => SetZoomLevel(0.1);
	private void zoomHundredth_Click(object sender, EventArgs e) => SetZoomLevel(0.01);

	// Helper methods
	private Panel CreateTrackPanel(Track track)
	{
		Panel pan = new WaveformPanel()
		{
			BackColor = Color.DimGray,
			Width = panTracks.Width,
			Left = 0,
			Top = 0,
		};
		pan.Click += (sender, e) =>
		{
			trackManager.SelectTrack(track);
		};
		pan.DoubleClick += (sender, e) =>
		{
			track.ToggleDisabled();
			track.PanDrawSamples.Invalidate();
		};
		pan.Paint += (sender, e) =>
		{
			track.DrawSamples(e.Graphics, trackManager.Viewport);
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
		btnFitSong.Enabled = viewport.Duration > 0;
		zoomToolStripMenuItem.Enabled = viewport.Duration > 0;
		// Allow for rounding the continuous zoom scale to the slider's integer steps.
		double tolerance = 0.5 / (trkDrawTimescale.Maximum - trkDrawTimescale.Minimum) + 1e-9;
		fitToolStripMenuItem.Checked = viewport.IsFit;
		toolStripMenuItem2.Checked = Math.Abs(viewport.ZoomFraction - 0.5) <= tolerance;
		toolStripMenuItem3.Checked = Math.Abs(viewport.ZoomFraction - 0.25) <= tolerance;
		toolStripMenuItem4.Checked = Math.Abs(viewport.ZoomFraction - 0.1) <= tolerance;
		toolStripMenuItem5.Checked = Math.Abs(viewport.ZoomFraction - 0.01) <= tolerance;
		lblDebug.Text = $"{viewport.Start:0.00}s – {viewport.TimeAt(viewport.Width):0.00}s  /  {viewport.Duration:0.00}s";
		trackManager.RedrawAll();
		UpdatePlayhead();
	}

	private void UpdatePlayhead()
	{
		double x = trackManager.Viewport.XAt(trackManager.PlaybackTime);
		lblPlayhead.Visible = trackManager.Tracks.Count > 0 && x >= 0 && x < panTracks.ClientSize.Width;
		if (lblPlayhead.Visible) lblPlayhead.Left = (int)x;
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
}
