namespace CIS.Infrastructure.Messaging.Kafka.Dto;

public sealed class ProducerKbHeaders
{
    public string Id { get; set; } = null!;

    public string? B3 { get; set; }

    public string? ReplyTopic { get; set; }

    public string? ReplyBrokerUri { get; set; }

    public DateTime Timestamp { get; set; }

    public string Origin { get; set; } = null!;

    public string Caller { get; set; } = null!;
}
