using App.Domain.Config;
using App.UseCase.Interfaces.Commands;
using App.UseCase.Interfaces.Repositories;
using Common.Exceptions;
using Common.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace App.UseCase.Command.V1;

public class TokenValidatorCommand : ITokenValidatorCommand
{
    private readonly JwtOptions _jwtOptions;
    private readonly IUserRepository _userRepository;

    public TokenValidatorCommand(
        IOptions<JwtOptions> jwtOptions,
        IUserRepository userRepository)
    {
        _jwtOptions = jwtOptions.Value;
        _userRepository = userRepository;
    }

    public async Task ValidateSignatureAsync(MessageReceivedContext context)
    {
        var path = context.HttpContext.Request.Path;
        if (path.StartsWithSegments("/hubs") && context.Request.Query.TryGetValue("access_token", out var tokenInQuery))
        {
            context.Token = tokenInQuery.FirstOrDefault();
        }
        else if (context.Request.Headers.TryGetValue("Authorization", out var tokenInHeader))
        {
            context.Token = tokenInHeader.FirstOrDefault();
        }

        if (context.Token == null)
        {
            return;
        }

        var (principal, securityToken) = ValidateToken(context.Token.ReplaceRecursive("Bearer "));
        var validated = new TokenValidatedContext(context.HttpContext, context.Scheme, context.Options)
        {
            Principal = principal,
            SecurityToken = securityToken
        };

        await ValidateClaimsAsync(validated);

        if (validated.Result.Succeeded == true)
        {
            context.Principal = principal;
            context.Success();
        }
    }

    private (ClaimsPrincipal, SecurityToken) ValidateToken(string token)
    {
        JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        var signKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SignKey));
        var encKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.EncKey));
        var tokenHandler = new JwtSecurityTokenHandler
        {
            MapInboundClaims = false,
            TokenLifetimeInMinutes = _jwtOptions.AccessTokenLifetimeMinutes
        };
        _jwtOptions.TokenValidationParameters.ClockSkew = TimeSpan.FromMinutes(5);
        _jwtOptions.TokenValidationParameters.IssuerSigningKey = signKey;
        _jwtOptions.TokenValidationParameters.TokenDecryptionKey = encKey;

        var principal = tokenHandler.ValidateToken(token, _jwtOptions.TokenValidationParameters.Clone(), out SecurityToken securityToken);
        return (securityToken == null ? null : principal, securityToken);
    }

    public async Task ValidateClaimsAsync(TokenValidatedContext context)
    {
        if (context?.Principal?.Identity is not ClaimsIdentity identity)
        {
            context.Fail(new ForbiddenException("Unauthorized identity!"));
            return;
        }

        if (!identity.IsAuthenticated || !identity.Claims.Any())
        {
            context.Fail(new ForbiddenException("Not authenticated!"));
            return;
        }

        if (context.SecurityToken is not JwtSecurityToken token || !string.Equals(token.Header?.Typ, "JWT", StringComparison.Ordinal))
        {
            context.Fail(new ForbiddenException("Token is not an access token!"));
            return;
        }

        var issuedAt = long.Parse(identity.FindFirst("iat")?.Value);
        var expiredAt = DateTimeOffset.UtcNow.AddMinutes(-_jwtOptions.AccessTokenLifetimeMinutes).ToUnixTimeSeconds();
        if (expiredAt >= issuedAt)
        {
            context.Fail(new TokenExpiredException());
            return;
        }

        if (string.IsNullOrWhiteSpace(token?.Subject) || !Guid.TryParse(token?.Subject, out var userId) || identity.FindFirst("sub")?.Value == null)
        {
            context.Fail(new ForbiddenException("Invalid subject claim value!"));
            return;
        }

        var user = await _userRepository.FindUserAndTokensAsync(userId);
        if (user == null)
        {
            context.Fail(new ForbiddenException("Invalid subject claim value!"));
            return;
        }

        var accessTokenHash = token?.RawData?.Sha256();
        if (!user.UserTokens.Any(e => e.AccessTokenHash == accessTokenHash))
        {
            context.Fail(new ForbiddenException("Token is not in store!"));
            return;
        }

        context.Success();
    }
}
