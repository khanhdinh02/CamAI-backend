namespace Core.Domain.DTO;

public class TicketSearchRequest : BaseSearchRequest
{
    public Guid? AssignedToId { get; }
    public int? TicketStatusId { get; }
}
