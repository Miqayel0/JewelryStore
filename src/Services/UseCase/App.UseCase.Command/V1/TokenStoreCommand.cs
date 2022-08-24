using App.Domain.Config;
using App.Domain.Entities;
using App.UseCase.Interfaces.Commands;
using App.UseCase.Interfaces.Repositories;
using App.UseCase.Models.Auth;
using Common.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace App.UseCase.Command.V1;

public class TokenStoreCommand : ITokenStoreCommand
{
    private readonly JwtOptions _jwtOptions;
    private readonly IUserRepository _userRepository;

    public TokenStoreCommand(
        IOptions<JwtOptions> jwtOptions,
        IUserRepository userRepository)
    {
        _jwtOptions = jwtOptions.Value;
        _userRepository = userRepository;
    }

    public async Task<TokenDto> CreateJwtTokensAsync(User user, string oldRefreshToken = null)
    {
        var now = DateTime.UtcNow;
        var result = new TokenDto
        {
            RefreshToken = Guid.NewGuid().ToString("N"),
            AccessToken = CreateAccessToken(user),
        };
        var userToken = new UserToken
        {
            UserId = user.Id,
            RefreshTokenHash = result.RefreshToken.Sha256(),
            RefreshTokenHashSource = oldRefreshToken?.Sha256(),
            AccessTokenHash = result.AccessToken.Sha256(),
            RefreshTokenExpireDate = now.AddDays(_jwtOptions.RefreshTokenLifetimeDays),
            AccessTokenExpireDate = now.AddMinutes(_jwtOptions.AccessTokenLifetimeMinutes)
        };
        await AddUserTokenAsync(userToken);
        await _userRepository.SaveChangesAsync();
        return result;
    }

    private async Task AddUserTokenAsync(UserToken userToken)
    {
        if (!_jwtOptions.AllowMultipleLoginsForUser)
            await RemoveUserTokenAsync(userToken);
        else
            await DeleteTokensForRefreshTokenSourceAsync(userToken.RefreshTokenHashSource);
        await _userRepository.AddAsync(userToken);
    }

    public async Task RemoveUserTokenAsync(UserToken userToken)
    {
        var userTokens = await _userRepository.FindUserTokensByIdAsync(userToken.UserId);
        if (userTokens.Any())
            _userRepository.RemoveRange(userTokens);
    }

    public async Task DeleteTokensForRefreshTokenSourceAsync(string refreshTokenSource)
    {
        if (!string.IsNullOrWhiteSpace(refreshTokenSource))
        {
            var userTokens = await _userRepository.FindUserTokenByRefreshTokenSourceAsync(refreshTokenSource);
            _userRepository.RemoveRange(userTokens);
        }
    }

    private string CreateAccessToken(User user)
    {
        var nameClaimType = _jwtOptions.TokenValidationParameters.NameClaimType;
        var now = DateTimeOffset.UtcNow;
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
            new Claim(JwtRegisteredClaimNames.Iss, _jwtOptions.Authority),
            new Claim(JwtRegisteredClaimNames.Iat, now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            new Claim(JwtRegisteredClaimNames.Nbf, now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            new Claim(JwtRegisteredClaimNames.Exp, now.AddMinutes(_jwtOptions.AccessTokenLifetimeMinutes).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            // new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(nameClaimType, user.Id.ToString()),
        };

        var encKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.EncKey));
        var enc = new EncryptingCredentials(encKey, SecurityAlgorithms.Aes256KW, SecurityAlgorithms.Aes256CbcHmacSha512);
        var signKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SignKey));
        var sign = new SigningCredentials(signKey, SecurityAlgorithms.HmacSha256Signature);

        return new JwtSecurityTokenHandler().CreateEncodedJwt(
            issuer: _jwtOptions.Authority,
            audience: _jwtOptions.Audience,
            new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme),
            notBefore: now.UtcDateTime,
            expires: now.AddMinutes(_jwtOptions.AccessTokenLifetimeMinutes).UtcDateTime,
            issuedAt: now.UtcDateTime,
            signingCredentials: sign,
            encryptingCredentials: enc);
    }
}
