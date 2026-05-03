using TaskTracker.Api.Domain.Entities;

namespace TaskTracker.Api.Repositories.Interfaces;

public interface IBoardRepository
{
    Task<List<Board>> GetAllAsync(CancellationToken cancellationToken);
    Task<List<Board>> GetAllForProjectAsync(int projectId, CancellationToken cancellationToken);
    Task<Board?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task AddAsync(Board project, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}