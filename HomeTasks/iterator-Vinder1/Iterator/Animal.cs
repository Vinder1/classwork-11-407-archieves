namespace IteratorV2;

public class Animal
{
    public string Name { get; }
    public ActivityPeriod ActivityPeriod { get; }

    public Animal(string name, ActivityPeriod activityPeriod)
    {
        Name = name;
        if (activityPeriod == ActivityPeriod.All)
            throw new ArgumentException("Activity period cannot be None");
        ActivityPeriod = activityPeriod;
    }
}