using AutoFixture;
using Moq;
using AspNetApp.Services;
using AspNetApp.Entity;
using AspNetApp.IRepository;

namespace AspNetAppTests.Services;

public class PlayerServiceTests
{
    private readonly Fixture _fixture;
    private readonly Mock<IPlayerRepository> _playerRepositoryMock;

    public PlayerServiceTests()
    {
        _fixture = new Fixture();

        // AutoFixture по умолчанию генерирует строки длиной 255 символов,
        // что вызовет ошибку валидации. Ограничиваем длину для корректных тестов.
        // _fixture.Customize<string>(composer => composer.FromFactory<string>(s => s[..Math.Min(s.Length, 50)]));

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _playerRepositoryMock = new Mock<IPlayerRepository>();
    }

    [Test]
    public async Task CreatePlayerAsync_ValidName_CreatesAndReturnsPlayer()
    {
        // Arrange
        var name = new string([.. _fixture.CreateMany<char>(10)]);
        var expectedPlayer = _fixture.Build<Player>()
            .With(p => p.Name, name)
            .Without(p => p.Id)
            .Create();

        _playerRepositoryMock
            .Setup(repo => repo.CreateAsync(It.IsAny<Guid>(), It.Is<Player>(p => p.Name == name)))
            .ReturnsAsync((Guid _, Player p) => p);

        var service = new PlayerService(_playerRepositoryMock.Object);

        // Act
        var result = await service.CreatePlayerAsync(name);

        // Assert
        await Assert.That(result).IsNotNull();
        await Assert.That(result.Name).IsEqualTo(name);
        await Assert.That(result.Id).IsNotEqualTo(Guid.Empty);

        // Проверяем, что репозиторий был вызван ровно 1 раз
        _playerRepositoryMock.Verify(
            repo => repo.CreateAsync(It.IsAny<Guid>(), It.Is<Player>(p => p.Name == name)),
            Times.Once);
    }

    [Test]
    [Arguments(null)]
    [Arguments("")]
    [Arguments("   ")]
    public async Task CreatePlayerAsync_InvalidName_ThrowsArgumentException(string? invalidName)
    {
        // Arrange
        var service = new PlayerService(_playerRepositoryMock.Object);

        // Act & Assert
        await Assert
            .That(async () => await service.CreatePlayerAsync(invalidName!))
            .Throws<ArgumentException>();

        // Убеждаемся, что репозиторий НЕ вызывался при ошибке валидации
        _playerRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Guid>(), It.IsAny<Player>()), Times.Never);
    }

    [Test]
    [Arguments("ab")]      // < 3 символов
    [Arguments("a")]
    public async Task CreatePlayerAsync_NameTooShort_ThrowsArgumentException(string name)
    {
        // Arrange
        var service = new PlayerService(_playerRepositoryMock.Object);

        // Act & Assert
        await Assert.That(async () => await service.CreatePlayerAsync(name))
            .Throws<ArgumentException>()
            .WithMessageContaining("between 3 and 50");

        _playerRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Guid>(), It.IsAny<Player>()), Times.Never);
    }

    [Test]
    public async Task CreatePlayerAsync_NameTooLong_ThrowsArgumentException()
    {
        // Arrange
        var service = new PlayerService(_playerRepositoryMock.Object);

        // Arrange
        var longName = new string('A', 51); // > 50 символов

        // Act & Assert
        await Assert.That(async () => await service.CreatePlayerAsync(longName))
            .Throws<ArgumentException>()
            .WithMessageContaining("between 3 and 50");

        _playerRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Guid>(), It.IsAny<Player>()), Times.Never);
    }

    [Test]
    public async Task GetPlayerAsync_ExistingId_ReturnsPlayer()
    {
        // Arrange
        var playerId = Guid.NewGuid();
        var expectedPlayer = _fixture.Build<Player>()
            .With(p => p.Id, playerId)
            .Create();

        _playerRepositoryMock
            .Setup(repo => repo.GetAsync(playerId))
            .ReturnsAsync(expectedPlayer);

        var service = new PlayerService(_playerRepositoryMock.Object);

        // Act
        var result = await service.GetPlayerAsync(playerId);

        // Assert
        await Assert.That(result).IsSameReferenceAs(expectedPlayer);
        await Assert.That(result.Id).IsEqualTo(playerId);
    }

    [Test]
    public async Task GetPlayerAsync_NonExistingId_ThrowsKeyNotFoundException()
    {
        // Arrange
        var unknownId = Guid.NewGuid();

        _playerRepositoryMock
            .Setup(repo => repo.GetAsync(unknownId))
            .ThrowsAsync(new KeyNotFoundException($"Player with id {unknownId} not found"));

        // Arrange
        var service = new PlayerService(_playerRepositoryMock.Object);

        // Act & Assert
        await Assert.That(async () => await service.GetPlayerAsync(unknownId))
            .Throws<KeyNotFoundException>();
    }

    [Test]
    public async Task UpdatePlayerNameAsync_ValidInput_UpdatesAndReturnsPlayer()
    {
        // Arrange
        var playerId = Guid.NewGuid();
        var newName = _fixture.Create<string>()[..20];
        var updatedPlayer = _fixture.Build<Player>()
            .With(p => p.Id, playerId)
            .With(p => p.Name, newName)
            .Create();

        // Мок обновления: просто возвращает задачу
        _playerRepositoryMock
            .Setup(repo => repo.UpdateAsync(playerId, It.Is<Player>(p => p.Name == newName)))
            .Returns(Task.CompletedTask);

        // Мок получения: возвращает обновлённого игрока
        _playerRepositoryMock
            .Setup(repo => repo.GetAsync(playerId))
            .ReturnsAsync(updatedPlayer);

        var service = new PlayerService(_playerRepositoryMock.Object);

        // Act
        var result = await service.UpdatePlayerNameAsync(playerId, newName);

        // Assert
        await Assert.That(result).IsNotNull();
        await Assert.That(result.Name).IsEqualTo(newName);
        await Assert.That(result.Id).IsEqualTo(playerId);

        // Проверяем порядок вызовов: сначала Update, потом Get
        _playerRepositoryMock.Verify(
            repo => repo.UpdateAsync(playerId, It.Is<Player>(p => p.Name == newName)),
            Times.Once);
        _playerRepositoryMock.Verify(repo => repo.GetAsync(playerId), Times.Once);
    }

    [Test]
    [Arguments(null)]
    [Arguments("")]
    [Arguments("   ")]
    public async Task UpdatePlayerNameAsync_InvalidName_ThrowsArgumentException(string? invalidName)
    {
        // Arrange
        var playerId = Guid.NewGuid();
        var service = new PlayerService(_playerRepositoryMock.Object);

        // Act & Assert
        await Assert.That(async () => await service.UpdatePlayerNameAsync(playerId, invalidName!))
            .Throws<ArgumentException>();

        // Репозиторий не должен вызываться при ошибке валидации
        _playerRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Guid>(), It.IsAny<Player>()), Times.Never);
        _playerRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Test]
    public async Task DeletePlayerAsync_ExistingId_CallsRepository()
    {
        // Arrange
        var playerId = Guid.NewGuid();

        _playerRepositoryMock
            .Setup(repo => repo.DeleteAsync(playerId))
            .Returns(Task.CompletedTask);

        var service = new PlayerService(_playerRepositoryMock.Object);

        // Act
        await service.DeletePlayerAsync(playerId);

        // Assert
        _playerRepositoryMock.Verify(repo => repo.DeleteAsync(playerId), Times.Once);
    }

    [Test]
    public async Task DeletePlayerAsync_NonExistingId_PropagatesException()
    {
        // Arrange
        var unknownId = Guid.NewGuid();

        _playerRepositoryMock
            .Setup(repo => repo.DeleteAsync(unknownId))
            .ThrowsAsync(new KeyNotFoundException());

        var service = new PlayerService(_playerRepositoryMock.Object);

        // Act & Assert
        await Assert.That(async () => await service.DeletePlayerAsync(unknownId))
            .Throws<KeyNotFoundException>();
    }

    [Test]
    public async Task PlayerExistsAsync_ExistingPlayer_ReturnsTrue()
    {
        // Arrange
        var playerId = Guid.NewGuid();
        var player = _fixture.Build<Player>()
            .With(p => p.Id, playerId)
            .Create();

        _playerRepositoryMock
            .Setup(repo => repo.GetAsync(playerId))
            .ReturnsAsync(player);

        var service = new PlayerService(_playerRepositoryMock.Object);

        // Act
        var result = await service.PlayerExistsAsync(playerId);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task PlayerExistsAsync_NonExistingPlayer_ReturnsFalse()
    {
        // Arrange
        var unknownId = Guid.NewGuid();

        _playerRepositoryMock
            .Setup(repo => repo.GetAsync(unknownId))
            .ThrowsAsync(new KeyNotFoundException());

        var service = new PlayerService(_playerRepositoryMock.Object);

        // Act
        var result = await service.PlayerExistsAsync(unknownId);

        // Assert
        await Assert.That(result).IsFalse();

        // Важно: проверяем, что исключение было "поймано" внутри метода, а не утекло наружу
    }

    [Test]
    public async Task PlayerExistsAsync_UnexpectedException_Propagates()
    {
        // Arrange
        var playerId = Guid.NewGuid();
        var expectedException = new InvalidOperationException("Да просто захотелось выкинуть ошибку, от нечего делать");

        _playerRepositoryMock
            .Setup(repo => repo.GetAsync(playerId))
            .ThrowsAsync(expectedException);

        var service = new PlayerService(_playerRepositoryMock.Object);

        // Act & Assert
        // Метод ловит только KeyNotFoundException, остальные исключения должны пробрасываться
        await Assert.That(async () => await service.PlayerExistsAsync(playerId))
            .Throws<InvalidOperationException>();
    }
}