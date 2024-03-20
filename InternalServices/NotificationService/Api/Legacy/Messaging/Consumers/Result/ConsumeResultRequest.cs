namespace CIS.InternalServices.NotificationService.Api.Messaging.Consumers.Result;

public class ConsumeResultRequest : IRequest<ConsumeResultResponse>
{
    public cz.kb.osbs.mcs.notificationreport.eventapi.v3.report.NotificationReport NotificationReport { get; set; } = null!;
}