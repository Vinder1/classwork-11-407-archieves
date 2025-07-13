namespace Structures.FuelGenerators;

public interface IWasteDisposer
{
    void DisposeWaste();
}

public class DoingNothingDisposer : IWasteDisposer
{
    public void DisposeWaste()
    {
        Console.WriteLine("[#] Отходы? Это проблема моих внуков");
    }
}

public class CarefulDisposer : IWasteDisposer
{
    public void DisposeWaste()
    {
        Console.WriteLine("[#] Аккуратно избавляемся от отходов...");
    }
}