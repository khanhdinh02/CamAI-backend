using System.ComponentModel.DataAnnotations;

namespace Core.Domain.DTO;

public class ChangePasswordDto
{
    [Required]
    public string OldPassword { get; set; } = null!;

    [Required]
    public string NewPassword { get; set; } = null!;

    [Required]
    public string NewPasswordRetype { get; set; } = null!;
}
