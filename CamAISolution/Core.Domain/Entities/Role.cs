using System.ComponentModel.DataAnnotations;
using Core.Domain.Entities.Base;

namespace Core.Domain.Entities;

public class Role : BaseEntity
{
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public virtual ICollection<AccountRole> AccountRoles { get; set; } = new HashSet<AccountRole>();
}
