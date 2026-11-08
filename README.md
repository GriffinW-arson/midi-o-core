# MediocreSoundDesigner
The most mediocre software you can find

The timeline zoom slider ranges from individual samples to the complete song.
Use **View → Zoom → 100% (Fit)** to show all tracks, and the horizontal scrollbar to explore a
zoomed view. **Follow playback** scrolls with the playhead; manually scrolling
turns following off. Track positions remain fixed in time when changing zoom.
Adding or removing tracks stops playback and resets the playhead. Muting updates
the playing mix at the current position; edits made while paused take effect on
resume.

The transport buttons beneath the waveform are **Reset**, **Play/Pause**, and
**End**. Reset and End stop playback and place the playhead at the project
boundaries. Playing from the end restarts at the beginning. Buttons use red
icons with brief white click feedback; the red pause icon indicates active playback.
The current playhead time appears below the transport buttons as hours, minutes,
seconds, and milliseconds.

Right-click a waveform to seek while preserving the playing, paused, or stopped
state. Reaching the project's end stops playback. Scroll the mouse wheel over
the waveform to zoom in or out around the pointer; the slider and View menu stay
in sync.

Left-drag on a waveform to select a region of that track. Flip, Reverse, Volume,
and Eliminate Silence then affect only the highlighted region. With no region,
effects apply to the whole selected track. Click without dragging or press Escape
to clear the region; switching tracks also clears it. Region volume is a gain
relative to its existing audio (100% leaves it unchanged). Removing silence
closes gaps within that track and shifts its later audio earlier.

Build and run the timeline regression checks (requires the .NET SDK and Windows):

```powershell
dotnet build MediocreSoundDesigner/MediocreSoundDesigner.sln
dotnet run --project MediocreSoundDesigner/MediocreSoundDesigner.Tests
```
