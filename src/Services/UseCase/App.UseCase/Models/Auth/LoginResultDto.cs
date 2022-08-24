namespace App.UseCase.Models.Auth;

public class LoginResultDto
{
    public TokenDto Token { get; set; }
    public UserDto User { get; set; }
}
