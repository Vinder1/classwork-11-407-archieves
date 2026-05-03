using AspNetApp.Entity;
using AspNetApp.IRepository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AspNetApp.Repository;

/// <summary>
/// 
/// </summary>
/// <param name="context"></param>
/// <param name="mapper"></param>
public class EfSpaceshipRepository(GameDbContext context, IMapper mapper) : ISpaceshipRepository
{
    private readonly GameDbContext _dbContext = context;
    private readonly DbSet<DbEntity.Spaceship> _dbSet = context.Spaceships;
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ownerId"></param>
    /// <returns></returns>
    public async Task<Spaceship> GetAsync(Guid ownerId)
    {
        var dbSpaceship = await _dbSet.AsNoTracking()
            // .Include(s => s.Owner)
            // .Include(s => s.Galaxy)
            .FirstOrDefaultAsync(s => s.OwnerId == ownerId);
        return _mapper.Map<Spaceship>(dbSpaceship);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ownerId"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task<Spaceship> CreateAsync(Guid ownerId, Spaceship entity)
    {
        if (entity.OwnerId != ownerId)
            throw new ArgumentException("OwnerId in entity must match the provided id", nameof(ownerId));

        var dbEntity = _mapper.Map<DbEntity.Spaceship>(entity);
        await _dbSet.AddAsync(dbEntity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ownerId"></param>
    /// <returns></returns>
    public Task<Spaceship> GetByOwnerIdAsync(Guid ownerId)
    {
        return GetAsync(ownerId);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ownerId"></param>
    /// <param name="newRadius"></param>
    /// <param name="newAngleMilliradians"></param>
    /// <returns></returns>
    public async Task MoveAsync(Guid ownerId, int newRadius, int newAngleMilliradians)
    {
        var ship = await _dbSet.FindAsync(ownerId);
        if (ship != null)
        {
            ship.LocatedRadius = newRadius;
            ship.LocatedAngleMilliradians = newAngleMilliradians;
            await _dbContext.SaveChangesAsync();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<Spaceship>> GetAllAsync()
    {
        var entities = await _dbSet.AsNoTracking()
            // .Include(s => s.Owner)
            // .Include(s => s.Galaxy)
            .ToListAsync();
        return _mapper.Map<IEnumerable<Spaceship>>(entities);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="radius"></param>
    /// <param name="angleMilliradians"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Spaceship>> GetAllOnStarAsync(int radius, int angleMilliradians)
    {
        var spaceships = await _dbSet.AsNoTracking()
            .Where(v => v.LocatedRadius == radius &&
                v.LocatedAngleMilliradians == angleMilliradians)
            .ToListAsync();

        return _mapper.Map<IEnumerable<Spaceship>>(spaceships);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="galaxyVersion"></param>
    /// <returns></returns>
    public async Task DeleteInactiveAsync(int galaxyVersion)
    {
        var inactiveShips = await _dbSet
            .Where(s => s.GalaxyVersion < galaxyVersion)
            .ToListAsync();

        _dbSet.RemoveRange(inactiveShips);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ownerId"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Spaceship>> GetByOwnerWithVisitedStarsAsync(Guid ownerId)
    {
        var entities = await _dbSet.AsNoTracking()
            .Include(s => s.VisitedStars).ThenInclude(v => v.Star)
            .Where(s => s.OwnerId == ownerId)
            .ToListAsync();
        return _mapper.Map<IEnumerable<Spaceship>>(entities);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ownerId"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task UpdateAsync(Guid ownerId, Spaceship entity)
    {
        if (entity.OwnerId != ownerId)
            throw new ArgumentException("OwnerId cannot be changed", nameof(ownerId));
        var dbEntity = _mapper.Map<DbEntity.Spaceship>(entity);
        _dbSet.Update(dbEntity);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ownerId"></param>
    /// <returns></returns>
    public async Task DeleteAsync(Guid ownerId)
    {
        var spaceship = await _dbSet
            .Include(s => s.VisitedStars) // Загружаем связанные записи для каскада
            .FirstOrDefaultAsync(s => s.OwnerId == ownerId);

        if (spaceship == null)
            return;

        _dbSet.Remove(spaceship);
        await _dbContext.SaveChangesAsync();
    }
}