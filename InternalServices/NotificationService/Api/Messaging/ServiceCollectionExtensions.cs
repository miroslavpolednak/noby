using CIS.InternalServices.NotificationService.Api.Configuration;
using CIS.InternalServices.NotificationService.Api.Messaging.Consumers;
using CIS.InternalServices.NotificationService.Api.Messaging.Consumers.BackgroundServices;
using CIS.InternalServices.NotificationService.Api.Messaging.Producers;
using CIS.InternalServices.NotificationService.Mcs.AvroSerializers;
using Confluent.Kafka;
using Confluent.Kafka.DependencyInjection;

namespace CIS.InternalServices.NotificationService.Api.Messaging;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMessaging(
        this IServiceCollection services,
        KafkaConfiguration kafkaConfiguration)
    {
        var businessBootstrapServer = kafkaConfiguration.BootstrapServers.Business;
        var businessConsumerConfig = new ConsumerConfig { BootstrapServers = businessBootstrapServer };
        var businessProducerConfig = new ProducerConfig { BootstrapServers = businessBootstrapServer };
        
        var logmanBootstrapServer = kafkaConfiguration.BootstrapServers.Logman;
        var logmanConsumerConfig = new ConsumerConfig { BootstrapServers = logmanBootstrapServer };
        var logmanProducerConfig = new ProducerConfig { BootstrapServers = logmanBootstrapServer };

        return services
            .AddAvroSerializers()
            .AddHostedService<BusinessResultConsumerService>()
            .AddHostedService<LogmanResultConsumerService>()
            .AddKafkaClient(new Dictionary<string, string>
            {
                { "group.id", kafkaConfiguration.GroupId },
                { "ssl.keystore.location", kafkaConfiguration.SslKeystoreLocation },
                { "ssl.keystore.password", kafkaConfiguration.SslKeystorePassword },
                { "security.protocol", kafkaConfiguration.SecurityProtocol },
                { "ssl.ca.location", kafkaConfiguration.SslCaLocation },
                { "ssl.certificate.location", kafkaConfiguration.SslCertificateLocation },
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
}