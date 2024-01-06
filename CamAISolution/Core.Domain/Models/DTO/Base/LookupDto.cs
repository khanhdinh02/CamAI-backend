namespace Core.Domain.DTO;

public abstract class LookupDto : UpdatableDto<int>
{
    public override int Id { get; set; }
    public virtual string Name { get; set; } = null!;
    public virtual string? Description { get; set; }
}
