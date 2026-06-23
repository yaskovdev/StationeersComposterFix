namespace StationeersComposterFix;

using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Objects;
using HarmonyLib;
using Objects.Electrical;
using static Assets.Scripts.Atmospherics.Chemistry;

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
            if (IsExpectedUnpatched(composter))
            {
                composter.ExpelledGas.Add(new SpawnGas(GasType.CarbonDioxide, 50, composter.ExpelledGas[0].Kelvin));
                composter.ExpelledGas.Add(new SpawnGas(GasType.Steam, 20, composter.ExpelledGas[0].Kelvin));
            }
            Plugin.Logger?.LogInfo($"Expelled gases after patching are: {Format(composter.ExpelledGas)}");
        }
    }

    private static bool IsExpectedUnpatched(AdvancedComposter composter) =>
        AdvancedComposter.MolesDrainedPerProcessedItem.ToDouble().Equals(20.0)
        && composter.ExpelledGas.Count == 2
        && composter.ExpelledGas[0].Type == GasType.Methane && composter.ExpelledGas[0].Quantity.Equals(50)
        && composter.ExpelledGas[1].Type == GasType.Nitrogen && composter.ExpelledGas[1].Quantity.Equals(50);

    private static string Format(IReadOnlyCollection<SpawnGas> gases) => string.Join(", ", gases.Select(Format));

    private static string Format(SpawnGas gas) => $"(type: {gas.Type}, quantity: {gas.Quantity}, temperature: {gas.Kelvin})";
}
