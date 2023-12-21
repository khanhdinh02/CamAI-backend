namespace Core.Domain.DTO;

public abstract class BaseSearchRequest
{
    public int Size { get; set; } = 1;
    public int PageIndex { get; set; } = 0;
}
