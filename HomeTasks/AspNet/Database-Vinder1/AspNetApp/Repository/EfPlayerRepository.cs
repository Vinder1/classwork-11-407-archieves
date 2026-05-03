using AspNetApp.Entity;
using AspNetApp.IRepository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AspNetApp.Repository;

public class EfPlayerRepository(GameDbContext context, IMapper mapper) : IPlayerRepository
{
    private readonly GameDbContext _context = context;
    private readonly IMapper _mapper = mapper;

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

    public async Task<Player> GetAsync(Guid id)
    {
        var player = await _context.Players
            .AsNoTracking()
            .Include(p => p.Spaceship)
            .FirstOrDefaultAsync(p => p.Id == id)
            ?? throw new KeyNotFoundException($"Player with id {id} not found");

        return _mapper.Map<Player>(player);
    }

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