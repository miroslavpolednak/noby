using Confluent.Kafka;

namespace CIS.Infrastructure.Messaging.Configuration;

public interface IKafkaRiderConfiguration
{
    string BootstrapServers { get; set; }
    
    string SslKeyLocation { get; set; }
    
    string SslKeyPassword { get; set; }
    
    SecurityProtocol SecurityProtocol { get; set; }
    
    string SslCaLocation { get; set; }
    
    string SslCaCertificateStores { get; set; }
    
    string SslCertificateLocation { get; set; }
}
