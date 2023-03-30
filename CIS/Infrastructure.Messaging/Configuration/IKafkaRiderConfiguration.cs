using Confluent.Kafka;

namespace CIS.Infrastructure.Messaging.Configuration;

public interface IKafkaRiderConfiguration
{
    string BootstrapServers { get; }
    
    string? SslKeyLocation { get; }
    
    string? SslKeyPassword { get; }
    
    SecurityProtocol SecurityProtocol { get; }
    
    string? SslCaLocation { get; }
    
    string? SslCaCertificateStores { get; }
    
    string? SslCertificateLocation { get; }

    string? Debug { get; }
}
