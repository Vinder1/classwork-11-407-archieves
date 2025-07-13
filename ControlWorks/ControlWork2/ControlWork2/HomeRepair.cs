namespace ControlWork2;

public class HomeRepair : RepairWork
{
    public bool HasGarden { get; }
 
    public HomeRepair(string location, DateTime deadline, bool hasGarden = true) : base(location, deadline)
    {
        HasGarden = hasGarden;
    }
    
    public override void CalculateCost()
    {
        var cost = 100_000m;
        if (HasGarden)
            cost *= 2;
        EstimatedCost = cost;
    }

    public override string GetDetails()
    {
        var details = base.GetDetails();
        if (HasGarden)
        {
            details += "\n • Присутствует сад";
        }
        return details;
    }
}