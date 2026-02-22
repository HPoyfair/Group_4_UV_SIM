# UVSim (BasicML Simulator) — Group 4

UVSim is a simple virtual machine simulator used to run **BasicML** programs for learning computer architecture concepts.  
This version includes a **Windows Forms GUI** (no command line needed for normal use).

---

## Requirements

### To run the GUI (from Release download)
- Windows 10/11
- Download the Release asset and run the GUI executable (see **Run from GitHub Release** below)

### To build/run from source
- Windows 10/11
- .NET 8 SDK installed
- (Optional) Visual Studio 2022 with “.NET desktop development” workload

---

## Project Structure

- `UVGUI/`  
  Windows Forms GUI application (this is what you run)

- `Group_4_UV_SIM/`  
  Core simulator logic (memory, CPU state, instruction execution)

- `Group_4_UV_SIM.Tests/`  
  Unit tests for instruction behavior and simulator features

---

## Run From GitHub Release (Recommended for Graders)

1. Go to the repository **Releases** page.
2. Download the latest release asset (the GUI build).
3. If it’s a `.zip`: unzip it.
4. Run `UVGUI.exe`.

> Note: Windows may show a SmartScreen warning for unsigned student applications.
> If that happens: More info → Run anyway.

---

## Run From Source (GUI)

From the repo root:

```powershell
dotnet restore
dotnet run --project .\UVGUI\UVGUI.csproj



