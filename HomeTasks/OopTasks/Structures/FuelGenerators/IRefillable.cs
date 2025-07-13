namespace Structures.FuelGenerators;

public interface IRefillable
{
    bool RefillRequired { get; }
    void Refill(string supply, int count = 1);
}