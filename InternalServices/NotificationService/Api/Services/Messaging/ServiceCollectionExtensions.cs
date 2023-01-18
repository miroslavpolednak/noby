using Avro.Specific;
using CIS.InternalServices.NotificationService.Api.Configuration;
using CIS.InternalServices.NotificationService.Api.Services.Messaging.Consumers;
using Confluent.Kafka;
using cz.kb.osbs.mcs.notificationreport.eventapi.v3.report;
using KB.Speed.MassTransit.DependencyInjection;
using KB.Speed.MassTransit.Kafka;
using KB.Speed.Messaging.Kafka.DependencyInjection;
using KB.Speed.Tracing.Extensions;
using KB.Speed.Tracing.Instrumentations.AspNetCore;
using KB.Speed.Tracing.Instrumentations.HttpClient;
using MassTransit;

namespace CIS.InternalServices.NotificationService.Api.Services.Messaging;

public static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddMessaging(this WebApplicationBuilder builder)
    {
        builder.Services.AddSpeedTracing(builder.Configuration, providerBuilder =>
        {
            providerBuilder.SetDefaultResourceBuilder()
                .AddDefaultExporter()
                .AddSpeedAspNetInstrumentation()
                .AddSpeedHttpClientInstrumentation()
                .AddMassTransitInstrumentation();
        }); 
        
        var kafkaConfiguration = builder.GetKafkaConfiguration();
        var appConfiguration = builder.GetAppConfiguration();
        
        builder.Services.AddAvroSerializerConfiguration();
        builder.Services.AddAvroDeserializerConfiguration();
        builder.Services.AddApicurioSchemaRegistry();
        
        builder.Services.AddMassTransit(configurator =>
        {
            configurator.UsingInMemory((context, config) =>
            {
                config.ConfigureEndpoints(context);
            });
            
            configurator.AddRider(rider =>
            {
                var businessNode = kafkaConfiguration.Nodes.Business;
                var topics = appConfiguration.KafkaTopics;
                
                // add consumers
                rider.AddConsumer<McsResultConsumer>();
                
                // add producers
                rider.AddProducerAvro<ISpecificRecord>(topics.McsSender);
                // rider.AddProducerAvro<SendEmail>(topics.McsSender);
                // rider.AddProducerAvro<SendSMS>(topics.McsSender);
                // todo: Add Mpss SendEmail, Push, MsgBox...
                
                rider.UsingKafka((context, k) =>
                {
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
                    
                    // configure topic mapping
                    k.TopicEndpointAvro<NotificationReport, McsResultConsumer>(
                        context,
                        topics.McsResult,
                        kafkaConfiguration.GroupId,
                        e =>
                        {
                        });
                });
            });
        });
        
        return builder;
    }
}