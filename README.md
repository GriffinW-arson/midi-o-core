# MediocreSoundDesigner
The most mediocre software you can find

The timeline zoom slider ranges from individual samples to the complete song.
Use **Fit song** to show all tracks, and the horizontal scrollbar to explore a
zoomed view. **Follow playback** scrolls with the playhead; manually scrolling
turns following off. Track positions remain fixed in time when changing zoom.
Adding or removing tracks stops playback and resets the playhead. Muting updates
the playing mix at the current position; edits made while paused take effect on
resume.

Build and run the timeline regression checks (requires the .NET SDK and Windows):

```powershell
dotnet build MediocreSoundDesigner/MediocreSoundDesigner.sln
dotnet run --project MediocreSoundDesigner/MediocreSoundDesigner.Tests
```
