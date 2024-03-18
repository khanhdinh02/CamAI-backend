namespace Core.Domain.Models.Publishers;

public class ConfirmedEdgeBoxActivation
{
    public Guid EdgeBoxId { get; set; }
    public bool IsActivatedSuccessfully { get; set; }
}