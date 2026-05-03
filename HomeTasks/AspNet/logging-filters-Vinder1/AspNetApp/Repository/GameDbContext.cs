using Microsoft.EntityFrameworkCore;

namespace AspNetApp.Repository;

/// <summary>
/// 
/// </summary>
/// <param name="options"></param>
public class GameDbContext(DbContextOptions<GameDbContext> options) : DbContext(options)
{
    /// <summary>
    /// 
    /// </summary>
    public DbSet<DbEntity.Galaxy> Galaxies { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public DbSet<DbEntity.Star> Stars { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public DbSet<DbEntity.Player> Players { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public DbSet<DbEntity.Spaceship> Spaceships { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public DbSet<DbEntity.SpaceshipStarVisit> SpaceshipStarVisits { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // === Galaxy ===
        modelBuilder.Entity<DbEntity.Galaxy>(entity =>
        {
            entity.HasKey(g => g.Version);
            entity.HasMany(g => g.Stars)
                  .WithOne(s => s.Galaxy)
                  .HasForeignKey(s => s.GalaxyVersion)
                  .IsRequired()
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // === Star ===
        modelBuilder.Entity<DbEntity.Star>(entity =>
        {
            entity.HasKey(s => new { s.GalaxyVersion, s.Radius, s.AngleMilliradians });
            entity.HasIndex(s => new { s.GalaxyVersion, s.Radius, s.AngleMilliradians }).IsUnique();
        });

        // === Player ===
        modelBuilder.Entity<DbEntity.Player>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.HasOne(p => p.Spaceship)
              .WithOne(s => s.Owner)
              .HasForeignKey<DbEntity.Spaceship>(s => s.OwnerId)
              .IsRequired(false)
              .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(p => p.Login);
        });

        // === Spaceship ===
        modelBuilder.Entity<DbEntity.Spaceship>(entity =>
        {
            entity.HasKey(sp => sp.OwnerId);

            entity.HasOne(sp => sp.Owner)
                .WithOne(p => p.Spaceship)
                .HasForeignKey<DbEntity.Spaceship>(sp => sp.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(sp => sp.VisitedStars)
                .WithOne(v => v.Spaceship)
                .HasForeignKey(v => v.SpaceshipId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // === M-T-M: Spaceship ↔ Star ===
        modelBuilder.Entity<DbEntity.SpaceshipStarVisit>(entity =>
        {
            entity.HasKey(v => new
            {
                v.SpaceshipId,
                v.StarGalaxyVersion,
                v.StarRadius,
                v.StarAngleMilliradians
            });

            entity.HasOne(v => v.Spaceship)
                .WithMany(s => s.VisitedStars)
                .HasForeignKey(v => v.SpaceshipId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(v => v.Star)
                .WithMany(s => s.VisitedByShips)
                .HasForeignKey(v => new
                {
                    v.StarGalaxyVersion,
                    v.StarRadius,
                    v.StarAngleMilliradians
                })
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(v => v.VisitedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });
    }
}