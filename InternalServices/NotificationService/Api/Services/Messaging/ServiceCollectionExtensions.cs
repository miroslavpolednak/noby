using CIS.Infrastructure.Messaging;
using CIS.InternalServices.NotificationService.Api.Configuration;
using CIS.InternalServices.NotificationService.Api.Services.Messaging.Consumers;
using CIS.InternalServices.NotificationService.Api.Services.Messaging.Infrastructure;
using CIS.InternalServices.NotificationService.Api.Services.Messaging.Messages.Partials;
using CIS.InternalServices.NotificationService.Api.Services.Messaging.Producers;
using CIS.InternalServices.NotificationService.Api.Services.Messaging.Producers.Abstraction;
using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using cz.kb.osbs.mcs.notificationreport.eventapi.v3.report;
using cz.kb.osbs.mcs.sender.sendapi.v4.email;
using cz.kb.osbs.mcs.sender.sendapi.v4.sms;
using KB.Speed.MassTransit.DependencyInjection;
using KB.Speed.MassTransit.Kafka;
using KB.Speed.MassTransit.Tracing;
using KB.Speed.Messaging.Kafka.DependencyInjection;
using KB.Speed.Tracing.Extensions;
using MassTransit;

namespace CIS.InternalServices.NotificationService.Api.Services.Messaging;

public static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddMessaging(this WebApplicationBuilder builder)
    {
        var appConfiguration = builder.GetAppConfiguration();
        var topics = appConfiguration.KafkaTopics;
        
        builder
            .AddCisMessaging()
            .AddKafka()
            // Mcs
                .AddConsumer<McsResultConsumer>()
                .AddConsumerTopicAvro<IMcsResultTopic>(topics.McsResult)
                .AddProducerAvro<IMcsSenderTopic>(topics.McsSender)
            // Mpss
                .AddConsumer<MpssSendEmailConsumer>()
                .AddConsumerTopicAvro<IMpssSendEmailTopic>(topics.NobySendEmail)
                .AddProducerAvro<IMpssSendEmailTopic>(topics.NobySendEmail)
            .Build();

        builder.Services
            .AddScoped<IMcsEmailProducer, McsEmailProducer>()
            .AddScoped<IMcsSmsProducer, McsSmsProducer>()
            .AddScoped<IMpssEmailProducer, MpssEmailProducer>();

        return builder;
    }
}