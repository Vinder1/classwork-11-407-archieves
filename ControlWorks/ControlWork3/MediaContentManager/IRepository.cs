namespace MediaContentManager;

public interface IRepository<T>
{
    void Add(T item);
    IEnumerable<T> GetAll();
    IEnumerable<T> Find(Func<T, bool> predicate);
}