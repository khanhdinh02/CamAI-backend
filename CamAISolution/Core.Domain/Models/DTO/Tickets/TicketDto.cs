namespace Core.Domain.DTO;

public class TicketDto : BaseDto
{
    public Guid? AssignedToId { get; set; }
    public Guid? ShopId { get; set; }
    public string? Detail { get; set; }

    public string? Reply { get; set; }
    public ShopDto? Shop { get; set; }
    public TicketTypeDto TicketType { get; set; } = null!;
    public TicketStatusDto TicketStatus { get; set; } = null!;
}
