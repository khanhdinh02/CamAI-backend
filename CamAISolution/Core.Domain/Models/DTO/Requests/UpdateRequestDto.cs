using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class UpdateRequestDto
{
    public string? Reply { get; set; }
    public RequestStatus? RequestStatus { get; set; }
}
