namespace ControlWork2;

public class OfficeRepair : RepairWork
{
    public int NumberOfWorkstations { get; }
    
    /// <exception cref="ArgumentException">If numberOfWorkstations is not positive</exception>
    public OfficeRepair(string location, DateTime deadline, int numberOfWorkstations) : base(location, deadline)
    {
        if (numberOfWorkstations <= 0)
            throw new ArgumentException("Invalid number of workstations: must be positive");
        NumberOfWorkstations = numberOfWorkstations;
    }
    
    public override void CalculateCost()
    {
        EstimatedCost = 70_000m;
    }
    
    public override string GetDetails()
    {
        return base.GetDetails() + "\n • Кол-во рабочих мест: " + NumberOfWorkstations;
    }
}