namespace Core.Domain.DTO;

public class BaseActivityDto
{
    public Guid Id { get; set; }
    public string? Description { get; set; }
    public Guid? ModifiedById { get; set; }
    public DateTime ModifiedTime { get; set; }

    public virtual AccountDto? ModifiedBy { get; set; } = null!;
}
