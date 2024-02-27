using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain.Entities.Base;

namespace Core.Domain.Entities;

public class EdgeBoxModel : BusinessEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? ModelCode { get; set; }
    public string? Manufacturer { get; set; }
    public string? CPU { get; set; }
    public string? RAM { get; set; }
    public string? Storage { get; set; }
    public string? OS { get; set; }

    public virtual ICollection<EdgeBox> EdgeBoxes { get; set; } = new HashSet<EdgeBox>();
}
