using MassTransit;

namespace DomainServices.CaseService.Api.Messaging.SendEmail;

internal class NotificationReportConsumer : IConsumer<cz.kb.osbs.mcs.notificationreport.eventapi.v3.report.NotificationReport>
{
    public Task Consume(ConsumeContext<cz.kb.osbs.mcs.notificationreport.eventapi.v3.report.NotificationReport> context)
    {
        var s = "xxx";
        return Task.CompletedTask;
    }
}
