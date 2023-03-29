using Confluent.Kafka;

namespace CIS.Infrastructure.Messaging.Configuration;

public sealed class KafkaRiderConfiguration
{
    public string BootstrapServers { get; set; } = null!;
    public string SslKeyLocation { get; set; } = null!;
    public string SslKeyPassword { get; set; } = null!;
    public SecurityProtocol? SecurityProtocol { get; set; } = null!;
    public string SslCaLocation { get; set; } = null!;
    public string SslCaCertificateStores { get; set; } = null!;
    public string SslCertificateLocation { get; set; } = null!;

    internal void ValidateConfiguration()
    {
        if (string.IsNullOrEmpty(BootstrapServers))
        {
            throw new CIS.Core.Exceptions.CisConfigurationException(0, "CIS.Messaging: BootstrapServers configuration is empty");
        }
    }
}
