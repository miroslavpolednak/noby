using Avro.Specific;
using Confluent.Kafka;
using KB.Speed.MassTransit.Kafka;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.Extensions.Configuration;

namespace CIS.Infrastructure.Messaging.Kafka;

public interface IKafkaBuilderOptions
{
    IKafkaBuilderOptions AddConsumer<TConsumer, TAvroSchema>() 
        where TConsumer : IConsumer
        where TAvroSchema : ISpecificRecord;

    void AddProducer();
}

internal sealed class KafkaBuilderOptions
    : IKafkaBuilderOptions
{
    public List<>

    public IKafkaBuilderOptions AddConsumer<TConsumer, TAvroSchema>()
    {

    }
}

internal sealed class CisMessagingKafkaBuilder
    : ICisMessagingKafkaBuilder
{
    private readonly WebApplicationBuilder _builder;
    private Configuration.KafkaRiderConfiguration _kafkaRiderConfiguration = null!;
    private List<Rider> _riders = new();

    private sealed class Rider
    {
        public string Name { get; set; } = null!;
        public Configuration.KafkaRiderConfiguration Configuration { get; set; } = null!;
    }

    public CisMessagingKafkaBuilder(WebApplicationBuilder builder)
    {
        _builder = builder;
    }

    public ICisMessagingKafkaBuilder AddRider(string configurationName, IKafkaBuilderOptions options)
    {
        // takovy rider uz byl pridany
        if (_riders.Any(t => t.Name == configurationName))
        {
            throw new Core.Exceptions.CisConfigurationException(0, $"Rider '{configurationName}' has already been added");
        }

        string configurationElement = $"CisMessaging:Kafka:{configurationName}";
        var riderConfiguration = _builder
            .Configuration
            .GetSection(configurationElement)
            .Get<Configuration.KafkaRiderConfiguration>()
            ?? throw new Core.Exceptions.CisConfigurationNotFound(configurationElement);

        // validovat konfiguraci
        riderConfiguration.ValidateConfiguration();

        var rider = new Rider
        {
            Name = configurationName,
            Configuration = riderConfiguration
        };

        return this;
    }

    public void Build()
    {
        if (_riders.Count == 0)
        {
            throw new Core.Exceptions.CisConfigurationException(0, "CIS.Messaging: No Kafka riders has been added during application startup. Use AddRider() extension method to add a new rider.");
        }

        _builder.Services.AddMassTransit(configurator =>
        {
            configurator.UsingInMemory((context, config) =>
            {
                config.ConfigureEndpoints(context);
            });

            foreach (var rider in _riders)
            {
                configurator.AddRider(rider =>
                {
                    rider.AddConsumer<SendEmailConsumer>();

                    rider.AddProducerAvro<ISpecificRecord>(topics.McsResult);

                    rider.UsingKafka((context, k) =>
                    {
                        // 1. configure connection
                        k.SecurityProtocol = SecurityProtocol.Ssl;
                        k.Host(businessNode.BootstrapServers, c =>
                        {
                            if (businessNode.SecurityProtocol == SecurityProtocol.Ssl)
                            {
                                c.UseSsl(sslConfig =>
                                {
                                    sslConfig.EnableCertificateVerification = true;
                                    sslConfig.SslCaLocation = businessNode.SslCaLocation;
                                    sslConfig.SslCertificateLocation = businessNode.SslCertificateLocation;
                                    sslConfig.KeyLocation = businessNode.SslKeyLocation;
                                    sslConfig.KeyPassword = businessNode.SslKeyPassword;
                                    sslConfig.SslCaCertificateStores = businessNode.SslCaCertificateStores;
                                });
                            }
                        });
                    });
                });
            }
        });
    }
}
