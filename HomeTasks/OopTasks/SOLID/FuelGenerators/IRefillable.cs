namespace SOLID.FuelGenerators;

public interface IRefillable
{
    bool RefillRequired { get; }
    void Refill(int amount);
}