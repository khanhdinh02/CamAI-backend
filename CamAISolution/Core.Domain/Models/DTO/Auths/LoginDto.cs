using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Models.DTO.Auths;

public class LoginDto
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
