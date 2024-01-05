using Core.Domain.DTO;

namespace Core.Domain.Models.DTO;

public class TicketDto : BaseDto
{
    public int TicketTypeId { get; set; }
    public Guid? AssignedToId { get; set; }
    public Guid? ShopId { get; set; }
    public string? Detail { get; set; }

    /// <summary>
    /// Reply from technician
    /// </summary>
    public string? Reply { get; set; }
    public int TicketStatusId { get; set; }
}
