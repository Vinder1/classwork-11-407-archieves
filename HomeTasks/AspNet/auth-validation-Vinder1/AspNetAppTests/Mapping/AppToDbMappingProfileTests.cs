using AspNetApp.Entity;
using AspNetApp.Mapping;
using AutoFixture;
using AutoMapper;
using Castle.Core.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;

namespace AspNetAppTests.Mapping;

public class AppToDbMappingProfileTests
{
    private readonly IMapper _mapper;
    private readonly IMapper _galaxyMapper;
    protected readonly Fixture _fixture;

    public AppToDbMappingProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<AppToDbMappingProfile>();
        }, new NullLoggerFactory());

        // Валидация конфигурации при запуске тестов
        config.AssertConfigurationIsValid();
        _mapper = config.CreateMapper();

        // Galaxy Mapper
        var services = new ServiceCollection();
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<AppToDbMappingProfile>();
        });
        services.AddSingleton<GalaxyConverter>();
        services.AddSingleton<GalaxyReverseConverter>();
        services.AddSingleton<Microsoft.Extensions.Logging.ILoggerFactory>(new NullLoggerFactory());
        var _serviceProvider = services.BuildServiceProvider();
        _galaxyMapper = _serviceProvider.GetRequiredService<IMapper>();

        // Fixture
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    #region Helper Methods

    private AspNetApp.Repository.DbEntity.Star CreateDbStar(int galaxyVersion, int radius, int angleMilliradians)
    {
        return _fixture.Build<AspNetApp.Repository.DbEntity.Star>()
            .With(s => s.AngleMilliradians, angleMilliradians)
            .With(s => s.Radius, radius)
            .With(s => s.GalaxyVersion, galaxyVersion)
            .Without(s => s.Galaxy)
            .Without(s => s.VisitedByShips)
            .Create();
    }

    private AspNetApp.Repository.DbEntity.Star CreateDbStar()
    {
        return _fixture.Build<AspNetApp.Repository.DbEntity.Star>()
            .Without(s => s.Galaxy)
            .Without(s => s.VisitedByShips)
            .Create();
    }

    private Star CreateDomainStar()
    {
        return _fixture.Build<Star>()
            .Without(s => s.VisitedByShips)
            .Create();
    }

    private SpaceshipStarVisit CreateDomainVisit(Guid spaceshipId, int galaxyVersion)
    {
        return new SpaceshipStarVisit
        {
            Spaceship = new Spaceship
            {
                OwnerId = spaceshipId
            },
            Star = CreateDomainStar(),
            VisitedAt = DateTime.Now,
            GalaxyVersion = galaxyVersion
        };
    }

    private static AspNetApp.Repository.DbEntity.SpaceshipStarVisit CreateDbVisit(
        Guid spaceshipId, AspNetApp.Repository.DbEntity.Star star)
    {
        return new AspNetApp.Repository.DbEntity.SpaceshipStarVisit
        {
            SpaceshipId = spaceshipId,
            StarGalaxyVersion = star.GalaxyVersion,
            StarRadius = star.Radius,
            StarAngleMilliradians = star.AngleMilliradians,
            VisitedAt = DateTime.Now,
            Star = star,
            Spaceship = new AspNetApp.Repository.DbEntity.Spaceship { OwnerId = spaceshipId }
        };
    }

    #endregion

    [Test]
    public async Task Map_Star_DomainToDb_WithGalaxyVersion_ShouldMapAllProperties()
    {
        // Arrange
        var domainStar = CreateDomainStar();
        var galaxyVersion = 42;

        // Act
        var dbStar = _mapper.Map<AspNetApp.Repository.DbEntity.Star>(domainStar, opts =>
        {
            opts.Items["GalaxyVersion"] = galaxyVersion;
        });

        // Assert
        await Assert.That(dbStar.Radius).IsEqualTo(domainStar.Radius);
        await Assert.That(dbStar.AngleMilliradians).IsEqualTo(domainStar.AngleMilliradians);
        await Assert.That(dbStar.GalaxyVersion).IsEqualTo(galaxyVersion);
    }

    [Test]
    public async Task Map_Star_DomainToDb_WithoutGalaxyVersion_ShouldThrow()
    {
        // Arrange
        var domainStar = CreateDomainStar();

        // Act & Assert
        await Assert.That(() => _mapper.Map<AspNetApp.Repository.DbEntity.Star>(domainStar))
            .ThrowsException();
    }

    [Test]
    public async Task Map_Star_DbToDomain_ShouldMapAllPropertiesExceptGalaxyVersion()
    {
        // Arrange
        var dbStar = CreateDbStar();

        // Act
        var domainStar = _mapper.Map<Star>(dbStar);

        // Assert
        await Assert.That(domainStar.Radius).IsEqualTo(dbStar.Radius);
        await Assert.That(domainStar.AngleMilliradians).IsEqualTo(dbStar.AngleMilliradians);
        // GalaxyVersion не маппится в домен (у домена нет такого поля)
    }

    [Test]
    public async Task Map_Star_DomainToDb_ZeroGalaxyVersion_ShouldStillMap()
    {
        // Arrange
        var domainStar = CreateDomainStar();
        var galaxyVersion = 0;

        // Act
        var dbStar = _mapper.Map<AspNetApp.Repository.DbEntity.Star>(domainStar, opts =>
        {
            opts.Items["GalaxyVersion"] = galaxyVersion;
        });

        // Assert
        await Assert.That(dbStar.GalaxyVersion).IsEqualTo(0);
        await Assert.That(dbStar.Radius).IsEqualTo(domainStar.Radius);
        await Assert.That(dbStar.AngleMilliradians).IsEqualTo(domainStar.AngleMilliradians);
    }

    [Test]
    public async Task Map_Spaceship_DomainToDb_ShouldMapAllProperties()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var domainShip = new Spaceship
        {
            OwnerId = ownerId,
            GalaxyVersion = 5,
            LocatedRadius = 300,
            LocatedAngleMilliradians = 120,
        };

        // Act
        var dbShip = _mapper.Map<AspNetApp.Repository.DbEntity.Spaceship>(domainShip);

        // Assert
        await Assert.That(dbShip.OwnerId).IsEqualTo(ownerId);
        await Assert.That(dbShip.GalaxyVersion).IsEqualTo(domainShip.GalaxyVersion);
        await Assert.That(dbShip.LocatedRadius).IsEqualTo(domainShip.LocatedRadius);
        await Assert.That(dbShip.LocatedAngleMilliradians).IsEqualTo(domainShip.LocatedAngleMilliradians);
    }

    [Test]
    public async Task Map_Spaceship_DbToDomain_ShouldMapAllProperties()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var dbShip = new AspNetApp.Repository.DbEntity.Spaceship
        {
            OwnerId = ownerId,
            GalaxyVersion = 3,
            LocatedRadius = 250,
            LocatedAngleMilliradians = 60,
        };

        // Act
        var domainShip = _mapper.Map<Spaceship>(dbShip);

        // Assert
        await Assert.That(domainShip.OwnerId).IsEqualTo(ownerId);
        await Assert.That(domainShip.GalaxyVersion).IsEqualTo(dbShip.GalaxyVersion);
        await Assert.That(domainShip.LocatedRadius).IsEqualTo(dbShip.LocatedRadius);
        await Assert.That(domainShip.LocatedAngleMilliradians).IsEqualTo(dbShip.LocatedAngleMilliradians);
    }

    [Test]
    public async Task Map_Spaceship_RoundTrip_ShouldPreserveData()
    {
        // Arrange
        var original = new Spaceship
        {
            OwnerId = Guid.NewGuid(),
            GalaxyVersion = 7,
            LocatedRadius = 400,
            LocatedAngleMilliradians = 270
        };

        // Act: Domain → Db → Domain
        var db = _mapper.Map<AspNetApp.Repository.DbEntity.Spaceship>(original);
        var result = _mapper.Map<Spaceship>(db);

        // Assert
        await Assert.That(result.OwnerId).IsEqualTo(original.OwnerId);
        await Assert.That(result.GalaxyVersion).IsEqualTo(original.GalaxyVersion);
        await Assert.That(result.LocatedRadius).IsEqualTo(original.LocatedRadius);
        await Assert.That(result.LocatedAngleMilliradians).IsEqualTo(original.LocatedAngleMilliradians);
    }

    [Test]
    public async Task Map_Player_DomainToDb_ShouldMapAllProperties()
    {
        // Arrange
        var playerId = Guid.NewGuid();
        var domainPlayer = new Player
        {
            Id = playerId,
            Name = "Commander Shepard",
            RegisteredAt = DateTime.Now
        };

        // Act
        var dbPlayer = _mapper.Map<AspNetApp.Repository.DbEntity.Player>(domainPlayer);

        // Assert
        await Assert.That(dbPlayer.Id).IsEqualTo(playerId);
        await Assert.That(dbPlayer.Name).IsEqualTo(domainPlayer.Name);
        await Assert.That(dbPlayer.RegisteredAt).IsEqualTo(domainPlayer.RegisteredAt);
    }

    [Test]
    public async Task Map_Player_DbToDomain_ShouldMapAllProperties()
    {
        // Arrange
        var playerId = Guid.NewGuid();
        var dbPlayer = new AspNetApp.Repository.DbEntity.Player
        {
            Id = playerId,
            Name = "John Doe",
            RegisteredAt = DateTime.Now
        };

        // Act
        var domainPlayer = _mapper.Map<Player>(dbPlayer);

        // Assert
        await Assert.That(domainPlayer.Id).IsEqualTo(playerId);
        await Assert.That(domainPlayer.Name).IsEqualTo(dbPlayer.Name);
        await Assert.That(domainPlayer.RegisteredAt).IsEqualTo(dbPlayer.RegisteredAt);
    }

    [Test]
    public async Task Map_Player_RoundTrip_ShouldPreserveData()
    {
        // Arrange
        var original = new Player
        {
            Id = Guid.NewGuid(),
            Name = "Test Player",
            RegisteredAt = DateTime.UtcNow
        };

        // Act: Domain → Db → Domain
        var db = _mapper.Map<AspNetApp.Repository.DbEntity.Player>(original);
        var result = _mapper.Map<Player>(db);

        // Assert
        await Assert.That(result.Id).IsEqualTo(original.Id);
        await Assert.That(result.Name).IsEqualTo(original.Name);
    }

    #region SpaceshipVisit

    [Test]
    public async Task Map_DbToDomain_WithNavigation_ShouldMapStarAndSpaceship()
    {
        // Arrange
        var spaceshipId = Guid.NewGuid();
        var dbVisit = CreateDbVisit(spaceshipId, CreateDbStar());

        // Act
        var domainVisit = _mapper.Map<SpaceshipStarVisit>(dbVisit);

        // Assert
        await Assert.That(domainVisit.VisitedAt).IsEqualTo(dbVisit.VisitedAt);

        // Проверяем, что звезда замаппилась
        await Assert.That(domainVisit.Star).IsNotDefault();
        await Assert.That(domainVisit.Star.Radius).IsEqualTo(dbVisit.StarRadius);
        await Assert.That(domainVisit.Star.AngleMilliradians).IsEqualTo(dbVisit.StarAngleMilliradians);

        // Корабль тоже должен замаппиться
        await Assert.That(domainVisit.Spaceship).IsNotNull();
        await Assert.That(domainVisit.Spaceship.OwnerId).IsEqualTo(spaceshipId);
    }

    [Test]
    public async Task Map_DomainToDb_WithGalaxyVersion_ShouldMapAllProperties()
    {
        // Arrange
        var spaceshipId = Guid.NewGuid();
        var galaxyVersion = 42;
        var domainVisit = CreateDomainVisit(spaceshipId, galaxyVersion);

        // Act
        var dbVisit = _mapper.Map<AspNetApp.Repository.DbEntity.SpaceshipStarVisit>(domainVisit);

        // Assert
        await Assert.That(dbVisit.SpaceshipId).IsEqualTo(spaceshipId);
        await Assert.That(dbVisit.StarGalaxyVersion).IsEqualTo(galaxyVersion);
        await Assert.That(dbVisit.StarRadius).IsEqualTo(domainVisit.Star.Radius);
        await Assert.That(dbVisit.StarAngleMilliradians).IsEqualTo(domainVisit.Star.AngleMilliradians);
        await Assert.That(dbVisit.VisitedAt).IsEqualTo(domainVisit.VisitedAt);
    }

    #endregion

    #region Galaxy

    [Test]
    public async Task GalaxyConverter_ShouldMapDbEntityToDomain_WithAllStars()
    {
        // Arrange
        var dbGalaxy = new AspNetApp.Repository.DbEntity.Galaxy
        {
            Version = 123,
            Stars =
            [
                new() { GalaxyVersion = 123, Radius = 10, AngleMilliradians = 45 },
                new() { GalaxyVersion = 123, Radius = 20, AngleMilliradians = 90 },
                new() { GalaxyVersion = 123, Radius = 15, AngleMilliradians = 135 }
            ]
        };

        // Act
        var domainGalaxy = _galaxyMapper.Map<Galaxy>(dbGalaxy);

        // Assert
        await Assert.That(domainGalaxy).IsNotNull();
        await Assert.That(domainGalaxy.Version).IsEqualTo(dbGalaxy.Version);

        var allStars = domainGalaxy.Stars.GetAllStars().ToList();
        await Assert.That(allStars).Contains(s => s.Radius == 10 && s.AngleMilliradians == 45);
        await Assert.That(allStars).Contains(s => s.Radius == 20 && s.AngleMilliradians == 90);
        await Assert.That(allStars).Contains(s => s.Radius == 15 && s.AngleMilliradians == 135);
    }

    [Test]
    public async Task GalaxyConverter_ShouldHandleEmptyStarCollection()
    {
        // Arrange
        var dbGalaxy = new AspNetApp.Repository.DbEntity.Galaxy
        {
            Version = 456,
            Stars = []
        };

        // Act
        var domainGalaxy = _galaxyMapper.Map<Galaxy>(dbGalaxy);

        // Assert
        await Assert.That(domainGalaxy).IsNotNull();
        await Assert.That(domainGalaxy.Version).IsEqualTo(456);
        await Assert.That(domainGalaxy.Stars.GetAllStars()).IsEmpty();
    }

    [Test]
    public async Task GalaxyReverseConverter_ShouldMapDomainToDbEntity_WithAllStars()
    {
        // Arrange
        var star1 = CreateDomainStar() with { Radius = 1 };
        var star2 = CreateDomainStar() with { Radius = 3 };
        var star3 = CreateDomainStar() with { Radius = 2 };

        var stars = new Star[][] { [star1], [star2], [star3], };
        var starCollection = new StarCollection(stars);

        var domainGalaxy = new Galaxy
        {
            Version = 999,
            Stars = starCollection
        };

        // Act
        var dbGalaxy = _galaxyMapper.Map<AspNetApp.Repository.DbEntity.Galaxy>(domainGalaxy);

        // Assert
        await Assert.That(dbGalaxy).IsNotNull();
        await Assert.That(dbGalaxy.Version).IsEqualTo(999);
        await Assert.That(dbGalaxy.Stars).Count().IsEqualTo(3);

        await Assert.That(dbGalaxy.Stars).Contains(s => s.Radius == star1.Radius && s.AngleMilliradians == star1.AngleMilliradians);
        await Assert.That(dbGalaxy.Stars).Contains(s => s.Radius == star2.Radius && s.AngleMilliradians == star2.AngleMilliradians);
        await Assert.That(dbGalaxy.Stars).Contains(s => s.Radius == star3.Radius && s.AngleMilliradians == star3.AngleMilliradians);


        foreach (var star in dbGalaxy.Stars)
        {
            await Assert.That(star.GalaxyVersion).IsEqualTo(999);
        }
    }

    [Test]
    public async Task BidirectionalMapping_ShouldPreserveData()
    {
        // Arrange
        var originalDbGalaxy = new AspNetApp.Repository.DbEntity.Galaxy
        {
            Version = 333,
            Stars =
            [
                new AspNetApp.Repository.DbEntity.Star { GalaxyVersion = 333, Radius=2 },
                new AspNetApp.Repository.DbEntity.Star { GalaxyVersion = 333, Radius=1 },
            ]
        };

        // Act
        var domainGalaxy = _galaxyMapper.Map<Galaxy>(originalDbGalaxy);
        var mappedBackDbGalaxy = _galaxyMapper.Map<AspNetApp.Repository.DbEntity.Galaxy>(domainGalaxy);

        // Assert
        await Assert.That(mappedBackDbGalaxy).IsNotNull();
        await Assert.That(mappedBackDbGalaxy.Version).IsEqualTo(originalDbGalaxy.Version);
        await Assert.That(mappedBackDbGalaxy.Stars).Count().IsEqualTo(originalDbGalaxy.Stars.Count);

        foreach (var originalStar in originalDbGalaxy.Stars)
        {
            await Assert.That(mappedBackDbGalaxy.Stars).Contains(s =>
                s.Radius == originalStar.Radius &&
                s.AngleMilliradians == originalStar.AngleMilliradians);
        }
    }

    #endregion
}