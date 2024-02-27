using System.Text.Json.Serialization;
using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class SearchNotificationRequest : BaseSearchRequest
{
    /// <summary>
    /// Always be set with current user's Id.
    /// </summary>
    [JsonIgnore]
    public Guid AccountId { get; set; }
    public Guid? NotificationId { get; set; }
    public NotificationStatus? Status { get; set; }
}
