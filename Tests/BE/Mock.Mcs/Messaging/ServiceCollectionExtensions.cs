using Avro.Specific;
using Confluent.Kafka;
using cz.kb.osbs.mcs.sender.sendapi.v4.sms;
using KB.Speed.MassTransit.DependencyInjection;
using KB.Speed.MassTransit.Kafka;
using KB.Speed.Messaging.Kafka.DependencyInjection;
using KB.Speed.Tracing.Extensions;
using KB.Speed.Tracing.Instrumentations.AspNetCore;
using KB.Speed.Tracing.Instrumentations.HttpClient;
using MassTransit;
using Mock.Mcs.Configuration;
using Mock.Mcs.Messaging.Consumers;
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
                    
                    // configure topic mapping for Mcs
                    k.TopicEndpointAvro<SendEmail, SendEmailConsumer>(
                        context,
                        topics.McsSender,
                        kafkaConfiguration.GroupId,
                        _ =>
                        {
                        });
                    
                    k.TopicEndpointAvro<SendSMS, SendSmsConsumer>(
                        context,
                        topics.McsSender,
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