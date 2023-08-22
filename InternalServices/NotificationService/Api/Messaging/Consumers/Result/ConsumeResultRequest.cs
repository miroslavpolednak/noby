using cz.kb.osbs.mcs.notificationreport.eventapi.v3.report;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Messaging.Consumers.Result;

public class ConsumeResultRequest : IRequest<ConsumeResultResponse>
{
    public NotificationReport NotificationReport { get; set; } = null!;
}