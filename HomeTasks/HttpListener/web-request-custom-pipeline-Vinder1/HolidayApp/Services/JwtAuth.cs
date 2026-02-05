using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Emerald;
using Emerald.Exceptions;
using HolidayApp.Dto;
using HolidayApp.Entities;
using Microsoft.IdentityModel.Tokens;

namespace HolidayApp.Services;

public class JwtAuth
{
    private string SecretKey { get; init; }

    public JwtAuth(Configuration config)
    {
        if (config.TryGetString("SecretKey", out var secretKey))
        {
            SecretKey = secretKey;
        }
        else
        {
            throw new InitializationException("В config.json не найдена строка SecretKey");
        }
    }
    #region Jwt Token
    public string GenerateJwtToken(IAuthDto dto)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim("sub", dto.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("Role", dto.Role)
        };

        var token = new JwtSecurityToken(
            issuer: "localhost:5000",
            audience: "localhost:5000",
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public IAuthDto? ValidateJwtToken(string? token)
    {
        if (token == null)
        {
            return null;
        }

        var tokenHandler = new JwtSecurityTokenHandler();

        tokenHandler.InboundClaimTypeMap.Clear();

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "localhost:5000",
            ValidAudience = "localhost:5000",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey)),
            ClockSkew = TimeSpan.FromMinutes(2) // ⏱ допускаем разницу системных часов
        };

        try
        {
            var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken _);
            var userId = claimsPrincipal.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
            var roleClaim = claimsPrincipal.Claims.First(x => x.Type == "Role").Value;
            return new AuthDto {
                Id = int.Parse(userId),
                Role = roleClaim,
            };
        }
        catch
        {
            return null;
        }
    }

    #endregion
    
    #region Refresh token
    public string GenerateRefreshToken(IAuthDto dto)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim("sub", dto.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("Role", dto.Role)
        };

        var token = new JwtSecurityToken(
            issuer: "localhost:5000",
            audience: "localhost:5000",
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddDays(14),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public IAuthDto? ValidateRefreshToken(string? token)
    {
        if (token == null)
        {
            return null;
        }

        var tokenHandler = new JwtSecurityTokenHandler();

        tokenHandler.InboundClaimTypeMap.Clear();

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "localhost:5000",
            ValidAudience = "localhost:5000",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey)),
            ClockSkew = TimeSpan.FromMinutes(2) // ⏱ допускаем разницу системных часов
        };

        try
        {
            var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken _);
            var userId = claimsPrincipal.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
            var roleClaim = claimsPrincipal.Claims.First(x => x.Type == "Role").Value;
            return new AuthDto {
                Id = int.Parse(userId),
                Role = roleClaim,
            };
        }
        catch
        {
            return null;
        }
    }
    #endregion
}
