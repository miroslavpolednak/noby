using CIS.InternalServices.NotificationService.Api.Configuration;
using CIS.InternalServices.NotificationService.Api.Mcs.Consumers;
using CIS.InternalServices.NotificationService.Api.Mcs.Consumers.BackgroundServices;
using CIS.InternalServices.NotificationService.Api.Mcs.Producers;
using CIS.InternalServices.NotificationService.Mcs.AvroSerializers;
using Confluent.Kafka;
using Confluent.Kafka.DependencyInjection;

namespace CIS.InternalServices.NotificationService.Api.Mcs;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMessaging(
        this IServiceCollection services,
        KafkaConfiguration kafkaConfiguration)
    {

        var businessConsumerConfig = CreateConsumerConfig(kafkaConfiguration.Nodes.Business);
        var businessProducerConfig = CreateProducerConfig(kafkaConfiguration.Nodes.Business);
        
        var logmanConsumerConfig = CreateConsumerConfig(kafkaConfiguration.Nodes.Logman);
        var logmanProducerConfig = CreateProducerConfig(kafkaConfiguration.Nodes.Logman);

        return services
            .AddAvroSerializers()
            .AddHostedService<BusinessResultConsumerService>()
            .AddHostedService<LogmanResultConsumerService>()
            .AddKafkaClient(new Dictionary<string, string>
            {
                { "group.id", kafkaConfiguration.GroupId },
                { "ssl.keystore.location", logmanConsumerConfig.SslKeystoreLocation },
                { "ssl.keystore.password", logmanConsumerConfig.SslKeystorePassword },
                { "security.protocol", logmanConsumerConfig.SecurityProtocol?.ToString() ?? "" },
                { "ssl.ca.location", logmanConsumerConfig.SslCaLocation },
                { "ssl.certificate.location", logmanConsumerConfig.SslCertificateLocation },
                { "enable.idempotence", "true" },
                { "enable.ssl.certificate.verification", "false" },
                { "debug", kafkaConfiguration.Debug },
            })
            .AddKafkaClient<BusinessResultConsumer>(businessConsumerConfig)
            .AddKafkaClient<BusinessEmailProducer>(businessProducerConfig)
            .AddKafkaClient<LogmanResultConsumer>(logmanConsumerConfig)
            .AddKafkaClient<LogmanEmailProducer>(logmanProducerConfig)
            .AddKafkaClient<LogmanSmsProducer>(logmanProducerConfig);
    }
    
    private static ConsumerConfig CreateConsumerConfig(Node node) => new ()
    {
        BootstrapServers = node.BootstrapServers,
        SslKeystoreLocation  = node.SslKeystoreLocation,
        SslKeystorePassword  = node.SslKeystorePassword,
        SecurityProtocol  = node.SecurityProtocol,
        SslCaLocation = node.SslCaLocation,
        SslCertificateLocation = node.SslCertificateLocation
    };
   
    private static ProducerConfig CreateProducerConfig(Node node) => new ()
    {
        BootstrapServers = node.BootstrapServers,
        SslKeystoreLocation  = node.SslKeystoreLocation,
        SslKeystorePassword  = node.SslKeystorePassword,
        SecurityProtocol  = node.SecurityProtocol,
        SslCaLocation = node.SslCaLocation,
        SslCertificateLocation = node.SslCertificateLocation
    };
}