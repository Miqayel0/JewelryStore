using App.UseCase.Interfaces.Commands;
using App.UseCase.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppAPI.Controllers.V1;

public class AuthController : ApiController
{
    private readonly IUserAccountCommand _userCommand;

    public AuthController(IUserAccountCommand userCommand)
    {
        _userCommand = userCommand;
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    public Task<LoginResultDto> Login(LoginDto dto)
    {
        return _userCommand.LoginAsync(dto);
    }

    [HttpPost("Register")]
    [AllowAnonymous]
    public Task<RegisterResultDto> Register(RegisterDto dto)
    {
        return _userCommand.RegisterAsync(dto);
    }

    [HttpPost("RefreshToken")]
    [AllowAnonymous]
    public async Task<TokenDto> RefreshRoken(AppSessionUpdateDto dto)
    {
        var userToken = await _userCommand.AuthenticateRefreshTokenAsync(dto.RefreshToken);
        return await _userCommand.CreateUserSessionRefreshAsync(userToken.UserId, dto.RefreshToken);
    }
}
