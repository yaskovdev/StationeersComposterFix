namespace StationeersComposterFix.Tests;

using Assets.Scripts.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using static Assets.Scripts.Atmospherics.Chemistry;

[TestClass]
public class SpawnGasExtensionsTests
{
    private const float Temperature = 318.15f;

    [TestMethod]
    public void ShouldAddCo2AndSteamToExpectedGases()
    {
        var gases = CreateExpectedUnpatchedGases();
        gases.Patch(20);
        gases.Count.ShouldBe(4);
        gases[2].Type.ShouldBe(GasType.CarbonDioxide);
        gases[2].Quantity.ShouldBe(50);
        gases[3].Type.ShouldBe(GasType.Steam);
        gases[3].Quantity.ShouldBe(20);
    }

    [TestMethod]
    public void ShouldPreserveTemperatureFromOriginalGases()
    {
        var gases = CreateExpectedUnpatchedGases();
        gases.Patch(20);
        gases[2].Kelvin.ShouldBe(Temperature);
        gases[3].Kelvin.ShouldBe(Temperature);
    }

    [TestMethod]
    public void ShouldNotPatchWhenGasTypesAreUnexpected()
    {
        var gases = new List<SpawnGas>
        {
            new(GasType.CarbonDioxide, 50, Temperature),
            new(GasType.Nitrogen, 50, Temperature)
        };
        gases.Patch(20);
        gases.Count.ShouldBe(2);
    }

    [TestMethod]
    public void ShouldNotPatchWhenQuantitiesAreUnexpected()
    {
        var gases = new List<SpawnGas>
        {
            new(GasType.Methane, 100, Temperature),
            new(GasType.Nitrogen, 50, Temperature)
        };
        gases.Patch(20);
        gases.Count.ShouldBe(2);
    }

    [TestMethod]
    public void ShouldNotPatchWhenGasCountIsUnexpected()
    {
        var gases = new List<SpawnGas>
        {
            new(GasType.Methane, 50, Temperature)
        };
        gases.Patch(20);
        gases.Count.ShouldBe(1);
    }

    [TestMethod]
    public void ShouldNotPatchEmptyList()
    {
        var gases = new List<SpawnGas>();
        gases.Patch(20);
        gases.Count.ShouldBe(0);
    }

    [TestMethod]
    public void ShouldNotPatchAlreadyPatchedGases()
    {
        var gases = CreateExpectedUnpatchedGases();
        gases.Patch(20);
        gases.Count.ShouldBe(4);
        gases.Patch(20);
        gases.Count.ShouldBe(4);
    }

    private static List<SpawnGas> CreateExpectedUnpatchedGases() =>
        new() { new SpawnGas(GasType.Methane, 50, Temperature), new SpawnGas(GasType.Nitrogen, 50, Temperature) };
}
