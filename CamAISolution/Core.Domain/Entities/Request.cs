using Core.Domain.Entities.Base;

namespace Core.Domain.Entities;

public class Request : BusinessEntity
{
    public int RequestTypeId { get; set; }
    public Guid AccountId { get; set; }
    public Guid? ShopId { get; set; }
    public string? Detail { get; set; }

    /// <summary>
    /// Reply from admin
    /// </summary>
    public string? Reply { get; set; }
    public int RequestStatusId { get; set; }

    public virtual RequestType RequestType { get; set; } = null!;
    public virtual Account Account { get; set; } = null!;
    public virtual Shop? Shop { get; set; }
    public virtual RequestStatus RequestStatus { get; set; } = null!;
}
