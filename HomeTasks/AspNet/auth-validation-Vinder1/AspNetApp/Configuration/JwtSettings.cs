namespace AspNetApp.Configuration;

/// <summary>
/// 
/// </summary>
public class JwtSettings
{
    /// <summary>
    /// 
    /// </summary>
    public string Key { get; set; } = null!;
    /// <summary>
    /// 
    /// </summary>
    public string Issuer { get; set; } = null!;
    /// <summary>
    /// 
    /// </summary>
    public string Audience { get; set; } = null!;
    /// <summary>
    /// 
    /// </summary>
    public int TokenLifetimeMinutes { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int RefreshTokenLifetimeDays { get; set; }
}