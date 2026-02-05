using System.Security.Cryptography;

namespace ControlWorkApp.Services;

public static class SessionAuth
{
    public static string GenerateSessionId(int byteLength = 32)
    {
        byte[] bytes = new byte[byteLength];
        RandomNumberGenerator.Fill(bytes);
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
