namespace App.UseCase.Models.Auth;

public class RegisterResultDto
{
    public TokenDto Token { get; set; }
    public UserDto User { get; set; }
}
