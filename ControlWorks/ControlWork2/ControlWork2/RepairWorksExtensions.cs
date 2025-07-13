namespace ControlWork2;

public static class RepairWorksExtensions
{
    public static decimal CalculateTotalCost(this RepairWork[] repairWorks)
    {
        var cost = 0m;
        foreach (var work in repairWorks)
        {
            work.CalculateCost();
            cost += work.EstimatedCost;
        }
        return cost;
    }
}