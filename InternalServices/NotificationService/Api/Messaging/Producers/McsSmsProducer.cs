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

internal class McsSmsProducer : IMcsSmsProducer
{
    private readonly ITopicProducer<IMcsSenderTopic> _producer;
    private readonly TimeProvider _dateTime;
    private readonly KafkaTopics _kafkaTopics;

    public McsSmsProducer(
        ITopicProducer<IMcsSenderTopic> producer,
        TimeProvider dateTime,
        IOptions<AppConfiguration> appOptions)
    {
        _producer = producer;
        _dateTime = dateTime;
        _kafkaTopics = appOptions.Value.KafkaTopics;
    }
    
    public async Task SendSms(McsSendApi.v4.sms.SendSMS sendSms, CancellationToken cancellationToken)
    {
        var headers = new Headers
        {
            Id = Guid.NewGuid().ToString("N"),
            B3 = Activity.Current?.RootId,
            Timestamp = _dateTime.GetLocalNow().DateTime,
            ReplyTopic = _kafkaTopics.McsResult,
            Caller = "{\"app\":\"NOBY\",\"appComp\":\"NOBY.DS.NotificationService\"}",
            // Origin = "{\"app\":\"NOBY\",\"appComp\":\"NOBY.DS.NotificationService\"}",
        };
        
        await _producer.Produce(sendSms, new ProducerPipe<IMcsSenderTopic>(headers), cancellationToken);
    }
}