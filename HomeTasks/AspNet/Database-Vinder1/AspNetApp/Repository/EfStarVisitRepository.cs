using AspNetApp.Entity;
using AspNetApp.IRepository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AspNetApp.Repository;

public class EfStarVisitRepository(GameDbContext context, IMapper mapper) : IStarVisitRepository
{
    private readonly GameDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task Create(SpaceshipStarVisit visit)
    {
        // Конвертируем domain entity в db entity
        var dbVisit = _mapper.Map<DbEntity.SpaceshipStarVisit>(visit);

        await _context.SpaceshipStarVisits.AddAsync(dbVisit);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsActiveFor(Spaceship spaceship, Star star)
    {
        var oneDayAgo = DateTime.UtcNow.AddDays(-1);

        return await _context.SpaceshipStarVisits
            .AsNoTracking()
            .AnyAsync(v => v.SpaceshipId == spaceship.OwnerId &&
                v.StarRadius == star.Radius &&
                v.StarAngleMilliradians == star.AngleMilliradians &&
                v.VisitedAt >= oneDayAgo);
    }
    
    public async Task<SpaceshipStarVisit?> GetActiveFor(Spaceship spaceship, Star star)
    {
        var oneDayAgo = DateTime.UtcNow.AddDays(-1);

        var dbEntity = await _context.SpaceshipStarVisits
            .AsNoTracking()
            .Include(v => v.Spaceship)
            .Include(v => v.Star)
            .FirstOrDefaultAsync(v => v.SpaceshipId == spaceship.OwnerId &&
                v.StarRadius == star.Radius &&
                v.StarAngleMilliradians == star.AngleMilliradians &&
                v.VisitedAt >= oneDayAgo);
        return _mapper.Map<SpaceshipStarVisit>(dbEntity);
    }
}