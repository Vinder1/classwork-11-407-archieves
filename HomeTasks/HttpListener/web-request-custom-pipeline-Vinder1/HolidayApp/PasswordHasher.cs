using System.Security.Cryptography;

namespace HolidayApp;

/// <summary>
/// Скопировано из SimpleHttpListenerServer (с небольшими переименованиями)
/// </summary>
public static class PasswordHasher
{
    private const int KeySize = 32; //256 бит - 32 байта
    private const int SaltSize = 16;
    private const int Iterations = 10000;
    private static readonly HashAlgorithmName HashAlgorithmName = HashAlgorithmName.SHA256;
    private const char SaltDelimiter = ';';

    public static string GetHash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName, KeySize);
        return string.Join(SaltDelimiter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
    }

    public static bool Validate(string passwordHash, string password)
    {
        var passwordElements = passwordHash.Split(SaltDelimiter);
        var salt = Convert.FromBase64String(passwordElements[0]);
        var hash = Convert.FromBase64String(passwordElements[1]);
        var hashInput = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName, KeySize);
        return CryptographicOperations.FixedTimeEquals(hash, hashInput);
    }
}
