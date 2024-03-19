using Core.Domain.Entities.Base;
using Core.Domain.Enums;

namespace Core.Domain.Entities;

public class Request : BusinessEntity
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
    public virtual Account Account { get; set; } = null!;
    public virtual Shop? Shop { get; set; }
    public virtual EdgeBox? EdgeBox { get; set; }
}
