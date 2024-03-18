using CIS.Infrastructure.Messaging.Kafka;
using KB.Speed.Messaging.Kafka.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using System.Reflection;
using KafkaFlow;
using CIS.Infrastructure.Messaging.KafkaFlow;
using CIS.Infrastructure.Messaging.KafkaFlow.Configuration;

namespace CIS.Infrastructure.Messaging;

internal sealed class CisMessagingBuilder : ICisMessagingBuilder
{
    internal readonly WebApplicationBuilder _appBuilder;

    public CisMessagingBuilder(WebApplicationBuilder builder)
    {
        _appBuilder = builder;
    }

    public ICisMessagingKafkaBuilder AddKafka(Assembly? assembly = null)
    {
        var configuration = _appBuilder.GetKafkaRiderConfiguration();

        if (!configuration.Disabled)
        {
            _appBuilder.Services.AddAvroSerializerConfiguration();
            _appBuilder.Services.AddAvroDeserializerConfiguration();
            _appBuilder.Services.AddJsonSerializerConfiguration();
            _appBuilder.Services.AddJsonDeserializerConfiguration();
            _appBuilder.Services.AddApicurioSchemaRegistry();
        }

        if (assembly is null)
        {
            assembly = Assembly.GetEntryAssembly()!;
        }
        return new CisMessagingKafkaBuilder(this, configuration, assembly);
    }

    public ICisMessagingBuilder AddKafkaFlow(Action<IKafkaFlowMessagingConfigurator> messaging)
    {
        var settings = new KafkaFlowConfiguratorSettings(_appBuilder.Configuration);

        if (settings.Configuration.Disabled || _appBuilder.Environment.EnvironmentName.Equals("Testing", StringComparison.OrdinalIgnoreCase))
            return this;

        _appBuilder.Services.AddSchemaRegistryClient(settings);

        _appBuilder.Services.AddKafkaFlowHostedService(
            kafka =>
            {
                kafka.AddCluster(cluster =>
                     {
                         cluster.WithBrokers(settings.Configuration.Brokers);
                         cluster.WithSecurityInformation(securityInfo => KafkaFlowSecurityInformationHelper.SetSecurityInfo(settings.Configuration, securityInfo));

                         KafkaFlowMessagingConfigurator.Configure(settings, messaging, cluster);
                     }).UseMicrosoftLog();

                if (settings.Configuration.Admin is not null && !string.IsNullOrWhiteSpace(settings.Configuration.Admin.Topic))
                {
                    kafka.AddCluster(cluster =>
                    {
                        cluster.WithBrokers([settings.Configuration.Admin.Broker]);
                        cluster.WithSecurityInformation(securityInfo => KafkaFlowSecurityInformationHelper.SetSecurityInfo(settings.Configuration, securityInfo));

                        cluster.EnableAdminMessages(settings.Configuration.Admin.Topic).EnableTelemetry(settings.Configuration.Admin.Topic);
                    });
                }
            });

        return this;
    }

    public ICisMessagingBuilder AddKafkaFlowDashboard()
    {
        var settings = new KafkaFlowConfiguratorSettings(_appBuilder.Configuration);

        if (_appBuilder.Environment.EnvironmentName.Equals("Testing", StringComparison.OrdinalIgnoreCase))
            return this;

        if (settings.Configuration.Disabled || settings.Configuration.Admin is null)
            throw new InvalidOperationException("KafkaFlow messaging is not configured");

        _appBuilder.Services.AddKafkaFlowHostedService(
            kafka => kafka.AddCluster(cluster =>
                          {
                              cluster.WithBrokers(settings.Configuration.Brokers);

                              cluster.WithSecurityInformation(
                                  securityInfo => KafkaFlowSecurityInformationHelper.SetSecurityInfo(settings.Configuration, securityInfo)
                              );

                              cluster.EnableAdminMessages(settings.Configuration.Admin.Topic).EnableTelemetry(settings.Configuration.Admin.Topic);
                          })
                          .UseMicrosoftLog());

        return this;
    }
}
