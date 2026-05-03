using AutoFixture;
using Moq;
using AspNetApp.Services;
using AspNetApp.IRepository;
using AspNetApp.Entity;
using AspNetApp.Exceptions;

namespace AspNetAppTests.Services;

public class GalaxyServiceTests
{
    private readonly Fixture _fixture;
    private readonly Mock<IGalaxyRepository> _galaxyRepositoryMock;

    public GalaxyServiceTests()
    {
        _fixture = new Fixture();
        // Убираем циклические зависимости для авто-генерации
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _fixture.Register<Star>(() => new Star
        {
            Radius = _fixture.Create<int>(),
            Angle = Random.Shared.NextDouble() * 2 * Math.PI
        });

        _galaxyRepositoryMock = new Mock<IGalaxyRepository>();
    }

    #region GetCurrentGalaxyVersionAsync

    [Test]
    public async Task GetCurrentGalaxyVersionAsync_GalaxyExists_ReturnsVersion()
    {
        // Arrange
        var expectedVersion = _fixture.Create<int>();
        var galaxy = _fixture.Build<Galaxy>()
            .With(g => g.Version, expectedVersion)
            .With(g => g.Stars, (IStarCollection?)null)
            .Create();

        _galaxyRepositoryMock.Setup(repo => repo.GetAsync())
            .ReturnsAsync(galaxy);

        var service = new GalaxyService(_galaxyRepositoryMock.Object);

        // Act
        var result = await service.GetCurrentGalaxyVersionAsync();

        // Assert
        await Assert.That(result).IsEqualTo(expectedVersion);
        _galaxyRepositoryMock.Verify(repo => repo.GetAsync(), Times.Once);
    }

    [Test]
    public async Task GetCurrentGalaxyVersionAsync_GalaxyMissing_ThrowsGalaxyMissingException()
    {
        // Arrange
        _galaxyRepositoryMock.Setup(repo => repo.GetAsync())
            .ReturnsAsync((Galaxy?)null);

        var service = new GalaxyService(_galaxyRepositoryMock.Object);

        // Act & Assert
        await Assert.That(service.GetCurrentGalaxyVersionAsync)
            .ThrowsExactly<GalaxyMissingException>();
    }

    #endregion

    #region RegenerateGalaxyAsync

    [Test]
    public async Task RegenerateGalaxyAsync_ExistingGalaxy_IncrementsVersion()
    {
        // Arrange
        var currentVersion = _fixture.Create<int>();
        var currentGalaxy = _fixture.Build<Galaxy>()
            .With(g => g.Version, currentVersion)
            .With(g => g.Stars, (IStarCollection?)null)
            .Create();

        _galaxyRepositoryMock.Setup(repo => repo.GetAsync())
            .ReturnsAsync(currentGalaxy);

        // Мокаем OverrideAsync чтобы не выполнять реальную запись
        _galaxyRepositoryMock.Setup(repo => repo.OverrideAsync(It.IsAny<Galaxy>()))
            .Returns(Task.CompletedTask);

        var service = new GalaxyService(_galaxyRepositoryMock.Object);

        // Act
        var result = await service.RegenerateGalaxyAsync();

        // Assert
        await Assert.That(result.Version).IsEqualTo(currentVersion + 1);
        await Assert.That(result.Stars).IsNotNull();

        // Проверяем, что репозиторий был вызван для сохранения
        _galaxyRepositoryMock.Verify(repo => repo.OverrideAsync(
            It.Is<Galaxy>(g => g.Version == currentVersion + 1)), Times.Once);
    }

    [Test]
    public async Task RegenerateGalaxyAsync_NoExistingGalaxy_StartsFromVersion1()
    {
        // Arrange
        _galaxyRepositoryMock.Setup(repo => repo.GetAsync())
            .ReturnsAsync((Galaxy?)null);

        _galaxyRepositoryMock.Setup(repo => repo.OverrideAsync(It.IsAny<Galaxy>()))
            .Returns(Task.CompletedTask);

        var service = new GalaxyService(_galaxyRepositoryMock.Object);

        // Act
        var result = await service.RegenerateGalaxyAsync();

        // Assert
        await Assert.That(result.Version).IsEqualTo(1);
        await Assert.That(result.Stars).IsNotNull();
    }

    [Test]
    public async Task RegenerateGalaxyAsync_GeneratedStars_AreValid()
    {
        // Arrange
        _galaxyRepositoryMock.Setup(repo => repo.GetAsync())
            .ReturnsAsync((Galaxy?)null);

        _galaxyRepositoryMock.Setup(repo => repo.OverrideAsync(It.IsAny<Galaxy>()))
            .Returns(Task.CompletedTask);

        var service = new GalaxyService(_galaxyRepositoryMock.Object);

        // Act
        var result = await service.RegenerateGalaxyAsync();

        // Assert - проверяем инварианты сгенерированной галактики
        await Assert.That(result.Stars).IsNotNull();
        // GalaxyGenerator создаёт массив [maxRadius+1][], где [0] = null
        await Assert.That(result.Stars.GetAllStars()).IsNotEmpty();
    }

    #endregion

    #region GetAllStarsAsync

    [Test]
    public async Task GetAllStarsAsync_GalaxyExists_ReturnsAllStars()
    {
        var maxRadius = 5;
        var stars = new Star[maxRadius + 1][];

        for (int r = 1; r <= maxRadius; r++)
        {
            stars[r] = _fixture.CreateMany<Star>(4)
                .Select(s => s with { Radius = r })
                .ToArray();
        }
        var starCollection = new StarCollection(stars);

        var galaxy = _fixture.Build<Galaxy>()
            .With(g => g.Stars, starCollection)
            .Create();

        _galaxyRepositoryMock.Setup(repo => repo.GetAsync())
            .ReturnsAsync(galaxy);

        var service = new GalaxyService(_galaxyRepositoryMock.Object);

        // Act
        var result = await service.GetAllStarsAsync();

        // Assert
        var expectedFlat = stars.Where(s => s != null).SelectMany(s => s!).ToArray();
        await Assert.That(result).IsEquivalentTo(expectedFlat);
        _galaxyRepositoryMock.Verify(repo => repo.GetAsync(), Times.Once);
    }

    [Test]
    public async Task GetAllStarsAsync_GalaxyMissing_ThrowsGalaxyMissingException()
    {
        // Arrange
        _galaxyRepositoryMock.Setup(repo => repo.GetAsync())
            .ReturnsAsync((Galaxy?)null);

        var service = new GalaxyService(_galaxyRepositoryMock.Object);

        // Act & Assert
        await Assert.That(async () => await service.GetAllStarsAsync())
            .ThrowsExactly<GalaxyMissingException>();
    }

    [Test]
    public async Task GetAllStarsAsync_GalaxyHasNullStars_ThrowsNullReferenceException()
    {
        // Arrange
        var galaxy = _fixture.Build<Galaxy>()
            .With(g => g.Stars, (StarCollection?)null)
            .Create();

        _galaxyRepositoryMock.Setup(repo => repo.GetAsync())
            .ReturnsAsync(galaxy);

        var service = new GalaxyService(_galaxyRepositoryMock.Object);

        // Act & Assert
        await Assert.That(async () => await service.GetAllStarsAsync())
            .ThrowsExactly<NullReferenceException>();
    }

    #endregion

    #region FindStarAsync

    [Test]
    [Arguments(5, 1.5)]
    [Arguments(10, 3.14)]
    [Arguments(1, 0.0)]
    public async Task FindStarAsync_StarFound_ReturnsStar(int radius, double angle)
    {
        // Arrange
        var expectedStar = new Star
        {
            Radius=radius, Angle=angle
        };

        var starCollectionMock = new Mock<IStarCollection>([]) { CallBase = true };
        starCollectionMock.Setup(sc => sc.Find(radius, angle))
            .Returns(expectedStar);

        var galaxy = _fixture.Build<Galaxy>()
            .With(g => g.Stars, starCollectionMock.Object)
            .Create();

        _galaxyRepositoryMock.Setup(repo => repo.GetAsync())
            .ReturnsAsync(galaxy);

        var service = new GalaxyService(_galaxyRepositoryMock.Object);

        // Act
        var result = await service.FindStarAsync(radius, angle);

        // Assert
        await Assert.That(result).IsEqualTo(expectedStar);
    }

    [Test]
    public async Task FindStarAsync_StarNotFound_ReturnsNull()
    {
        // Arrange
        var starCollectionMock = new Mock<IStarCollection>([]) { CallBase = true };
        starCollectionMock.Setup(sc => sc.Find(999, 9.99))
            .Returns((Star?)null);

        var galaxy = _fixture.Build<Galaxy>()
            .With(g => g.Stars, starCollectionMock.Object)
            .Create();

        _galaxyRepositoryMock.Setup(repo => repo.GetAsync())
            .ReturnsAsync(galaxy);

        var service = new GalaxyService(_galaxyRepositoryMock.Object);

        // Act
        var result = await service.FindStarAsync(999, 9.99);

        // Assert
        await Assert.That(result).IsNull();
    }

    [Test]
    public async Task FindStarAsync_GalaxyMissing_ThrowsGalaxyMissingException()
    {
        // Arrange
        _galaxyRepositoryMock.Setup(repo => repo.GetAsync())
            .ReturnsAsync((Galaxy?)null);

        var service = new GalaxyService(_galaxyRepositoryMock.Object);

        // Act & Assert
        await Assert.That(async () => await service.FindStarAsync(1, 1.0))
            .ThrowsExactly<GalaxyMissingException>();
    }

    #endregion

    #region GetStarsNearStarAsync

    [Test]
    public async Task GetStarsNearStarAsync_ValidCenter_ReturnsNearbyStars()
    {
        // Arrange
        var centerStar = new Star()
        {
            Radius = 10,
            Angle = 1.5
        };

        var expectedNearby = _fixture.CreateMany<Star>(3).ToArray();

        var starCollectionMock = new Mock<IStarCollection>([]) { CallBase = true };
        starCollectionMock.Setup(sc => sc.FindNearbyStars(10, 1.5, 5))
            .Returns(expectedNearby);

        var galaxy = _fixture.Build<Galaxy>()
            .With(g => g.Stars, starCollectionMock.Object)
            .Create();

        _galaxyRepositoryMock.Setup(repo => repo.GetAsync())
            .ReturnsAsync(galaxy);

        var service = new GalaxyService(_galaxyRepositoryMock.Object);

        // Act
        var result = await service.GetStarsNearStarAsync(centerStar, 5);

        // Assert
        await Assert.That(result).IsEquivalentTo(expectedNearby);
        starCollectionMock.Verify(sc => sc.FindNearbyStars(10, 1.5, 5), Times.Once);
    }

    [Test]
    public async Task GetStarsNearStarAsync_NoNearbyStars_ReturnsEmpty()
    {
        // Arrange
        var centerStar = _fixture.Create<Star>();

        var starCollectionMock = new Mock<IStarCollection>([]) { CallBase = true };
        starCollectionMock.Setup(sc => sc.FindNearbyStars(It.IsAny<int>(), It.IsAny<double>(), It.IsAny<int>()))
            .Returns([]);

        var galaxy = _fixture.Build<Galaxy>()
            .With(g => g.Stars, starCollectionMock.Object)
            .Create();

        _galaxyRepositoryMock.Setup(repo => repo.GetAsync())
            .ReturnsAsync(galaxy);

        var service = new GalaxyService(_galaxyRepositoryMock.Object);

        // Act
        var result = await service.GetStarsNearStarAsync(centerStar, 5);

        // Assert
        await Assert.That(result).IsEmpty();
    }

    #endregion

    #region Конструктор

    [Test]
    public async Task Constructor_NullRepository_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.That(() => new GalaxyService(null!))
            .ThrowsExactly<ArgumentNullException>();
    }

    #endregion
}