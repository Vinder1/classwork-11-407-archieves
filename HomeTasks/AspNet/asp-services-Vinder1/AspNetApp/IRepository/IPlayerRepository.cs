using AspNetApp.Entity;
using AspNetApp.IRepository.Template;

namespace AspNetApp.IRepository;

public interface IPlayerRepository : IRepository<Player, Guid> {}