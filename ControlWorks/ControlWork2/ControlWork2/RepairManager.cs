namespace ControlWork2;

public class RepairManager : ISchedulable
{
    private List<RepairWork> repairWorks = [];
    
    /// <summary>
    /// Adds StartDate to last added work
    /// </summary>
    /// <param name="startDate">Start date</param>
    /// <exception cref="ArgumentException">If StartTime is earlier than now or later than deadline</exception>
    public void Schedule(DateTime startDate)
    {
        if (repairWorks.Count == 0)
        {
            Console.WriteLine("Работ нет, планировать нечего");
            return;
        }

        var currentWork = repairWorks[^1];
        if (currentWork.Deadline.Date < startDate.Date)
        {
            throw new ArgumentException("Invalid input: Deadline earlier than StartDate");
        }

        if (startDate.Date < DateTime.Now.Date)
        {
            throw new ArgumentException("Invalid input: StartDate has already passed. Delay it");
        }

        currentWork.Schedule(startDate);
        Console.WriteLine($"***Запланирована новая работа***\n" +
                          $"{currentWork.GetDetails()}" +
                          $"\n\n*** Цена: {currentWork.EstimatedCost} ***");
    }

    /// <summary>
    /// Add work to work list
    /// </summary>
    public void AddWork(RepairWork work)
    {
        repairWorks.Add(work);
    }

    /// <returns>
    /// Array of works which deadline is the same as given
    /// </returns>
    public RepairWork[] GetWorksByDeadline(DateTime date)
    {
        var works = new List<RepairWork>();
        foreach (var work in repairWorks)
        {
            if (work.Deadline.Date == date)
            {
                works.Add(work);
            }
        }
        return works.ToArray();
    }
}