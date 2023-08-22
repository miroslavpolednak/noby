﻿using System.Diagnostics;
using CIS.Core;
using CIS.InternalServices.NotificationService.Api.Configuration;
using CIS.InternalServices.NotificationService.Api.Messaging.Messages.Partials;
using CIS.InternalServices.NotificationService.Api.Messaging.Producers.Abstraction;
using CIS.InternalServices.NotificationService.Api.Messaging.Producers.Infrastructure;
using MassTransit;
using Microsoft.Extensions.Options;
using Headers = CIS.InternalServices.NotificationService.Api.Messaging.Producers.Infrastructure.Headers;

namespace CIS.InternalServices.NotificationService.Api.Messaging.Producers;

public class MpssEmailProducer : IMpssEmailProducer
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