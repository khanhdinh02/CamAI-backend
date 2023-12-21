using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Models.DTOs;

public abstract class BaseSearchRequest
{
    [Range(1, 1000)]
    public int Size { get; set; } = 5;
    public int PageIndex { get; set; }
}
