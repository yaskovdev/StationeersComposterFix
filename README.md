# Stationeers Composter Fix

Fixes the Advanced Composter to produce chemically correct outputs.

## Chemistry

### Reactions

| Component | Reaction                             |
|-----------|--------------------------------------|
| Plants    | CO₂ + H₂O → CH₂O + O₂                |
| Character | CH₂O + O₂ → CO₂ + H₂O                |
| Composter | 2 CH₂O + H₂O(l) → CH₄ + CO₂ + H₂O(g) |
| Combustor | CH₄ + 2 O₂ → CO₂ + 2 H₂O             |

Biomass is treated as CH₂O (formaldehyde equivalent), derived from the Plants equation. The composter consumes liquid water as a medium; composting heat evaporates it as steam.

The Combustor reaction requires the [Stationeers Combustion Fix](https://steamcommunity.com/sharedfiles/filedetails/?id=3724908136).

### Closed Loop (self-sustainable ratios)

| Component | Multiplier | Reaction                             |
|-----------|:----------:|--------------------------------------|
| Plants    |     4×     | CO₂ + H₂O → CH₂O + O₂                |
| Character |     2×     | CH₂O + O₂ → CO₂ + H₂O                |
| Composter |     1×     | 2 CH₂O + H₂O(l) → CH₄ + CO₂ + H₂O(g) |
| Combustor |     1×     | CH₄ + 2 O₂ → CO₂ + 2 H₂O             |

All species (CO₂, H₂O, O₂, CH₂O, CH₄) net to zero. Steam condenses back into the water supply.

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
