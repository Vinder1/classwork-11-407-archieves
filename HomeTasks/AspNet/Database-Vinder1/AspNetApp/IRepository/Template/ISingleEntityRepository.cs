namespace AspNetApp.IRepository.Template;

/// <summary>
/// For entities that must always be single
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface ISingleEntityRepository<TEntity>
{
    public Task<TEntity?> GetAsync();
    public Task OverrideAsync(TEntity entity);
}