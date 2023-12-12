namespace Core.Domain.Entities.Base;

using System.ComponentModel.DataAnnotations;

public abstract class BaseEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedDate { get; set; } = Utilities.DateTimeHelper.VNDateTime;
    public DateTime ModifiedDate { get; set; }
    [Timestamp]
    public byte[]? Timestamp { get; set; }
}
