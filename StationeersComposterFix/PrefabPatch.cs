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
    /// Adds CO2 to Advanced Composter prefab expelled gases.
    /// </summary>
    internal static void Prefix(Thing sourcePrefab)
    {
        // TODO: consider tuning the regular composter as well for consistency
        // TODO: consider tuning the water consumptions for composters according to the correct reactions
        if (sourcePrefab is AdvancedComposter composter)
        {
            Plugin.Logger?.LogInfo($"Prefab.Register called with: {composter}, expelled gases before patching are: {Format(composter.ExpelledGas)}");
            // TODO: guard against empty ExpelledGas list before accessing [0]
            // TODO: verify that copying ExpelledGas[0].Quantity gives the correct CO2 amount per stoichiometry (2 CH2O → CH4 + CO2)
            if (composter.ExpelledGas.All(it => it.Type != GasType.CarbonDioxide))
            {
                composter.ExpelledGas.Add(new SpawnGas(GasType.CarbonDioxide, composter.ExpelledGas[0].Quantity, composter.ExpelledGas[0].Kelvin));
            }
            Plugin.Logger?.LogInfo($"Expelled gases after patching are: {Format(composter.ExpelledGas)}");
        }
    }

    private static string Format(IReadOnlyCollection<SpawnGas> gases) => string.Join(", ", gases.Select(Format));

    private static string Format(SpawnGas gas) => $"(type: {gas.Type}, quantity: {gas.Quantity}, temperature: {gas.Kelvin})";
}
