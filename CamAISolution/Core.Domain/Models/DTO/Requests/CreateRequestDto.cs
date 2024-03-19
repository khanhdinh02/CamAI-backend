using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class CreateRequestDto
{
    public RequestType RequestType { get; set; }
    public Guid? ShopId { get; set; }
    public Guid? EdgeBoxId { get; set; }
    public string? Detail { get; set; }
}
