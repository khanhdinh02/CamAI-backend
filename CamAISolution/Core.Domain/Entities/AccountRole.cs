using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities;

[Table("AccountRole")]
public class AccountRole
{
    public Guid AccountId { get; set; }
    public Guid RoleId { get; set; }

    public virtual Account Account { get; set; } = null!;
    public virtual Role Role { get; set; } = null!;
}
