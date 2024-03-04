using Confluent.Kafka;

namespace CIS.Infrastructure.Messaging.Configuration;

public enum SchemaIdentificationType
{
    ContentId,
    GlobalId
}

public enum RetryPolicy
{
    Default,
    Durable
}

public class KafkaFlowConfiguration
{
    public bool Disabled { get; set; }

    public IEnumerable<string> Brokers { get; set; } = [];

    public SchemaRegistryConfiguration SchemaRegistry { get; set; } = new();

    public RetryPolicy RetryPolicy { get; set; } = RetryPolicy.Default;

    public int RetryTimes { get; set; } = 10;

    public int TimeBetweenTriesMs { get; set; } = 1000;

    public string? AdminTopic { get; set; }

    public SecurityProtocol SecurityProtocol { get; set; }

    public string? SslKeyLocation { get; set; }

    public string? SslKeyPassword { get; set; }

    public string? SslCertificateLocation { get; set; }

    public class SchemaRegistryConfiguration
    {
        public string SchemaRegistryUrl { get; set; } = null!;

        public SchemaIdentificationType SchemaIdentificationType { get; set; } = SchemaIdentificationType.ContentId;
    }
}