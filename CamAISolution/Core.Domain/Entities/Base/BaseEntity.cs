using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Entities.Base;

public abstract class BaseEntity : BaseBasicEntity
{
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    [Timestamp]
    public byte[]? Timestamp { get; set; }
}
