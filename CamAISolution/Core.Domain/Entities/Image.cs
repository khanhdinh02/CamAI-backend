using System.Text.Json.Serialization;
using Core.Domain.Entities.Base;

namespace Core.Domain.Entities;

public class Image : BaseEntity<Guid>
{
    public Image()
    {
        Id = Guid.NewGuid();
    }

    /// <summary>
    /// This Uri is used to get the image in the client
    /// </summary>
    public Uri HostingUri { get; set; } = null!;

    /// <summary>
    /// Physical path in the system
    /// </summary>
    [JsonIgnore]
    public string PhysicalPath { get; set; } = null!;
    public string ContentType { get; set; } = null!;
}
