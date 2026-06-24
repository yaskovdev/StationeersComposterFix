namespace StationeersComposterFix;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Assets.Scripts.Objects;
using HarmonyLib;
using Objects.Electrical;

[ExcludeFromCodeCoverage]
[HarmonyPatch(typeof(Prefab), "Register")] // TODO: or RegisterExisting? It's public, by the way, so you can use nameof().
internal class PrefabPatch
{
    /// <summary>
    /// Adds CO2 and Steam to Advanced Composter and Dynamic Composter prefab expelled gases.
    /// </summary>
    internal static void Prefix(Thing sourcePrefab)
    {
        if (sourcePrefab is DynamicComposter dynamicComposter)
        {
            Plugin.Logger?.LogInfo($"Prefab.Register called with: {dynamicComposter}, expelled gases before patching are: {Format(dynamicComposter.ExspelledGas)}, MolesDrainedPerProcessedItem: {DynamicComposter.MolesDrainedPerProcessedItem}");
            dynamicComposter.ExspelledGas.Patch((float)DynamicComposter.MolesDrainedPerProcessedItem.ToDouble());
            Plugin.Logger?.LogInfo($"Expelled gases after patching are: {Format(dynamicComposter.ExspelledGas)}");
        }
        else if (sourcePrefab is AdvancedComposter advancedComposter)
        {
            Plugin.Logger?.LogInfo($"Prefab.Register called with: {advancedComposter}, expelled gases before patching are: {Format(advancedComposter.ExpelledGas)}, MolesDrainedPerProcessedItem: {AdvancedComposter.MolesDrainedPerProcessedItem}");
            advancedComposter.ExpelledGas.Patch((float)AdvancedComposter.MolesDrainedPerProcessedItem.ToDouble());
            Plugin.Logger?.LogInfo($"Expelled gases after patching are: {Format(advancedComposter.ExpelledGas)}");
        }
    }

    private static string Format(IReadOnlyCollection<SpawnGas> gases) => string.Join(", ", gases.Select(Format));

    private static string Format(SpawnGas gas) => $"(type: {gas.Type}, quantity: {gas.Quantity}, temperature: {gas.Kelvin})";
}
