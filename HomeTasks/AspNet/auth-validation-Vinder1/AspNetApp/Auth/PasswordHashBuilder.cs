using System.Security.Cryptography;

namespace AspNetApp.Auth;

/// <summary>
/// 
/// </summary>
public static class PasswordHashBuilder
{
    private const int KeySize = 32;
    private const int SaltSize = 16;
    private const int Iterations = 10000;
    private static readonly HashAlgorithmName HashAlgorithmName = HashAlgorithmName.SHA256;
    private const char SaltDelimiter = ';';

    /// <summary>
    /// 
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    public static string GetHash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName, KeySize);
        return string.Join(SaltDelimiter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="passwordHash"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public static bool Validate(string passwordHash, string password)
    {
        var passwordElements = passwordHash.Split(SaltDelimiter);
        var salt = Convert.FromBase64String(passwordElements[0]);
        var hash = Convert.FromBase64String(passwordElements[1]);
        var hashInput = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName, KeySize);
        return CryptographicOperations.FixedTimeEquals(hash, hashInput);
    }
}
