using App.Domain.Config;
using App.UseCase.Interfaces.Commands;
using Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace App.UseCase.Command.V1;

public class UserSessionCommand : IUserSessionCommand
{
    private readonly JwtOptions _jwtOptions;
    public IHttpContextAccessor HttpContextAccessor { get; }
    public ClaimsPrincipal User => HttpContextAccessor?.HttpContext?.User;

    public UserSessionCommand(
        IHttpContextAccessor httpContextAccessor,
        IOptions<JwtOptions> jwtOptions)
    {
        HttpContextAccessor = httpContextAccessor;
        _jwtOptions = jwtOptions.Value;
    }

    public string GetStringId(ClaimsPrincipal user = null) => (user ?? User)?.FindFirstValue(_jwtOptions.TokenValidationParameters.NameClaimType);

    public Guid GetId(ClaimsPrincipal user = null)
    {
        var userStringId = GetStringId(user);
        if (string.IsNullOrEmpty(userStringId))
        {
            throw new NotFoundException($"{nameof(userStringId)} not found!");
        }

        if (!Guid.TryParse(userStringId, out var userId))
        {
            throw new NotFoundException($"{nameof(userId)} not found!");
        }

        return userId;
    }
}
