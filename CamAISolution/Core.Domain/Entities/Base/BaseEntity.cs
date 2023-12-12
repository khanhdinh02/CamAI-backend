using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Entities.Base;

public abstract class BaseEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedDate { get; set; } = Utilities.DateTimeHelper.VNDateTime;
    public DateTime ModifiedDate { get; set; }

    [Timestamp]
    public byte[]? Timestamp { get; set; }
}
