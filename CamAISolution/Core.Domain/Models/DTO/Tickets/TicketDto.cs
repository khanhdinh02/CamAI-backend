namespace Core.Domain.DTO;

public class TicketDto : BaseDto
{
    public int TicketTypeId { get; set; }
    public Guid? AssignedToId { get; set; }
    public Guid? ShopId { get; set; }
    public string? Detail { get; set; }

    public string? Reply { get; set; }
    public int TicketStatusId { get; set; }
}
