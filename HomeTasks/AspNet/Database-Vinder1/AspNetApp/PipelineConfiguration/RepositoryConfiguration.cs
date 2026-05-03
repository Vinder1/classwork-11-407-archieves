using AspNetApp.IRepository;
using AspNetApp.Repository;
using Microsoft.EntityFrameworkCore;

namespace AspNetApp.PipelineConfiguration;

public static class RepositoryConfiguration
{
    public static void AddRepositories(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        
        services.AddDbContext<GameDbContext>(options =>
        {
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("GameDb"));
        });

        services.AddScoped<IPlayerRepository, EfPlayerRepository>();
        services.AddScoped<ISpaceshipRepository, EfSpaceshipRepository>();
        services.AddScoped<IGalaxyRepository, EfGalaxyRepository>();
        services.AddScoped<IStarVisitRepository, EfStarVisitRepository>();
    }
}