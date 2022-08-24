using System.ComponentModel.DataAnnotations;

namespace App.UseCase.Models.Auth;

public class AppSessionDto
{
}

public class AppSessionUpdateDto
{
    [Required]
    [MaxLength(3000)]
    public string RefreshToken { get; set; }
}