namespace CIS.Infrastructure.Data.Redis;

public class RedisMessage
{
    public string MessageType { get; set; } = null!;

    public int RetryCount { get; set; }

    public DateTime CreatedAt { get; set; }

    public Dictionary<string, string> TelemetryInfo { get; set; } = null!;
}
