namespace ControlWork2;

public class ApartmentRepair : RepairWork
{
    public int FloorNumber { get; }
 

    /// <exception cref="ArgumentException">If flooNumber is not positive (we hate working underground)</exception>
    public ApartmentRepair(string location, DateTime deadline, int floorNumber) : base(location, deadline)
    {
        if (floorNumber <= 0)
            throw new ArgumentException("Invalid floorNumber: must be positive (we are not dwarves, we don`t work underground)");
        FloorNumber = floorNumber;
    }
    
    public override void CalculateCost()
    {
        EstimatedCost = 50_000m + 10_000m * (FloorNumber-1);
    }
    
    public override string GetDetails()
    {
        return base.GetDetails() + "\n • Этаж: " + FloorNumber;
    }
}