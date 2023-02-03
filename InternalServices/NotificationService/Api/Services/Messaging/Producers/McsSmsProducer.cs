﻿using System.Diagnostics;
using Avro.Specific;
using CIS.Core;
using CIS.Core.Attributes;
using CIS.InternalServices.NotificationService.Api.Configuration;
using CIS.InternalServices.NotificationService.Api.Services.Messaging.Producers.Infrastructure;
using MassTransit;
using Microsoft.Extensions.Options;
using Headers = CIS.InternalServices.NotificationService.Api.Services.Messaging.Producers.Infrastructure.Headers;

namespace CIS.InternalServices.NotificationService.Api.Services.Messaging.Producers;

[ScopedService, SelfService]
public class McsSmsProducer
{
    private readonly ITopicProducer<ISpecificRecord> _producer;
    private readonly IDateTime _dateTime;
    private readonly KafkaTopics _kafkaTopics;
    private readonly KafkaConfiguration _kafkaConfiguration;

    public McsSmsProducer(
        ITopicProducer<ISpecificRecord> producer,
        IDateTime dateTime,
        IOptions<AppConfiguration> appOptions,
        IOptions<KafkaConfiguration> kafkaOptions)
    {
        _producer = producer;
        _dateTime = dateTime;
        _kafkaTopics = appOptions.Value.KafkaTopics;
        _kafkaConfiguration = kafkaOptions.Value;
    }
    
    public async Task SendSms(McsSendApi.v4.sms.SendSMS sendSms, CancellationToken cancellationToken)
    {
        var headers = new Headers
        {
            Id = Guid.NewGuid().ToString("N"),
            B3 = Activity.Current?.RootId,
            Timestamp = _dateTime.Now,
            ReplyTopic = _kafkaTopics.McsResult,
            ReplyBrokerUri = _kafkaConfiguration.Nodes.Business.BootstrapServers,
            Caller = "{\"app\":\"NOBY\",\"appComp\":\"NOBY.DS.NotificationService\"}",
            // Origin = "{\"app\":\"NOBY\",\"appComp\":\"NOBY.DS.NotificationService\"}",
        };
        
        await _producer.Produce(sendSms, new ProducerPipe<ISpecificRecord>(headers), cancellationToken);
    }
}