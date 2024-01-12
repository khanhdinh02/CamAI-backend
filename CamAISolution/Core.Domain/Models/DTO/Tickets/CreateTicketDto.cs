namespace Core.Domain.DTO;

public class CreateTicketDto
{
    public int TicketTypeId { get; set; }
    public Guid? AssignedToId { get; set; }
    public Guid? ShopId { get; set; }
    public string? Detail { get; set; }
}
