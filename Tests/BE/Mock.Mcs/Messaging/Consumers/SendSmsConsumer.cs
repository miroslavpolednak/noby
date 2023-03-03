using Avro.Specific;
using cz.kb.osbs.mcs.notificationreport.eventapi.v3.notificationreport;
using cz.kb.osbs.mcs.notificationreport.eventapi.v3.report;
using cz.kb.osbs.mcs.sender.sendapi.v4.sms;
using MassTransit;

namespace Mock.Mcs.Messaging.Consumers;

public class SendSmsConsumer : IConsumer<SendSMS>
{
    private readonly ITopicProducer<ISpecificRecord> _producer;

    public SendSmsConsumer(ITopicProducer<ISpecificRecord> producer)
    {
        _producer = producer;
    }

    public async Task Consume(ConsumeContext<SendSMS> context)
    {
        await _producer.Produce(new NotificationReport
        {
            id = context.Message.id,
            channel = new Channel
            {
                id = "SMS"
            },
            state = "SENT",
            exactlyOn = DateTime.Now,
            notificationErrors = new List<NotificationError>()
        });
    }
}