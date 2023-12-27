namespace Core.Domain.DTO;

public abstract class BaseDto
{
    public Guid Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public byte[]? Timestamp { get; set; }
}
