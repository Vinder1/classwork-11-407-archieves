namespace Structures.FuelGenerators;

public static class FuelLibrary
{
    private static readonly Dictionary<string, FuelInfo> fuelInfos = new();

    public static bool Registred(string id) => fuelInfos.ContainsKey(id);
    public static void RegisterFuel(string id, FuelInfo fuelInfo) => FuelLibrary.fuelInfos.Add(id, fuelInfo);
    public static FuelInfo Get(string id) => fuelInfos[id];
}