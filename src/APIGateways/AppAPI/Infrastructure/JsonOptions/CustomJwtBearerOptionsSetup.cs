using App.Domain.Config;
using App.UseCase.Interfaces.Commands;
using Common.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace AppAPI.Infrastructure.JsonOptions;

public class CustomJwtBearerOptionsSetup : IConfigureOptions<JwtBearerOptions>
{
    private readonly IHostEnvironment _env;
    private readonly JwtOptions _jwtOptions;

    public CustomJwtBearerOptionsSetup(IHostEnvironment env, IOptions<JwtOptions> jwtOptions)
    {
        _env = env;
        _jwtOptions = jwtOptions.Value;
        IdentityModelEventSource.ShowPII = _env.IsDevelopment();
    }

    public void Configure(JwtBearerOptions ops)
    {
        JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        var tokenHandler = new JwtSecurityTokenHandler
        {
            MapInboundClaims = false,
            TokenLifetimeInMinutes = _jwtOptions.AccessTokenLifetimeMinutes
        };
        ops.SecurityTokenValidators.Clear();
        ops.SecurityTokenValidators.Add(tokenHandler);
        ops.TokenValidationParameters = _jwtOptions.TokenValidationParameters.Clone();
        ops.Authority = _jwtOptions.Authority;
        ops.Audience = _jwtOptions.Audience;
        ops.RefreshOnIssuerKeyNotFound = _jwtOptions.RefreshOnIssuerKeyNotFound;
        ops.SaveToken = _jwtOptions.SaveToken;
        ops.RequireHttpsMetadata = _jwtOptions.RequireHttpsMetadata;
        ops.IncludeErrorDetails = _env.IsDevelopment();
        ops.ForwardAuthenticate = JwtBearerDefaults.AuthenticationScheme;
        ops.ForwardChallenge = JwtBearerDefaults.AuthenticationScheme;
        ops.ForwardSignIn = JwtBearerDefaults.AuthenticationScheme;
        ops.EventsType = typeof(CustomJwtBearerEvents);
    }
}

public class CustomJwtBearerEvents : JwtBearerEvents
{
    public override Task AuthenticationFailed(AuthenticationFailedContext context)
    {
        context.Fail(new UnauthorizedException());
        return Task.CompletedTask;
    }

    public override Task Challenge(JwtBearerChallengeContext context)
    {
        if (context.Error != null)
        {
            var logger = context.HttpContext.RequestServices.GetService<ILogger<JwtBearerEvents>>();
            logger?.LogError($"OnChallenge: {context.Error}", context.ErrorDescription);
        }

        if (context.AuthenticateFailure is SecurityTokenExpiredException)
        {
            throw new TokenExpiredException();
        }

        return Task.CompletedTask;
    }

    public override Task MessageReceived(MessageReceivedContext context)
    {
        var validator = context.HttpContext.RequestServices.GetService<ITokenValidatorCommand>();
        return validator.ValidateSignatureAsync(context);
    }
}
