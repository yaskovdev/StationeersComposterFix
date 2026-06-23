namespace StationeersComposterFix;

using System.Collections.Generic;
using Assets.Scripts.Objects;
using static Assets.Scripts.Atmospherics.Chemistry;

public static class SpawnGasExtensions
{
    public static void Patch(this List<SpawnGas> expelledGas, float steamQuantity)
    {
        if (expelledGas.IsExpectedUnpatched())
        {
            expelledGas.Add(new SpawnGas(GasType.CarbonDioxide, 50, expelledGas[0].Kelvin));
            expelledGas.Add(new SpawnGas(GasType.Steam, steamQuantity, expelledGas[0].Kelvin));
        }
    }

    private static bool IsExpectedUnpatched(this List<SpawnGas> expelledGas) =>
        expelledGas.Count == 2
        && expelledGas[0].Type == GasType.Methane && expelledGas[0].Quantity.Equals(50)
        && expelledGas[1].Type == GasType.Nitrogen && expelledGas[1].Quantity.Equals(50);
}
