using AspNetApp.Auth;
using AspNetApp.Entity;
using AspNetApp.IRepository;
using AspNetApp.Services;
using AutoFixture;
using Moq;

namespace AspNetAppTests.Services;

public class AuthServiceTests
{
    private readonly Fixture _fixture;
    private readonly Mock<IPlayerRepository> _playerRepositoryMock;
    private readonly AuthService _authService;
    private const string LongString = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";

    public AuthServiceTests()
    {
        _fixture = new Fixture();

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _playerRepositoryMock = new Mock<IPlayerRepository>();
        _authService = new AuthService(_playerRepositoryMock.Object);
    }

    #region 🔐 LogInAsync Tests

    [Test]
    [DisplayName("LogInAsync: выбрасывает исключение при пустом login")]
    public async Task LogInAsync_EmptyLogin_ThrowsArgumentException()
    {
        // Act & Assert
        await Assert.That(async () => await _authService.LogInAsync("", "password123"))
            .Throws<ArgumentException>();
    }

    [Test]
    [DisplayName("LogInAsync: выбрасывает исключение при пустом password")]
    public async Task LogInAsync_EmptyPassword_ThrowsArgumentException()
    {
        // Act & Assert
        await Assert.That(async () => await _authService.LogInAsync("validlogin", ""))
            .Throws<ArgumentException>();
    }

    [Test]
    [DisplayName("LogInAsync: выбрасывает исключение при null login")]
    public async Task LogInAsync_NullLogin_ThrowsArgumentException()
    {
        // Act & Assert
        await Assert.That(async () => await _authService.LogInAsync(null!, "password123"))
            .Throws<ArgumentException>();
    }

    [Test]
    [Arguments("ab", "password123")]      // слишком короткий
    [Arguments(LongString, "pass")]  // слишком длинный
    [DisplayName("LogInAsync: выбрасывает исключение при неверной длине login")]
    public async Task LogInAsync_InvalidLoginLength_ThrowsArgumentException(string login, string password)
    {
        // Act & Assert
        await Assert.That(async () => await _authService.LogInAsync(login, password))
            .Throws<ArgumentException>()
            .WithMessageContaining("between 3 and 50");
    }

    [Test]
    [DisplayName("LogInAsync: возвращает null при несуществующем пользователе")]
    public async Task LogInAsync_UserNotFound_ReturnsNull()
    {
        // Arrange
        _playerRepositoryMock
            .Setup(r => r.GetByLoginAsync("nonexistent"))
            .ReturnsAsync((Player?)null);

        // Act
        var result = await _authService.LogInAsync("nonexistent", "anyPassword");

        // Assert
        await Assert.That(result).IsNull();
        _playerRepositoryMock.Verify(r => r.GetByLoginAsync("nonexistent"), Times.Once);
    }

    [Test]
    [DisplayName("LogInAsync: возвращает null при неверном пароле")]
    public async Task LogInAsync_WrongPassword_ReturnsNull()
    {
        // Arrange
        const string password = "some_hash";
        var user = _fixture.Build<Player>()
            .With(p => p.PasswordHash, PasswordHashBuilder.GetHash(password)).Create();
        
        _playerRepositoryMock
            .Setup(r => r.GetByLoginAsync(user.Login))
            .ReturnsAsync(user);

        // Act
        var result = await _authService.LogInAsync(user.Login, "wrongPassword");

        // Assert
        await Assert.That(result).IsNull();
    }

    [Test]
    [DisplayName("LogInAsync: возвращает пользователя при успешном входе")]
    public async Task LogInAsync_ValidCredentials_ReturnsPlayer()
    {
        // Arrange
        const string password = "SecureP@ss123";
        var user = _fixture.Build<Player>()
            .With(p => p.PasswordHash, PasswordHashBuilder.GetHash(password)).Create();
        
        _playerRepositoryMock
            .Setup(r => r.GetByLoginAsync(user.Login))
            .ReturnsAsync(user);

        // Act
        var result = await _authService.LogInAsync(user.Login, password);

        // Assert
        await Assert.That(result).IsNotNull();
        await Assert.That(result!.Id).IsEqualTo(user.Id);
        await Assert.That(result.Login).IsEqualTo(user.Login);
        await Assert.That(result.Name).IsEqualTo(user.Name);
        
        _playerRepositoryMock.Verify(r => r.GetByLoginAsync(user.Login), Times.Once);
        // Убеждаемся, что CreateAsync не вызывался
        _playerRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<Guid>(), It.IsAny<Player>()), Times.Never);
    }

    #endregion

    #region 👤 SignInNewPlayerAsync Tests

    [Test]
    [DisplayName("SignInNewPlayerAsync: выбрасывает исключение при пустом name")]
    public async Task SignInNewPlayerAsync_EmptyName_ThrowsArgumentException()
    {
        // Act & Assert
        await Assert.That(async () => await _authService.SignInNewPlayerAsync("", "login", "password"))
            .Throws<ArgumentException>();
    }

    [Test]
    [DisplayName("SignInNewPlayerAsync: выбрасывает исключение при пустом login")]
    public async Task SignInNewPlayerAsync_EmptyLogin_ThrowsArgumentException()
    {
        // Act & Assert
        await Assert.That(async () => await _authService.SignInNewPlayerAsync("Name", "", "password"))
            .Throws<ArgumentException>();
    }

    [Test]
    [DisplayName("SignInNewPlayerAsync: выбрасывает исключение при пустом password")]
    public async Task SignInNewPlayerAsync_EmptyPassword_ThrowsArgumentException()
    {
        // Act & Assert
        await Assert.That(async () => await _authService.SignInNewPlayerAsync("Name", "login", ""))
            .Throws<ArgumentException>();
    }

    [Test]
    [Arguments("ab", "login123", "pass")]           // name слишком короткий
    [Arguments(LongString, "login", "pass")]  // name слишком длинный
    [DisplayName("SignInNewPlayerAsync: выбрасывает исключение при неверной длине name")]
    public async Task SignInNewPlayerAsync_InvalidNameLength_ThrowsArgumentException(
        string name, string login, string password)
    {
        // Act & Assert
        await Assert.That(async () => await _authService.SignInNewPlayerAsync(name, login, password))
            .Throws<ArgumentException>()
            .WithMessageContaining("Player name must be between 3 and 50");
    }

    [Test]
    [Arguments("ValidName", "ab", "pass")]          // login слишком короткий
    [Arguments("ValidName", LongString, "pass")]  // login слишком длинный
    [DisplayName("SignInNewPlayerAsync: выбрасывает исключение при неверной длине login")]
    public async Task SignInNewPlayerAsync_InvalidLoginLength_ThrowsArgumentException(
        string name, string login, string password)
    {
        // Act & Assert
        await Assert.That(async () => await _authService.SignInNewPlayerAsync(name, login, password))
            .Throws<ArgumentException>()
            .WithMessageContaining("Player login must be between 3 and 50");
    }

    [Test]
    [DisplayName("SignInNewPlayerAsync: создаёт пользователя с корректными данными")]
    public async Task SignInNewPlayerAsync_ValidData_CreatesPlayerWithHashedPassword()
    {
        // Arrange
        const string name = "Test Player";
        const string login = "test_player";
        const string password = "SecureP@ss123";
        const string expectedRole = "User";
        
        Player? capturedPlayer = null;
        Guid capturedGuid = Guid.Empty;
        
        _playerRepositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<Guid>(), It.IsAny<Player>()))
            .Callback<Guid, Player>((guid, player) =>
            {
                capturedGuid = guid;
                capturedPlayer = player;
            })
            .ReturnsAsync((Guid id, Player player) => player); // возвращаем того же игрока

        // Act
        var result = await _authService.SignInNewPlayerAsync(name, login, password);

        // Assert
        await Assert.That(result).IsNotNull();
        await Assert.That(result!.Name).IsEqualTo(name);
        await Assert.That(result.Login).IsEqualTo(login);
        await Assert.That(result.Role).IsEqualTo(expectedRole);
        await Assert.That(result.Id).IsNotEqualTo(Guid.Empty);
        
        // Проверяем, что пароль захеширован, а не хранится в открытом виде
        await Assert.That(result.PasswordHash).IsNotEqualTo(password);
        await Assert.That(PasswordHashBuilder.Validate(result.PasswordHash, password)).IsTrue();
        
        // Проверяем, что репозиторий был вызван с корректными параметрами
        await Assert.That(capturedPlayer).IsNotNull();
        await Assert.That(capturedPlayer!.Id).IsEqualTo(result.Id);
        await Assert.That(capturedGuid).IsEqualTo(result.Id); // GUID передаётся и как параметр, и в объекте
        
        _playerRepositoryMock.Verify(
            r => r.CreateAsync(It.IsAny<Guid>(), It.Is<Player>(p => 
                p.Name == name && 
                p.Login == login && 
                p.Role == expectedRole && 
                PasswordHashBuilder.Validate(p.PasswordHash, password))),
            Times.Once);
    }

    [Test]
    [DisplayName("SignInNewPlayerAsync: присваивает роль 'User' по умолчанию")]
    public async Task SignInNewPlayerAsync_AssignsDefaultUserRole()
    {
        // Arrange
        _playerRepositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<Guid>(), It.IsAny<Player>()))
            .ReturnsAsync((Guid id, Player p) => p);

        // Act
        var result = await _authService.SignInNewPlayerAsync("Name", "login", "password");

        // Assert
        await Assert.That(result!.Role).IsEqualTo("User");
    }

    #endregion

    #region 🔄 Интеграционные сценарии (опционально)

    [Test]
    [DisplayName("Интеграция: регистрация + вход с теми же данными")]
    public async Task RegisterThenLogin_WithSameCredentials_Success()
    {
        // Arrange
        const string name = "Integration Test";
        const string login = "test_login";
        const string password = "TestP@ss123!";
        
        Player? createdPlayer = null;
        
        _playerRepositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<Guid>(), It.IsAny<Player>()))
            .Callback<Guid, Player>((_, p) => createdPlayer = p)
            .ReturnsAsync((Guid id, Player p) => p);
        
        _playerRepositoryMock
            .Setup(r => r.GetByLoginAsync(login))
            .ReturnsAsync(() => createdPlayer); // возвращаем созданного пользователя

        // Act 1: Регистрация
        var registered = await _authService.SignInNewPlayerAsync(name, login, password);
        
        // Act 2: Вход
        var loggedIn = await _authService.LogInAsync(login, password);

        // Assert
        await Assert.That(registered).IsNotNull();
        await Assert.That(loggedIn).IsNotNull();
        await Assert.That(loggedIn!.Id).IsEqualTo(registered!.Id);
        await Assert.That(loggedIn.Login).IsEqualTo(registered.Login);
        
        // Проверяем порядок вызовов репозитория
        _playerRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<Guid>(), It.IsAny<Player>()), Times.Once);
        _playerRepositoryMock.Verify(r => r.GetByLoginAsync(login), Times.Once);
    }

    #endregion
}