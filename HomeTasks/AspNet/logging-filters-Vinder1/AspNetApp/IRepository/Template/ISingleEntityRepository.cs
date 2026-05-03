namespace AspNetApp.IRepository.Template;

/// <summary>
/// Generic repository interface for entities that must always be single
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface ISingleEntityRepository<TEntity>
{
    /// <summary>
    /// Retrieves the single entity instance.
    /// </summary>
    /// <returns>The entity if it exists, otherwise null.</returns>
    public Task<TEntity?> GetAsync();

    /// <summary>
    /// Overwrites the existing entity with the provided new one.
    /// </summary>
    /// <param name="entity">The entity to save as the single instance.</param>
    public Task OverrideAsync(TEntity entity);
}