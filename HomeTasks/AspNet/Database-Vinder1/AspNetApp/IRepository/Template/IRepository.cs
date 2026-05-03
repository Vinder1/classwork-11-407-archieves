namespace AspNetApp.IRepository.Template;

public interface IRepository<TEntity, TId>
{
    public Task<TEntity> CreateAsync(TId id, TEntity entity);
    public Task<TEntity> GetAsync(TId id);
    public Task UpdateAsync(TId id, TEntity entity);
    public Task DeleteAsync(TId id);
}