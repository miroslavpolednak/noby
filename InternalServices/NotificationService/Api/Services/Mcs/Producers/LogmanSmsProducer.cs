﻿using CIS.Core;
using CIS.Core.Attributes;
using CIS.InternalServices.NotificationService.Api.Configuration;
using CIS.InternalServices.NotificationService.Mcs;
using Confluent.Kafka;
using cz.kb.osbs.mcs.sender.sendapi.v1.sms;
using Microsoft.Extensions.Options;

namespace CIS.InternalServices.NotificationService.Api.Services.Mcs.Producers;

[ScopedService, SelfService]
public class LogmanSmsProducer
{
    private readonly IProducer<string, SendSMS> _producer;
    private readonly IDateTime _dateTime;
    private readonly KafkaConfiguration _kafkaConfiguration;

    public LogmanSmsProducer(
        IProducer<string, SendSMS> producer,
        IDateTime dateTime,
        IOptions<KafkaConfiguration> kafkaOptions)
    {
        _producer = producer;
        _dateTime = dateTime;
        _kafkaConfiguration = kafkaOptions.Value;
    }
    
    public async Task<DeliveryResult<string, SendSMS>> SendSms(SendSMS sendSms, CancellationToken cancellationToken)
    {
        var keyValues = new Dictionary<string, string>
        {
            { "messaging.id", Guid.NewGuid().ToString("N") },
            { "messaging.messageType", "EVENT" },
            { "messaging.kafka.payloadTypeId", sendSms.Schema.Fullname },
            { "messaging.kafka.replyTopic", Topics.McsResult },
            { "messaging.kafka.replyBrokerUri", _kafkaConfiguration.Nodes.Logman.BootstrapServers },
            { "messaging.timestamp", _dateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd hh:mm:ssZ") },
            { "contentType", "application/*+avro" }
        };
        
        return await _producer.ProduceAsync(
            Topics.McsSender,
            new Message<string, SendSMS>
            {
                Key = "",
                Timestamp = new Timestamp(_dateTime.Now),
                Headers = HeadersHelpers.CreateHeaders(keyValues),
                Value = sendSms
            },
            cancellationToken);
    }
}