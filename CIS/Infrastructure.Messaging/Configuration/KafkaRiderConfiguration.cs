using Confluent.Kafka;

namespace CIS.Infrastructure.Messaging.Configuration;

internal sealed class KafkaRiderConfiguration
    : IKafkaRiderConfiguration
{
    public string BootstrapServers { get; set; } = null!;
    public string SslKeyLocation { get; set; } = null!;
    public string SslKeyPassword { get; set; } = null!;
    public SecurityProtocol SecurityProtocol { get; set; } = Confluent.Kafka.SecurityProtocol.Ssl;
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
