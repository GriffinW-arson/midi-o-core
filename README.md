# Mediocore
The most mediocre software you can find

This is general multipurpose sound editing software.

## Mouse Controls (on waveform)

- **Left Mouse**
  - **Click** — select a track
  - **Click & drag** — select a region of a track. This allows you to selectively apply effects or cut/copy/delete
- **Right Mouse**
  - **Click** — seek. The playhead (white vertical line) will be moved to your cursor. Retains playback state.
- **Scroll Wheel**
  - **Up/down** — zoom in and out around your cursor.

## Keyboard Controls

- **Arrow Keys (left/right)** — Adjust the selected track to be earlier or later. Movement speed is dependent on zoom.
- **Delete** — Remove the selected track or region of track.

### Shortcuts

- **Ctrl+X** — Cut (with region selected)
- **Ctrl+C** — Copy (with region selected)
- **Ctrl+V** — Paste (paste at playhead position, or at the start of a selected region. Shifts existing audio right)

## Onscreen Controls

### Clipboard

- **Cut**
- **Copy**
- **Paste**
- **Paste to New Track** — this will take whatever is in the clipboard (whatever has been cut/copied last) and insert it at the beginning of a new track.
- **Delete** — this will delete the selected track if no region is selected, or just the selected region if one is available.

### Playback

- **Reset** — put the playhead at the beginning of the project.
- **Play/Pause** — what do you think this does. Really.
- **End** — put the playhead at the end of the project.
- **Follow Playhead** — Once the playhead reaches the center of the waveform, the waveform will automatically scroll to follow the playhead.

### File

- **Save** — The mix will be saved to a single WAV or MP3 file.
- **Open** — Open a pre-existing track as a file.

## Effects

Generally, effects will apply to an entire selected track unless a specific region is selected.

**Flip**

- The polarity of the sounds in the track/region is inverted. ooh!

**Reverse**

- The track/region is reversed.

**Alter Volume**

- Alters the volume of the region by a specified multiplier. With no region selected, it sets the whole track's volume.

**Eliminate Silence**

- Removes sections of the track/region that are at or below the threshold and last at least as long as the minimum duration.
- In **Beginning** mode, it checks the quiet section from the beginning of the track/region until the sound exceeds its threshold.
- In **End** mode, it does this from the end of the track/region. In **Beginning and End** mode, it does it from both directions.
- In **Anywhere** mode, any sufficiently long and quiet portion of the track/region is removed.
- Later audio shifts earlier to close the gap.

**Normalize**

- Finds the average absolute sample value of the track/region and sets each nonzero sample to that magnitude while preserving polarity.
- Seriously distorts the sound. Very metal.

**Phaser**

- This duplicates the sound of the track/region and passes one copy through all-pass filters that alter the sound's phase, and then recombines it with the original sound.
- The out-of-phase sounds destructively interfere, creating a strange metallic warbling wooshing sound. Groovy!
- **Stages** — The number of all-pass filters. Changes the intensity of the effect.
- **Rate (Hz)** — The rate at which the filter sweeps between the minimum and maximum frequencies.
- **Min frequency (Hz)** — The lower end of the filter sweep.
- **Max frequency (Hz)** — The upper end of the filter sweep.
- **Mix (%)** — The relative mix of the phase-shifted copy and the normal copy. Lower means less of the phase-shifted copy.

**Chorus**

- This duplicates the sound of the track/region and blends it back with the original using a varying delay that slightly detunes the copy.
- This makes one instrument sound like many, or makes your music sound tipsy.
- **Rate (Hz)** — The rate at which the delay shifts.
- **Delay (ms)** — The average delay amount.
- **Depth (ms)** — The amount by which the delay varies.
- **Mix (%)** — The relative mix of the delayed copy and the normal copy. Lower means less of the delayed copy.

## Build and Test

```powershell
dotnet build MediocreSoundDesigner/MediocreSoundDesigner.sln
dotnet run --project MediocreSoundDesigner/MediocreSoundDesigner.Tests
```
