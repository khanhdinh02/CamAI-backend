using Core.Domain.Entities.Base;

namespace Core.Domain.Entities;

public class Account : BaseEntity {

    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;
}
