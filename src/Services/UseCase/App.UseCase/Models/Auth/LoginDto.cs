using System.ComponentModel.DataAnnotations;

namespace App.UseCase.Models.Auth;

public class LoginDto
{
    [Required]
    [StringLength(50, MinimumLength = 4)]
    public string Username { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 8)]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
