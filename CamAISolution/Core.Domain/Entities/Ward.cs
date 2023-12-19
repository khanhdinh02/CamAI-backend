using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain.Entities.Base;

namespace Core.Domain.Entities;

public class Ward : BaseEntity
{
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;
    public Guid DistrictId { get; set; }

    public virtual District District { get; set; } = null!;
}
