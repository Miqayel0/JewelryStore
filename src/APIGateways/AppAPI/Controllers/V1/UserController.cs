using App.UseCase.Interfaces.Queries;
using App.UseCase.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace AppAPI.Controllers.V1;

public class UserController : ApiController
{
    private readonly IUserInfoQuery _userInfoQuery;

    public UserController(IUserInfoQuery userInfoQuery)
    {
        _userInfoQuery = userInfoQuery;
    }

    [HttpGet]
    public Task<UserDto> GetUserInfo()
    {
        return _userInfoQuery.GetUserDetailsAsync();
    }
}
