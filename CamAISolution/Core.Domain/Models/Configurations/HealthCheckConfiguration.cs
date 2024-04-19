namespace Core.Domain.Models.Configurations;

public class HealthCheckConfiguration
{
    /// <summary>
    /// Time (second) interval between each health check
    /// </summary>
    public int EdgeBoxHealthCheckDelay { get; set; }
    public int UnhealthyElapsedTime { get; set; }
    public int UnhealthyNotifyTime { get; set; }
}
