using CIS.Core;
using CIS.Core.Attributes;
using CIS.InternalServices.NotificationService.Api.Configuration;
using CIS.InternalServices.NotificationService.Api.Services.Mcs.Producers.Infrastructure;
using CIS.InternalServices.NotificationService.Mcs;
using cz.kb.osbs.mcs.sender.sendapi.v1.sms;
using MassTransit;
using Microsoft.Extensions.Options;

namespace CIS.InternalServices.NotificationService.Api.Services.Mcs.Producers;

[ScopedService, SelfService]
public class McsSmsProducer
{
    private readonly ITopicProducer<SendSMS> _producer;
    private readonly IDateTime _dateTime;
    private readonly KafkaConfiguration _kafkaConfiguration;

    public McsSmsProducer(
        ITopicProducer<SendSMS> producer,
        IDateTime dateTime,
        IOptions<KafkaConfiguration> kafkaOptions)
    {
        _producer = producer;
        _dateTime = dateTime;
        _kafkaConfiguration = kafkaOptions.Value;
    }
    
    public async Task SendSms(SendSMS sendSms, CancellationToken cancellationToken)
    {
        var id = Guid.NewGuid().ToString("N");
        var pipe = new ProducerPipe<SendSMS>(id, Topics.McsResult,
            _kafkaConfiguration.Nodes.Business.BootstrapServers, _dateTime.Now);
        
        await _producer.Produce(sendSms, pipe, cancellationToken);
    }
}