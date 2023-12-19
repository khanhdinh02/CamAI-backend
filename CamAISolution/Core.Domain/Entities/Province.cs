using System.ComponentModel.DataAnnotations;
using Core.Domain.Entities.Base;

namespace Core.Domain.Entities;

public class Province : BaseEntity
{
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    public virtual ICollection<District> Districts { get; set; } = new HashSet<District>();
}
