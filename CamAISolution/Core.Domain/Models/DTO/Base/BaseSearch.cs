using System.ComponentModel.DataAnnotations;

namespace Core.Domain.DTO;

public abstract class BaseSearchRequest
{
    [Range(1, 1000)]
    public int Size { get; set; } = 10;
    public int PageIndex { get; set; } = 0;
}
