namespace ControlWork2;

public abstract class RepairWork : ISchedulable
{
    public string Location { get; }
    public decimal EstimatedCost { get; protected set; }
    public DateTime Deadline { get; protected set; }
    public DateTime StartDate { get; protected set; } = new(1, 1, 1);

    public RepairWork(string location, DateTime deadline)
    {
        if (deadline < DateTime.Now)
            throw new ArgumentException("The deadline has already passed");
        Location = location;
        Deadline = deadline;
    }

    public abstract void CalculateCost();

    public virtual string GetDetails()
    {
        CalculateCost();
        var startDate = (StartDate == new DateTime(1, 1, 1))
            ? "Не назначено"
            : StartDate.ToString();
        return $" • Местоположение объекта: {Location}\n"
               + $" • Расчетная стоимость: {EstimatedCost}\n"
               + $" • Срок выполнения: {Deadline}"
               + $"";
    }

    public void Schedule(DateTime startDate)
    {
        StartDate = startDate;
    }
}