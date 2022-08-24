using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace App.UseCase.Interfaces.Commands;

public interface IUserSessionCommand
{
    IHttpContextAccessor HttpContextAccessor { get; }
    ClaimsPrincipal User { get; }
    Guid GetId(ClaimsPrincipal user = null);
}
