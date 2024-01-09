namespace Core.Domain.DTO;

public class UpdatableDto<T> : BasicDto<T>
{
    public virtual byte[]? Timestamp { get; set; }
}
