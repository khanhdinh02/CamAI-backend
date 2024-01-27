using System.Text.Json.Serialization;
using Core.Domain.Entities.Base;

namespace Core.Domain.Entities;

public class Image : BaseEntity<Guid>
{
    public Image()
    {
        Id = Guid.NewGuid();
        Brands = new HashSet<Brand>();
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
    public int ImageTypeId { get; set; }
    public virtual ImageType ImageType { get; set; } = null!;
    public ICollection<Brand> Brands { get; set; }
}
