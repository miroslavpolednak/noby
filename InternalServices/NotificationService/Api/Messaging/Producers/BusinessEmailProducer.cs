using CIS.Core;
using CIS.Infrastructure.Attributes;
using CIS.InternalServices.NotificationService.Api.Configuration;
using CIS.InternalServices.NotificationService.Mcs;
using Confluent.Kafka;
using cz.kb.osbs.mcs.sender.sendapi.v2.email;
using Microsoft.Extensions.Options;

namespace CIS.InternalServices.NotificationService.Api.Messaging.Producers;

[ScopedService, SelfService]
public class BusinessEmailProducer
{
    private readonly IProducer<string, SendEmail> _producer;
    private readonly IDateTime _dateTime;
    private readonly KafkaConfiguration _kafkaConfiguration;
    
    public BusinessEmailProducer(
        IProducer<string, SendEmail> producer,
        IDateTime dateTime,
        IOptions<KafkaConfiguration> kafkaOptions)
    {
        _producer = producer;
        _dateTime = dateTime;
        _kafkaConfiguration = kafkaOptions.Value;
    }

    public async Task<DeliveryResult<string, SendEmail>> SendEmail(SendEmail sendEmail, CancellationToken cancellationToken)
    {
        var keyValues = new Dictionary<string, string>
        {
            { "messaging.id", Guid.NewGuid().ToString("N") },
            { "messaging.messageType", "EVENT" },
            { "messaging.kafka.payloadTypeId", sendEmail.Schema.Fullname },
            { "messaging.kafka.replyTopic", Topics.MscResultIn },
            { "messaging.kafka.replyBrokerUri", _kafkaConfiguration.BootstrapServers.Business },
            { "messaging.timestamp", _dateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd hh:mm:ssZ") },
            { "contentType", "application/*+avro" }
        };
        
        return await _producer.ProduceAsync(
            Topics.MscSenderIn,
            new Message<string, SendEmail>
            {
                Key = "",
                Timestamp = new Timestamp(_dateTime.Now),
                Headers = HeadersHelpers.CreateHeaders(keyValues),
                Value = sendEmail,
            },
            cancellationToken);
    }
}