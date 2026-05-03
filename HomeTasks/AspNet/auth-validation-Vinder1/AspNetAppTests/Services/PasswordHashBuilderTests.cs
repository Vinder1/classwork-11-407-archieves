using AspNetApp.Auth;

namespace AspNetAppTests.Services;

public class PasswordHashBuilderTests
{
    [Test]
    public async Task GetHash_ReturnsNonEmptyString()
    {
        var hash = PasswordHashBuilder.GetHash("test123");
        await Assert.That(hash).IsNotNull().And.IsNotEmpty();
    }

    [Test]
    public async Task Validate_CorrectPassword_ReturnsTrue()
    {
        const string password = "SecureP@ss!";
        var hash = PasswordHashBuilder.GetHash(password);
        
        await Assert.That(PasswordHashBuilder.Validate(hash, password)).IsTrue();
    }

    [Test]
    public async Task Validate_WrongPassword_ReturnsFalse()
    {
        const string password = "Correct123";
        var hash = PasswordHashBuilder.GetHash(password);
        
        await Assert.That(PasswordHashBuilder.Validate(hash, "Wrong456")).IsFalse();
    }

    [Test]
    public async Task GetHash_DifferentSalts_ForSamePassword()
    {
        // Проверяем, что хеши разные (из-за соли), но валидация работает
        const string password = "SamePassword";
        var hash1 = PasswordHashBuilder.GetHash(password);
        var hash2 = PasswordHashBuilder.GetHash(password);
        
        await Assert.That(hash1).IsNotEqualTo(hash2); // разные соли
        await Assert.That(PasswordHashBuilder.Validate(hash1, password)).IsTrue();
        await Assert.That(PasswordHashBuilder.Validate(hash2, password)).IsTrue();
    }
}