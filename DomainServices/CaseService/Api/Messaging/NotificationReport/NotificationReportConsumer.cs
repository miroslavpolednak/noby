using MassTransit;

namespace DomainServices.CaseService.Api.Messaging.NotificationReport;

public class NotificationReportConsumer : IConsumer<cz.kb.osbs.mcs.notificationreport.eventapi.v3.report.NotificationReport>
{
    private readonly ILogger<NotificationReportConsumer> _logger;

    public NotificationReportConsumer(ILogger<NotificationReportConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<cz.kb.osbs.mcs.notificationreport.eventapi.v3.report.NotificationReport> context)
    {
        _logger.LogInformation("Received");

        // mocking a business logic with Task.Delay
        await Task.Delay(TimeSpan.FromSeconds(1));
    }
}
