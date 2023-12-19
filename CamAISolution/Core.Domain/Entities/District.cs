using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain.Entities.Base;

namespace Core.Domain.Entities;

public class District : BaseEntity
{
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;
    public Guid ProvinceId { get; set; }

    public virtual Province Province { get; set; } = null!;
    public virtual ICollection<Ward> Wards { get; set; } = new HashSet<Ward>();
}
