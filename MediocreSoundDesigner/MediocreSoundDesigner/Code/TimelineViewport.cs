namespace MediocreSoundDesigner;

// viewport handling
public sealed class TimelineViewport {

    // the length of the project (longest track + offset)
    public double Duration { get; private set; }
    // width of the drawn waveform in pix
    public int Width { get; private set; } = 1;
    // time at the left edge of the wf
    public double Start { get; private set; }
    // zoom scale
    public double SecondsPerPixel { get; private set; } = 1.0 / 44100;
    // the duration of what's on the wf
    public double VisibleDuration => Width * SecondsPerPixel;
    // to prevent overscoll, 
    public double MaximumStart => Math.Max(0, Duration - VisibleDuration);
    // fit the whole project in the waveform
    public bool IsFit { get; private set; } = true;

    // defines zoom for fit- max zoom
    private double FitScale => Math.Max(MinimumScale, Duration / Width);
    // defines minumum scale
    private const double MinimumScale = 1.0 / 192000;


    // configure what the zoom scale and start time should be given duration and width
    public void Configure(double duration, int width) {
        Duration = Math.Max(0, duration);
        Width = Math.Max(1, width);
        if (IsFit)
        {
            Fit();
        }
        else
        {
            // secs per pix should be >minscale and <fitscale
            SecondsPerPixel = Math.Clamp(SecondsPerPixel, MinimumScale, FitScale);
            SetStart(Start);
        }
    }

    // scale to fit the longest track
    public void Fit() {
        IsFit = true;
        SecondsPerPixel = FitScale;
        Start = 0;
    }

    // takes a zoom fraction between 0 and 1
    public void SetZoom(double fraction, double? anchorX = null) {
        double anchor = Math.Clamp(anchorX ?? Width / 2.0, 0, Width);
        double anchorTime = TimeAt(anchor);
        fraction = Math.Clamp(fraction, 0, 1);
        // at max zoomout, just turn on isfit
        IsFit = fraction >= 1;

        // secs per pix = min zoom * (max zoom/minzoom)^fraction
        // dynamic maximum and logaritmic zoom!! zoom gets slower the more you zoom in
        SecondsPerPixel = MinimumScale * Math.Pow(FitScale / MinimumScale, fraction);
        SetStart(anchorTime - anchor * SecondsPerPixel);
    }

    // for the slider
    public double ZoomFraction => FitScale <= MinimumScale ? 1 :
        Math.Clamp(Math.Log(SecondsPerPixel / MinimumScale) / Math.Log(FitScale / MinimumScale), 0, 1);

    // set the start position
    public void SetStart(double seconds)
    {
        Start = Math.Clamp(seconds, 0, MaximumStart);
    }

    // convert timeline time to xpos
    public double XAt(double seconds)
    {
        return (seconds - Start) / SecondsPerPixel;
    }

    // get the timeline time at a specific xpos
    public double TimeAt(double x)
    {
        return Start + x * SecondsPerPixel;
    }

    // move the viewport to follow the playhead
    // returns True on successful movement
    public bool Follow(double seconds) {
        if (seconds >= Start && seconds < TimeAt(Width * 0.5)) return false;
        double previous = Start;
        SetStart(seconds - VisibleDuration * 0.5);
        return Start != previous;
    }
}
