using CIS.InternalServices.NotificationService.Api.Configuration;
using CIS.InternalServices.NotificationService.Api.Services.Mcs.Consumers;
using CIS.InternalServices.NotificationService.Mcs;
using Confluent.Kafka;
using cz.kb.osbs.mcs.notificationreport.eventapi.v3.report;
using cz.kb.osbs.mcs.sender.sendapi.v1.sms;
using cz.kb.osbs.mcs.sender.sendapi.v4.email;
using KB.Speed.MassTransit.Kafka;
using KB.Speed.Messaging.Kafka.DependencyInjection;
using MassTransit;

namespace CIS.InternalServices.NotificationService.Api.Services.Mcs;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMessaging(
        this IServiceCollection services,
        KafkaConfiguration kafkaConfiguration)
    {
        services.AddAvroSerializerConfiguration();
        services.AddAvroDeserializerConfiguration();
        services.AddApicurioSchemaRegistry();
        
        services.AddMassTransit(configurator =>
        {
            configurator.UsingInMemory((context, config) =>
            {
                config.ConfigureEndpoints(context);
            });
            
            configurator.AddRider(rider =>
            {
                var businessNode = kafkaConfiguration.Nodes.Business;
                
                // add consumers
                rider.AddConsumer<ResultConsumer>();
                
                // add producers
                rider.AddProducerAvro<SendEmail>(Topics.McsSender);
                rider.AddProducerAvro<SendSMS>(Topics.McsSender);
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
                    k.TopicEndpointAvro<NotificationReport, ResultConsumer>(
                        context,
                        Topics.McsResult,
                        kafkaConfiguration.GroupId,
                        e =>
                        {
                        });
                });
            });
        });
        
        return services;
    }
}