using System.Diagnostics;
using CIS.Core;
using CIS.Core.Attributes;
using CIS.InternalServices.NotificationService.Api.Configuration;
using CIS.InternalServices.NotificationService.Api.Services.Messaging.Producers.Infrastructure;
using CIS.InternalServices.NotificationService.Messaging.Partials;
using MassTransit;
using Microsoft.Extensions.Options;
using Headers = CIS.InternalServices.NotificationService.Api.Services.Messaging.Producers.Infrastructure.Headers;

namespace CIS.InternalServices.NotificationService.Api.Services.Messaging.Producers;

[ScopedService, SelfService]
public class MpssEmailProducer
{
    private readonly ITopicProducer<IMpssSendEmailTopic> _producer;
    private readonly IDateTime _dateTime;
    private readonly KafkaTopics _kafkaTopics;

    public MpssEmailProducer(
        ITopicProducer<IMpssSendEmailTopic> producer,
        IDateTime dateTime,
        IOptions<AppConfiguration> appOptions)
    {
        _producer = producer;
        _dateTime = dateTime;
        _kafkaTopics = appOptions.Value.KafkaTopics;
    }

    public async Task SendEmail(MpssSendApi.v1.email.SendEmail sendEmail, CancellationToken cancellationToken)
    {
        var headers = new Headers
        {
            Id = Guid.NewGuid().ToString("N"),
            B3 = Activity.Current?.RootId,
            Timestamp = _dateTime.Now,
            ReplyTopic = _kafkaTopics.McsResult,
            Caller = "{\"app\":\"NOBY\",\"appComp\":\"NOBY.DS.NotificationService\"}",
            // Origin = "{\"app\":\"NOBY\",\"appComp\":\"NOBY.DS.NotificationService\"}",
        };

        await _producer.Produce(sendEmail, new ProducerPipe<IMpssSendEmailTopic>(headers), cancellationToken);
    }
}