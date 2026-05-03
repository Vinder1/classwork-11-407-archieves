using AutoFixture;
using Moq;
using AspNetApp.Services;
using AspNetApp.Entity;
using AspNetApp.IRepository;
using AspNetApp.IServices;
using AspNetApp.DomainUtility;

namespace AspNetAppTests.Services;

#region CreateSpaceshipAsync Tests

public class SpaceshipServiceTests
{
    protected readonly Fixture _fixture;
    protected readonly Mock<ISpaceshipRepository> _spaceshipRepositoryMock;
    protected readonly Mock<IPlayerService> _playerServiceMock;
    protected readonly Mock<IGalaxyService> _galaxyServiceMock;
    protected readonly SpaceshipService _service;

    public SpaceshipServiceTests()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _spaceshipRepositoryMock = new Mock<ISpaceshipRepository>();
        _playerServiceMock = new Mock<IPlayerService>();
        _galaxyServiceMock = new Mock<IGalaxyService>();

        _service = new SpaceshipService(
            _spaceshipRepositoryMock.Object,
            _playerServiceMock.Object,
            _galaxyServiceMock.Object);
    }

    [Test]
    public async Task CreateSpaceshipAsync_ValidInput_CreatesAndReturnsSpaceship()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var galaxyVersion = 5;
        var expectedSpaceship = CreateTestSpaceship(ownerId, galaxyVersion);

        // Игрок существует
        _playerServiceMock
            .Setup(ps => ps.PlayerExistsAsync(ownerId))
            .ReturnsAsync(true);

        // Активного корабля нет
        _spaceshipRepositoryMock
            .Setup(repo => repo.GetAsync(ownerId))
            .ThrowsAsync(new KeyNotFoundException());

        // Создание корабля
        _spaceshipRepositoryMock
            .Setup(repo => repo.CreateAsync(ownerId, It.IsAny<Spaceship>()))
            .ReturnsAsync(expectedSpaceship);

        // Act
        var result = await _service.CreateSpaceshipAsync(ownerId, galaxyVersion);

        // Assert
        await Assert.That(result).IsNotNull();
        await Assert.That(result.OwnerId).IsEqualTo(ownerId);
        await Assert.That(result.GalaxyVersion).IsEqualTo(galaxyVersion);

        _playerServiceMock.Verify(ps => ps.PlayerExistsAsync(ownerId), Times.Once);
        _spaceshipRepositoryMock.Verify(repo => repo.CreateAsync(ownerId, It.IsAny<Spaceship>()), Times.Once);
    }

    [Test]
    public async Task CreateSpaceshipAsync_PlayerDoesNotExist_ThrowsInvalidOperationException()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var galaxyVersion = 5;

        _playerServiceMock
            .Setup(ps => ps.PlayerExistsAsync(ownerId))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.That(async () => await _service.CreateSpaceshipAsync(ownerId, galaxyVersion))
            .Throws<InvalidOperationException>()
            .WithMessageContaining($"Player {ownerId} does not exist");

        // Репозиторий не должен вызываться
        _spaceshipRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Guid>(), It.IsAny<Spaceship>()), Times.Never);
    }

    [Test]
    public async Task CreateSpaceshipAsync_AlreadyHasActiveSpaceship_ThrowsInvalidOperationException()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var galaxyVersion = 5;
        var existingShip = CreateTestSpaceship(ownerId, galaxyVersion); // Версия >= текущей

        _playerServiceMock
            .Setup(ps => ps.PlayerExistsAsync(ownerId))
            .ReturnsAsync(true);

        _spaceshipRepositoryMock
            .Setup(repo => repo.GetAsync(ownerId))
            .ReturnsAsync(existingShip);

        // Act & Assert
        await Assert.That(async () => await _service.CreateSpaceshipAsync(ownerId, galaxyVersion))
            .Throws<InvalidOperationException>()
            .WithMessageContaining($"Player {ownerId} already has one");

        _spaceshipRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Guid>(), It.IsAny<Spaceship>()), Times.Never);
    }

    [Test]
    public async Task CreateSpaceshipAsync_HasInactiveSpaceship_AllowsCreation()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var currentVersion = 5;
        var oldVersion = 3; // < currentVersion
        var oldShip = CreateTestSpaceship(ownerId, oldVersion);

        _playerServiceMock
            .Setup(ps => ps.PlayerExistsAsync(ownerId))
            .ReturnsAsync(true);

        // Есть корабль, но он устаревший (Inactive)
        // на всякий случай, на самом деле не должно играть роли, перезаписью займётся репозиторий
        _spaceshipRepositoryMock
            .Setup(repo => repo.GetAsync(ownerId))
            .ReturnsAsync(oldShip);

        var newShip = CreateTestSpaceship(ownerId, currentVersion);
        _spaceshipRepositoryMock
            .Setup(repo => repo.CreateAsync(ownerId, It.IsAny<Spaceship>()))
            .ReturnsAsync(newShip);

        // Act
        var result = await _service.CreateSpaceshipAsync(ownerId, currentVersion);

        // Assert
        await Assert.That(result).IsNotNull();
        await Assert.That(result.GalaxyVersion).IsEqualTo(currentVersion);
    }

    #endregion

    #region GetSpaceshipAsync Tests

    [Test]
    public async Task GetSpaceshipAsync_ExistingOwner_ReturnsSpaceship()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var expectedShip = CreateTestSpaceship(ownerId);

        _spaceshipRepositoryMock
            .Setup(repo => repo.GetAsync(ownerId))
            .ReturnsAsync(expectedShip);

        // Act
        var result = await _service.GetSpaceshipAsync(ownerId);

        // Assert
        await Assert.That(result).IsNotNull();
        await Assert.That(result.OwnerId).IsEqualTo(ownerId);
    }

    [Test]
    public async Task GetSpaceshipAsync_NonExistingOwner_PropagatesKeyNotFoundException()
    {
        // Arrange
        var ownerId = Guid.NewGuid();

        _spaceshipRepositoryMock
            .Setup(repo => repo.GetAsync(ownerId))
            .ThrowsAsync(new KeyNotFoundException());

        // Act & Assert
        await Assert.That(async () => await _service.GetSpaceshipAsync(ownerId))
            .Throws<KeyNotFoundException>();
    }

    #endregion

    #region GetAllSpaceshipsAsync Tests

    [Test]
    public async Task GetAllSpaceshipsAsync_ReturnsAllSpaceships()
    {
        // Arrange
        var ships = _fixture.Build<Spaceship>()
            .CreateMany(5)
            .ToList();

        _spaceshipRepositoryMock
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(ships);

        // Act
        var result = await _service.GetAllSpaceshipsAsync();

        // Assert
        await Assert.That(result).IsNotNull();
        await Assert.That(result).Count().IsEqualTo(5);
    }

    [Test]
    public async Task GetAllSpaceshipsAsync_EmptyRepository_ReturnsEmptyList()
    {
        // Arrange
        _spaceshipRepositoryMock
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync([]);

        // Act
        var result = await _service.GetAllSpaceshipsAsync();

        // Assert
        await Assert.That(result).IsEmpty();
    }

    #endregion

    #region MoveSpaceshipAsync Tests

    [Test]
    public async Task MoveSpaceshipAsync_ValidInput_MovesAndReturnsUpdatedShip()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        int newRadius = 50;
        int newAngle = 7000; // > 2π, должно нормализоваться
        var normalizedAngle = Angle.NormalizeMilliradians(newAngle);
        var destinationStar = new Star { AngleMilliradians=newAngle, Radius=newRadius };

        var updatedShip = CreateTestSpaceship(ownerId);

        _spaceshipRepositoryMock
            .Setup(repo => repo.MoveAsync(ownerId, newRadius, normalizedAngle))
            .Returns(Task.CompletedTask);

        _spaceshipRepositoryMock
            .Setup(repo => repo.GetAsync(ownerId))
            .ReturnsAsync(updatedShip);

        _galaxyServiceMock
            .Setup(repo => repo.StarExistsAsync(newRadius, normalizedAngle))
            .ReturnsAsync(true);

        // Act
        var result = await _service.MoveSpaceshipAsync(ownerId, destinationStar);

        // Assert
        await Assert.That(result).IsNotNull();

        // Проверяем, что репозиторий получил нормализованный угол
        _spaceshipRepositoryMock.Verify(
            repo => repo.MoveAsync(ownerId, newRadius, normalizedAngle),
            Times.Once);
    }
    
    [Test]
    public async Task MoveSpaceshipAsync_StarDoesNotExist_ThrowsException()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        int newRadius = 50;
        int newAngle = 7000; // > 2π, должно нормализоваться
        var normalizedAngle = Angle.NormalizeMilliradians(newAngle);
        var destinationStar = new Star { AngleMilliradians=newAngle, Radius=newRadius };

        var updatedShip = CreateTestSpaceship(ownerId);

        _spaceshipRepositoryMock
            .Setup(repo => repo.MoveAsync(ownerId, newRadius, normalizedAngle))
            .Returns(Task.CompletedTask);

        _spaceshipRepositoryMock
            .Setup(repo => repo.GetAsync(ownerId))
            .ReturnsAsync(updatedShip);

        _galaxyServiceMock
            .Setup(repo => repo.StarExistsAsync(newRadius, newAngle))
            .ReturnsAsync(false);

        // Act &  Assert
        await Assert.That(async () => await _service.MoveSpaceshipAsync(ownerId, destinationStar))
            .ThrowsException()
            .WithMessageContaining("Star not found");

        // Проверяем, что сервис вообще вызывался
        _galaxyServiceMock.Verify(
            repo => repo.StarExistsAsync(newRadius, normalizedAngle),
            Times.Once);
    }

    [Test]
    [Arguments(-1)]
    [Arguments(-100)]
    [Arguments(-999)]
    public async Task MoveSpaceshipAsync_NegativeRadius_ThrowsArgumentException(int invalidRadius)
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        int angle = (int)(Math.PI*1000);
        var destinationStar = new Star { AngleMilliradians=angle, Radius=invalidRadius };

        // Act & Assert
        await Assert.That(async () => await _service.MoveSpaceshipAsync(ownerId, destinationStar))
            .Throws<ArgumentException>()
            .WithMessageContaining("Radius cannot be negative");

        _spaceshipRepositoryMock.Verify(repo => repo.MoveAsync(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
    }

    [Test]
    public async Task MoveSpaceshipAsync_AngleNormalization_WorksWithNegativeAngle()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        int radius = 10;
        var negativeAngle = Angle.RadiansToMilliradians(-Math.PI / 2); // -90 градусов
        var expectedAngle = Angle.NormalizeMilliradians(negativeAngle); // Должно стать 3π/2
        var destinationStar = new Star { AngleMilliradians=expectedAngle, Radius=radius };

        var updatedShip = CreateTestSpaceship(ownerId);

        _spaceshipRepositoryMock
            .Setup(repo => repo.MoveAsync(ownerId, radius, expectedAngle))
            .Returns(Task.CompletedTask);

        _spaceshipRepositoryMock
            .Setup(repo => repo.GetAsync(ownerId))
            .ReturnsAsync(updatedShip);

        _galaxyServiceMock
            .Setup(repo => repo.StarExistsAsync(radius, expectedAngle))
            .ReturnsAsync(true);

        // Act
        var result = await _service.MoveSpaceshipAsync(ownerId, destinationStar);

        // Assert
        await Assert.That(result).IsNotNull();
        _spaceshipRepositoryMock.Verify(
            repo => repo.MoveAsync(ownerId, radius, expectedAngle),
            Times.Once);
    }

    #endregion

    #region CleanupInactiveSpaceshipsAsync Tests

    [Test]
    public async Task CleanupInactiveSpaceshipsAsync_RemovesCorrectCount()
    {
        // Arrange
        var currentVersion = 10;
        var allShips = _fixture.Build<Spaceship>().CreateMany(10).ToList();

        _spaceshipRepositoryMock
            .SetupSequence(repo => repo.GetAllAsync())
            .ReturnsAsync(allShips)       // До очистки (10 кораблей)
            .ReturnsAsync(allShips.Take(7).ToList()); // После очистки (7 кораблей)

        _spaceshipRepositoryMock
            .Setup(repo => repo.DeleteInactiveAsync(currentVersion))
            .Returns(Task.CompletedTask);

        // Act
        var removedCount = await _service.CleanupInactiveSpaceshipsAsync(currentVersion);

        // Assert
        await Assert.That(removedCount).IsEqualTo(3); // 10 - 7 = 3
        _spaceshipRepositoryMock.Verify(repo => repo.DeleteInactiveAsync(currentVersion), Times.Once);
    }

    [Test]
    public async Task CleanupInactiveSpaceshipsAsync_NoInactiveShips_ReturnsZero()
    {
        // Arrange
        var currentVersion = 10;
        var ships = _fixture.Build<Spaceship>().CreateMany(5).ToList();

        _spaceshipRepositoryMock
            .SetupSequence(repo => repo.GetAllAsync())
            .ReturnsAsync(ships)
            .ReturnsAsync(ships); // Количество не изменилось

        _spaceshipRepositoryMock
            .Setup(repo => repo.DeleteInactiveAsync(currentVersion))
            .Returns(Task.CompletedTask);

        // Act
        var removedCount = await _service.CleanupInactiveSpaceshipsAsync(currentVersion);

        // Assert
        await Assert.That(removedCount).IsEqualTo(0);
        _spaceshipRepositoryMock.Verify(repo => repo.DeleteInactiveAsync(currentVersion), Times.Once);
    }

    #endregion

    #region HasActiveSpaceshipAsync Tests

    [Test]
    [Arguments(5, 5)] // Версия == текущей (Active)
    [Arguments(10, 5)] // Версия > текущей (Active)
    public async Task HasActiveSpaceshipAsync_ActiveShip_ReturnsTrue(int shipVersion, int currentVersion)
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var ship = CreateTestSpaceship(ownerId, shipVersion);

        _spaceshipRepositoryMock
            .Setup(repo => repo.GetAsync(ownerId))
            .ReturnsAsync(ship);

        // Act
        var result = await _service.HasActiveSpaceshipAsync(ownerId, currentVersion);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task HasActiveSpaceshipAsync_InactiveShip_ReturnsFalse()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var ship = CreateTestSpaceship(ownerId, version: 3);
        int currentVersion = 5;

        _spaceshipRepositoryMock
            .Setup(repo => repo.GetAsync(ownerId))
            .ReturnsAsync(ship);

        // Act
        var result = await _service.HasActiveSpaceshipAsync(ownerId, currentVersion);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task HasActiveSpaceshipAsync_NoShip_ReturnsFalse()
    {
        // Arrange
        var ownerId = Guid.NewGuid();

        _spaceshipRepositoryMock
            .Setup(repo => repo.GetAsync(ownerId))
            .ThrowsAsync(new KeyNotFoundException());

        // Act
        var result = await _service.HasActiveSpaceshipAsync(ownerId, 5);

        // Assert
        await Assert.That(result).IsFalse();
    }
    #endregion

    private Spaceship CreateTestSpaceship(Guid? ownerId = null, int? version = null)
    {
        return _fixture.Build<Spaceship>()
            .With(s => s.OwnerId, ownerId ?? Guid.NewGuid())
            .With(s => s.GalaxyVersion, version ?? _fixture.Create<int>())
            .Without(s => s.Owner)
            .Create();
    }
}

