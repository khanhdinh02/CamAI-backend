using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class RequestDto : BaseDto
{
    public RequestType RequestType { get; set; }
    public Guid AccountId { get; set; }
    public Guid? ShopId { get; set; }
    public Guid? EdgeBoxId { get; set; }
    public string? Detail { get; set; }

    /// <summary>
    /// Reply from admin
    /// </summary>
    public string? Reply { get; set; }
    public RequestStatus RequestStatus { get; set; }

    public AccountDto Account { get; set; } = null!;
    public ShopDto? Shop { get; set; }
    public EdgeBoxDto? EdgeBox { get; set; }
}
