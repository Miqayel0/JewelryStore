using Microsoft.IdentityModel.Tokens;

namespace App.Domain.Config;

public class JwtOptions
{
    public string EncKey { get; set; }
    public string SignKey { get; set; }
    public int AccessTokenLifetimeMinutes { get; set; }
    public int RefreshTokenLifetimeDays { get; set; }
    public bool AllowMultipleLoginsForUser { get; set; }
    public bool RequireHttpsMetadata { get; set; }
    public string Authority { get; set; }
    public string Audience { get; set; }
    public bool RefreshOnIssuerKeyNotFound { get; set; } = true;
    public bool SaveToken { get; set; } = false;

    public TokenValidationParameters TokenValidationParameters { get; set; }
}
