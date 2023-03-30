using Avro.Specific;
using CIS.Infrastructure.Messaging.Kafka.Internals;
using Confluent.Kafka;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace CIS.Infrastructure.Messaging.Kafka;

public static class Extensions
{
    public static Configuration.IKafkaRiderConfiguration GetKafkaRiderConfiguration(this WebApplicationBuilder builder)
    {
        string configurationElement = $"{Constants.MessagingConfigurationElement}:{Constants.KafkaConfigurationElement}";

        var configuration = builder
            .Configuration
            .GetSection(configurationElement)
            .Get<Configuration.KafkaRiderConfiguration>()
            ?? throw new Core.Exceptions.CisConfigurationNotFound(configurationElement);

        // validovat konfiguraci
        configuration.ValidateConfiguration();

        return configuration;
    }

    public static void CreateKafkaHost(this IKafkaFactoryConfigurator factoryConfigurator, Configuration.IKafkaRiderConfiguration configuration)
    {
        if (!string.IsNullOrEmpty(configuration.Debug))
        {
            factoryConfigurator.Debug = configuration.Debug;
        }

        factoryConfigurator.SecurityProtocol = configuration.SecurityProtocol;

        factoryConfigurator.Host(configuration.BootstrapServers, c =>
        {
            if (configuration.SecurityProtocol == SecurityProtocol.Ssl)
            {
                c.UseSsl(sslConfig =>
                {
                    sslConfig.EnableCertificateVerification = true;
                    sslConfig.SslCaLocation = configuration.SslCaLocation;
                    sslConfig.SslCertificateLocation = configuration.SslCertificateLocation;
                    sslConfig.KeyLocation = configuration.SslKeyLocation;
                    sslConfig.KeyPassword = configuration.SslKeyPassword;
                    sslConfig.SslCaCertificateStores = configuration.SslCaCertificateStores;
                });
            }
        });
    }
}
