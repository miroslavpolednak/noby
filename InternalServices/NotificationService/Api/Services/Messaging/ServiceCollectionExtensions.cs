using CIS.InternalServices.NotificationService.Api.Configuration;
using CIS.InternalServices.NotificationService.Api.Services.Messaging.Consumers;
using CIS.InternalServices.NotificationService.Api.Services.Messaging.Infrastructure;
using CIS.InternalServices.NotificationService.Mcs.Partials;
using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using cz.kb.osbs.mcs.notificationreport.eventapi.v3.report;
using cz.kb.osbs.mcs.sender.sendapi.v4.email;
using cz.kb.osbs.mcs.sender.sendapi.v4.sms;
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
                rider.AddConsumer<MpssSendEmailConsumer>();
                
                // add producers
                // rider.AddProducerAvro<IMcsSenderCommand>(topics.McsSender);
                rider.AddProducerAvro<MpssSendApi.v1.email.SendEmail>(topics.NobySendEmail);
                
                var multipleTypeConfig = new MultipleTypeConfigBuilder<IMcsSenderCommand>()
                    .AddType<SendEmail>(SendEmail._SCHEMA)
                    .AddType<SendSMS>(SendSMS._SCHEMA)
                    .Build();
                
                rider.AddProducer<IMcsSenderCommand>(topics.McsSender, (riderContext, conf) =>
                {
                    var serializerConfig = new Confluent.SchemaRegistry.Serdes.AvroSerializerConfig
                    {
                        SubjectNameStrategy = SubjectNameStrategy.Record,
                        AutoRegisterSchemas = false
                    };

                    var schemaRegistryClient = riderContext.GetRequiredService<ISchemaRegistryClient>();

                    // var keySerializer = new Confluent.SchemaRegistry.Serdes.AvroSerializer<Null>(schemaRegistryClient);
                    // conf.SetKeySerializer(keySerializer.AsSyncOverAsync());
                    
                    var valueSerializer = new MultipleTypeSerializer<IMcsSenderCommand>(multipleTypeConfig, schemaRegistryClient, serializerConfig);
                    conf.SetValueSerializer(valueSerializer.AsSyncOverAsync());
                });
                
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
                    
                    // configure topic mapping for Mcs
                    k.TopicEndpointAvro<NotificationReport, McsResultConsumer>(
                        context,
                        topics.McsResult,
                        kafkaConfiguration.GroupId,
                        _ =>
                        {
                        });
                    
                    // configure topic mapping for Mpss
                    k.TopicEndpointAvro<MpssSendApi.v1.email.SendEmail, MpssSendEmailConsumer>(
                        context,
                        topics.NobySendEmail,
                        kafkaConfiguration.GroupId,
                        _ =>
                        {
                        });
                });
            });
        });

        return builder;
    }
}