using Core.Domain.Enums;

namespace Core.Domain.Entities;

public class AccountRole
{
    public Guid AccountId { get; set; }
    public Role Role { get; set; }

    public virtual Account Account { get; set; } = null!;
}
