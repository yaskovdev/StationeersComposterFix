# Copilot Instructions

## Project Overview

A BepInEx plugin (mod) for the game [Stationeers](https://store.steampowered.com/app/544550/Stationeers/) that fixes the Advanced Composter to also expel CO₂ and Steam. The chemistry reasoning: biomass is effectively CH₂O, so the correct composter reaction is `2 CH₂O + H₂O(l) → CH₄ + CO₂ + H₂O(g)`. Water is consumed as a medium for microbial activity and expelled as steam (phase change, not chemically consumed).

## Build & Test

**Prerequisites:** Copy `Directory.Build.props.example` to `Directory.Build.props` and set `GameDir` to your local Stationeers installation path. This file is git-ignored. The project references `Assembly-CSharp.dll` (and for tests, `UnityEngine.dll` and `UnityEngine.CoreModule.dll`) from the game's managed assemblies.

```powershell
dotnet build                         # Build entire solution
dotnet test                          # Run all tests
dotnet test --filter "FullyQualifiedName~Should2"  # Run a single test
.\Build-Plugin.ps1                   # Build Release + deploy to local mods folder
```

The plugin targets `netstandard2.0`; the test project targets `net8.0` (MSTest + Shouldly).

## Architecture

The mod patches the Advanced Composter prefab via a Harmony **prefix** on `Prefab.Register`. The flow is:

1. **`Plugin.cs`** — BepInEx entry point. Calls `harmony.PatchAll()` to apply all Harmony patches in the assembly.
2. **`PrefabPatch.cs`** — The Harmony `[HarmonyPatch]`. The `Prefix` method intercepts `Prefab.Register` calls, checks if the prefab is an `AdvancedComposter`, and adds `GasType.CarbonDioxide` and `GasType.Steam` to its `ExpelledGas` list if the current values exactly match the expected unpatched state.

## Key Conventions

- **Exact match detection:** The patch uses `IsExpectedUnpatched()` to verify expelled gas types, quantities, and `MolesDrainedPerProcessedItem` before applying changes. If the game developers change any of these values, the patch silently skips rather than applying corrections to an unrecognized configuration.
- **`using` directives go inside the namespace** (per `.editorconfig`).
- **Companion mod:** This mod is designed to work alongside [StationeersCombustionFix](https://steamcommunity.com/sharedfiles/filedetails/?id=3724908136), which fixes methane combustion stoichiometry.

## Distribution & Loading

- **The mod is a BepInEx plugin and needs a loader to run from the Workshop.** Subscribing on the Steam Workshop alone does nothing: BepInEx only scans `BepInEx/plugins`, while subscribed mods live in Steam's workshop content folder. **StationeersLaunchPad** is the loader that bridges the two — it discovers the subscribed mod and hands its assembly to BepInEx, which then invokes the `[BepInPlugin]` entry point. Requirements are therefore BepInEx + StationeersLaunchPad. BepInEx must be installed first; StationeersLaunchPad is then extracted into `BepInEx/plugins`. Without a loader, the mod is downloaded but never loaded, and no error is shown.
- **Do not link against StationeersLaunchPad code** — its API is explicitly unstable. The mod stays loader-agnostic (works under StationeersLaunchPad or the older StationeersMods) by exposing only a standard BepInEx entry point.
- **`About/About.xml` `WorkshopHandle` ties updates to the existing item.** Once this mod is published with its own Workshop file ID, that ID must be set in `WorkshopHandle` so publishing updates the existing page instead of creating a duplicate.
