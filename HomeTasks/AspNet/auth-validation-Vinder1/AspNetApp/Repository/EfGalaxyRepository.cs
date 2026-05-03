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
public class EfGalaxyRepository(GameDbContext context, IMapper mapper) : IGalaxyRepository
{
    private readonly GameDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<Galaxy?> GetAsync()
    {
        var dbGalaxy = await _context.Galaxies
            .AsNoTracking()
            .Include(g => g.Stars)
            .FirstOrDefaultAsync();
        
        if (dbGalaxy == null)
            return null;
        
        return _mapper.Map<Galaxy>(dbGalaxy);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public async Task OverrideAsync(Galaxy entity)
    {
        var existingGalaxy = await _context.Galaxies
            .Include(g => g.Stars)
            .FirstOrDefaultAsync();
        
        if (existingGalaxy == null)
        {
            var newGalaxy = _mapper.Map<DbEntity.Galaxy>(entity);
            await _context.Galaxies.AddAsync(newGalaxy);
        }
        else
        {
            _context.Galaxies.Remove(existingGalaxy);
            var updatedGalaxy = _mapper.Map<DbEntity.Galaxy>(entity);
            await _context.Galaxies.AddAsync(updatedGalaxy);
        }
        
        await _context.SaveChangesAsync();
    }
}