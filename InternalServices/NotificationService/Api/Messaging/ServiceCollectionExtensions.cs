using CIS.Infrastructure.Messaging;
using CIS.InternalServices.NotificationService.Api.Configuration;
using CIS.InternalServices.NotificationService.Api.Messaging.Consumers.Result;
using CIS.InternalServices.NotificationService.Api.Messaging.Messages.Partials;
using CIS.InternalServices.NotificationService.Api.Messaging.Producers;
using CIS.InternalServices.NotificationService.Api.Messaging.Producers.Abstraction;

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
            .Build();

        builder.Services
            .AddScoped<IMcsEmailProducer, McsEmailProducer>()
            .AddScoped<IMcsSmsProducer, McsSmsProducer>();

        return builder;
    }
}