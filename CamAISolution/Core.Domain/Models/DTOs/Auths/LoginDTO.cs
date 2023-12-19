using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Models.DTOs.Auths;

public class LoginDTO
{
    [Required]
    public string Username { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;

}
