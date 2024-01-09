namespace Core.Domain.DTO;

public class TicketDto : BaseDto
{
    public Guid? AssignedToId { get; set; }
    public Guid? ShopId { get; set; }
    public string? Detail { get; set; }

    public string? Reply { get; set; }
    public ShopDto? Shop { get; set; }
    public LookupDto TicketType { get; set; } = null!;
    public LookupDto TicketStatus { get; set; } = null!;
}
