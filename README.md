# Stationeers Composter Fix

Plants

2 CO2 + 2 H20 → 2 biomass + 2 O2

Composter

2 biomass → CH4 + ?

Combustor with the [Stationeers Combustion Fix](https://steamcommunity.com/sharedfiles/filedetails/?id=3724908136)

CH4 + 2 O2 → CO2 + 2 H2O

----

Solving the Plants equation, 1 biomass should be CH2O.

Then the Composter equation becomes `2 CH2O → CH4 + ?`, therefore `?` should be CO2.

## Setting Up the Project

The project requires a reference to `Assembly-CSharp.dll` from your local Stationeers installation. This file is not included in the repository.

1. Copy `Directory.Build.props.example` to `Directory.Build.props` (in the repository root):
   ```
   cp Directory.Build.props.example Directory.Build.props
   ```
2. Open `Directory.Build.props` and set `GameDir` to your Stationeers installation path:
    * **Windows:** `c:\Program Files (x86)\Steam\steamapps\common\Stationeers`
    * **macOS:** `/Users/yaskovdev/Library/Application Support/Steam/steamapps/common/Stationeers`

   `Directory.Build.props` is ignored in Git, so this change stays local to your machine.
3. Run `dotnet clean` and `dotnet build` to build the project.

## Installing the Mod (for Developers)

Before building, make sure there are no conflicting copies of the mod:

1. Unsubscribe from the mod in Steam Workshop (if subscribed).
2. Verify there is no `StationeersComposterFix.dll` in `<GameDir>\BepInEx\plugins\` (where `<GameDir>` is your Stationeers installation path, e.g. `C:\Program Files (x86)\Steam\steamapps\common\Stationeers`).

Then run the build script:

```powershell
.\Build-Plugin.ps1
```

This builds the plugin in Release configuration and deploys it (along with the `About` folder) to `Documents\My Games\Stationeers\mods\StationeersComposterFix\`.
