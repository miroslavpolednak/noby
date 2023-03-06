using Avro.Specific;
using cz.kb.osbs.mcs.notificationreport.eventapi.v3.notificationreport;
using cz.kb.osbs.mcs.notificationreport.eventapi.v3.report;
using cz.kb.osbs.mcs.sender.sendapi.v4.email;
using MassTransit;

namespace Mock.Mcs.Messaging.Consumers;

public class SendEmailConsumer : IConsumer<SendEmail>
{
    private readonly ITopicProducer<ISpecificRecord> _producer;

    public SendEmailConsumer(ITopicProducer<ISpecificRecord> producer)
    {
        _producer = producer;
    }
    
    public async Task Consume(ConsumeContext<SendEmail> context)
    {
        await _producer.Produce(new NotificationReport
        {
            id = context.Message.id,
            channel = new Channel
            {
                id = "EMAIL"
            },
            state = "SENT",
            exactlyOn = DateTime.Now,
            notificationErrors = new List<NotificationError>()
        });
    }
}