using cz.kb.osbs.mcs.notificationreport.eventapi.v3.report;
using MassTransit;

namespace Example_MassTransit;

public class ResultConsumer : IConsumer<NotificationReport>
{
    public Task Consume(ConsumeContext<NotificationReport> context)
    {
        Console.WriteLine($"[{DateTime.Now}]: {context.Message.id}, {context.Message.state}");
        return Task.CompletedTask;
    }
}