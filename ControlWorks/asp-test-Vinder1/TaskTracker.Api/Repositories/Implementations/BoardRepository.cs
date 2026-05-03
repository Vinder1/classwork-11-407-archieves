using Microsoft.EntityFrameworkCore;
using TaskTracker.Api.Data;
using TaskTracker.Api.Domain.Entities;
using TaskTracker.Api.Repositories.Interfaces;

namespace TaskTracker.Api.Repositories.Implementations;

public class BoardRepository : IBoardRepository
{
    private readonly AppDbContext _dbContext;

    public BoardRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<Board>> GetAllAsync(CancellationToken cancellationToken)
    {
        return _dbContext.Boards
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
    
    public Task<List<Board>> GetAllForProjectAsync(int projectId, CancellationToken cancellationToken)
    {
        return _dbContext.Boards
            .AsNoTracking()
            .Where(b => b.ProjectId == projectId)
            .ToListAsync(cancellationToken);
    }

    public Task<Board?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return _dbContext.Boards
            .Include(b => b.Tasks)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task AddAsync(Board board, CancellationToken cancellationToken)
    {
        await _dbContext.Boards.AddAsync(board, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}