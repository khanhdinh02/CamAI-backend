namespace Core.Domain.Models.Configurations;

public class HealthCheckConfiguration
{
    /// <summary>
    /// Time (second) interval between each health check
    /// </summary>
    public int EdgeBoxHealthCheckDelay { get; set; }

    public int MaxNumberOfRetry { get; set; }
    /// <summary>
    /// Time (second) interval between each retrying health check failed
    /// </summary>
    public int RetryDelay { get; set; }
}
