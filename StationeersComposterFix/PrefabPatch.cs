namespace StationeersComposterFix;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Assets.Scripts.Objects;
using HarmonyLib;
using Objects.Electrical;

[ExcludeFromCodeCoverage]
[HarmonyPatch(typeof(Prefab), "Register")]
internal class PrefabPatch
{
    /// <summary>
    /// Adds CO2 and Steam to Advanced Composter prefab expelled gases.
    /// </summary>
    internal static void Prefix(Thing sourcePrefab)
    {
        // TODO: consider tuning the regular composter as well for consistency
        if (sourcePrefab is AdvancedComposter composter)
        {
            Plugin.Logger?.LogInfo($"Prefab.Register called with: {composter}, expelled gases before patching are: {Format(composter.ExpelledGas)}, MolesDrainedPerProcessedItem: {AdvancedComposter.MolesDrainedPerProcessedItem}");
            composter.ExpelledGas.Patch((float)AdvancedComposter.MolesDrainedPerProcessedItem.ToDouble());
            Plugin.Logger?.LogInfo($"Expelled gases after patching are: {Format(composter.ExpelledGas)}");
        }
    }

    private static string Format(IReadOnlyCollection<SpawnGas> gases) => string.Join(", ", gases.Select(Format));

    private static string Format(SpawnGas gas) => $"(type: {gas.Type}, quantity: {gas.Quantity}, temperature: {gas.Kelvin})";
}
