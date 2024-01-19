namespace Infrastructure.MessageQueue.Models;

public class RabbitMQConfiguration
{
    public string RabbitMQHost { get; set; } = null!;
    public string RabbitMQUser { get; set; } = null!;
    public string RabbitMQPassword { get; set; } = null!;
    public int RabbitMQPort { get; set; }
}
