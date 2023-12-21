namespace Core.Domain.DTOs;

public abstract class BaseSearchRequest
{
    public int Size { get; set; }
    public int PageIndex { get; set; }
}
