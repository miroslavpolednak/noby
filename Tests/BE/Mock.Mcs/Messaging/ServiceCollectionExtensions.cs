using Avro.Specific;
using CIS.InternalServices.NotificationService.Messaging.Partials;
using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using cz.kb.osbs.mcs.sender.sendapi.v4.sms;
using KB.Speed.MassTransit.DependencyInjection;
using KB.Speed.MassTransit.Kafka;
using KB.Speed.MassTransit.Kafka.Serializers;
using KB.Speed.Messaging.Kafka.DependencyInjection;
using KB.Speed.Tracing.Extensions;
using KB.Speed.Tracing.Instrumentations.AspNetCore;
using KB.Speed.Tracing.Instrumentations.HttpClient;
using MassTransit;
using Mock.Mcs.Configuration;
using Mock.Mcs.Messaging.Consumers;
using Mock.Mcs.Messaging.Infrastructure;
using SendEmail = cz.kb.osbs.mcs.sender.sendapi.v4.email.SendEmail;

namespace Mock.Mcs.Messaging;

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
                rider.AddConsumer<SendEmailConsumer>();
                rider.AddConsumer<SendSmsConsumer>();
                
                // add producers
                rider.AddProducerAvro<ISpecificRecord>(topics.McsResult);

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
                    
                    var multipleTypeConfig = new MultipleTypeConfigBuilder<IMcsSenderCommand>()
                        .AddType<SendEmail>(SendEmail._SCHEMA)
                        .AddType<SendSMS>(SendSMS._SCHEMA)
                        .Build();
                    
                    k.TopicEndpoint<IMcsSenderCommand>(topics.McsSender, kafkaConfiguration.GroupId, conf =>
                    {
                        var schemaRegistryClient = context.GetRequiredService<ISchemaRegistryClient>();
                        var valueDeserializer = new MultipleTypeDeserializer<IMcsSenderCommand>(multipleTypeConfig, schemaRegistryClient);

                        conf.SetValueDeserializer(valueDeserializer.AsSyncOverAsync());
                        conf.ConfigureConsumer<SendEmailConsumer>(context);
                        conf.ConfigureConsumer<SendSmsConsumer>(context);
                        conf.SetHeadersDeserializer(new HeaderDeserializer());
                    });
                    
                });
            });
        });

        return builder;
    }
}