namespace MediaContentManager;

public class Repository<T> : IRepository<T>
{
    private List<T> _items = [];
    
    public void Add(T item) => _items.Add(item);

    public IEnumerable<T> GetAll()
    {
        foreach (var item in _items)
            yield return item;
    }

    public IEnumerable<T> Find(Func<T, bool> predicate)
    {
        foreach (var item in _items.Where(predicate))
            yield return item;
    }
}