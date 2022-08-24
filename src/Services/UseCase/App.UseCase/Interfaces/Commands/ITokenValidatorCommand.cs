using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace App.UseCase.Interfaces.Commands;

public interface ITokenValidatorCommand
{
    Task ValidateSignatureAsync(MessageReceivedContext context);
}
