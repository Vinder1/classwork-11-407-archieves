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
public class EfPlayerRepository(GameDbContext context, IMapper mapper) : IPlayerRepository
{
    private readonly GameDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task<Player> CreateAsync(Guid id, Player entity)
    {
        if (entity.Id != id)
            throw new ArgumentException("Entity id does not match provided id");

        var dbEntity = _mapper.Map<DbEntity.Player>(entity);
        await _context.Players.AddAsync(dbEntity);
        await _context.SaveChangesAsync();

        var newEntity = await _context.Players.FindAsync(entity.Id);
        return _mapper.Map<Player>(newEntity);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task<Player> GetAsync(Guid id)
    {
        var player = await _context.Players
            .AsNoTracking()
            .Include(p => p.Spaceship)
            .FirstOrDefaultAsync(p => p.Id == id)
            ?? throw new KeyNotFoundException($"Player with id {id} not found");

        return _mapper.Map<Player>(player);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="login"></param>
    /// <returns></returns>
    public async Task<Player?> GetByLoginAsync(string login)
    {
        var player = await _context.Players
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Login == login);

        if (player == null)
        {
            return null;
        }

        return _mapper.Map<Player>(player);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task UpdateNameAsync(Guid id, string newName)
    {
        var entity = await _context.Players.FindAsync(id);
        ArgumentNullException.ThrowIfNull(entity);

        if (entity.Name == newName)
            return;

        entity.Name = newName;
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task UpdateAsync(Guid id, Player entity)
    {
        if (entity.Id != id)
            throw new ArgumentException("Entity id does not match provided id");

        var existingPlayer = await _context.Players
            .FirstOrDefaultAsync(p => p.Id == id)
            ?? throw new KeyNotFoundException($"Player with id {id} not found");

        _context.Entry(existingPlayer).CurrentValues.SetValues(entity);
        // это конечно круто но я зачем настраивал автомаппер

        //Spaceship не трогаем на всякий случай ибо не понятно а надо или лучше не

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteAsync(Guid id)
    {
        var player = await _context.Players
            .FirstOrDefaultAsync(p => p.Id == id);

        if (player == null)
            return;

        _context.Players.Remove(player);
        await _context.SaveChangesAsync();
    }
}