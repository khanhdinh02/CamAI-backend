namespace Infrastructure.Streaming;

public class StreamingConfiguration
{
    public string Filename { get; set; } = null!;
    public string Arguments { get; set; } = null!;
    public string StreamingDomain { get; set; } = null!;
    public string StreamingReceiveDomain { get; set; } = null!;
    public int Interval { get; set; }
}
