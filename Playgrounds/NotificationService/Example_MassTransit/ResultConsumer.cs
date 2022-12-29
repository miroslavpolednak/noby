using cz.kb.osbs.mcs.notificationreport.eventapi.v3.report;
using MassTransit;

namespace Example_MassTransit;

public class ResultConsumer : IConsumer<NotificationReport>
{
    private readonly ILogger<ResultConsumer> _logger;

    public ResultConsumer(ILogger<ResultConsumer> logger)
    {
        _logger = logger;
    }
    
    public Task Consume(ConsumeContext<NotificationReport> context)
    {
        _logger.LogInformation("Received message: {0}, {1}", context.Message.id, context.Message.state);
        return Task.CompletedTask;
    }
}