using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Entities.Base
{
    public abstract class BaseEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Timestamp]
        public byte[]? Timestamp { get; set; }
    }
}
