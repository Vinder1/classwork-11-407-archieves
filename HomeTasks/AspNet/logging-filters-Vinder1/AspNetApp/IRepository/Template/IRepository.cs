namespace AspNetApp.IRepository.Template;

/// <summary>
/// Generic repository interface for CRUD operations on entities.
/// </summary>
public interface IRepository<TEntity, TId>
{
    /// <summary>
    /// Creates a new entity with the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier for the entity.</param>
    /// <param name="entity">The entity to create.</param>
    /// <returns>The created entity.</returns>
    public Task<TEntity> CreateAsync(TId id, TEntity entity);

    /// <summary>
    /// Retrieves an entity by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>The found entity.</returns>
    public Task<TEntity> GetAsync(TId id);

    /// <summary>
    /// Updates an existing entity by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <param name="entity">The updated entity data.</param>
    public Task UpdateAsync(TId id, TEntity entity);

    /// <summary>
    /// Deletes an entity by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    public Task DeleteAsync(TId id);
}