namespace Core.Domain.DTO;

public abstract class BasicDto<T>
{
    public virtual T Id { get; set; } = default!;
}
