using Core.Domain.Enums;

namespace Infrastructure.Observer.Messages;

public class EdgeBoxCameraDto
{
    public Guid Id { get; set; }
    public Guid ShopId { get; set; }
    public string Name { get; set; } = null!;
    public Zone Zone { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Protocol { get; set; } = null!;
    public int Port { get; set; }
    public string Host { get; set; } = null!;
    public string Path { get; set; } = null!;
    public bool WillRunAI { get; set; }

    public CameraStatus Status { get; set; } = CameraStatus.New;
}
