using CIS.Core;
using CIS.Infrastructure.Attributes;
using CIS.InternalServices.NotificationService.Api.Configuration;
using CIS.InternalServices.NotificationService.Api.Services.Abstraction;
using CIS.InternalServices.NotificationService.Msc;
using Confluent.Kafka;
using cz.kb.osbs.mcs.notificationreport.eventapi.v2.report;
using cz.kb.osbs.mcs.sender.sendapi.v2.email;
using Microsoft.Extensions.Options;

namespace CIS.InternalServices.NotificationService.Api.Services;

[TransientService, SelfService]
public class McsBusinessService : BaseMcsService
{
    private readonly IProducer<string, SendEmail> _mcsEmailProducer;
    private readonly IConsumer<string, NotificationReport> _mcsResultConsumer;
    private readonly KafkaConfiguration _kafkaConfiguration;
    private readonly IDateTime _dateTime;

    public McsBusinessService(
        IProducer<string, SendEmail> mcsEmailProducer,
        IConsumer<string, NotificationReport> mcsResultConsumer,
        IOptions<KafkaConfiguration> kafkaOptions,
        IDateTime dateTime)
    {
        _mcsEmailProducer = mcsEmailProducer;
        _mcsResultConsumer = mcsResultConsumer;
        _kafkaConfiguration = kafkaOptions.Value;
        _dateTime = dateTime;
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
        
        return await _mcsEmailProducer.ProduceAsync(
            Topics.MscSenderIn,
            new Message<string, SendEmail>
            {
                Key = "",
                Timestamp = new Timestamp(_dateTime.Now),
                Headers = CreateHeaders(keyValues),
                Value = sendEmail,
            },
            cancellationToken);
    }

    public async Task ConsumeResult(
        CancellationToken cancellationToken,
        Func<ConsumeResult<string, NotificationReport>, Task> consumeHandler)
    {
        _mcsResultConsumer.Subscribe(Topics.MscResultIn);
        
        while (!cancellationToken.IsCancellationRequested)
        {
            var result = _mcsResultConsumer.Consume(cancellationToken);
            await consumeHandler(result);
        }
            
        _mcsResultConsumer.Close();
    }
}