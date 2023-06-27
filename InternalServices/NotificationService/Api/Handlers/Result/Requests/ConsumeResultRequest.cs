using cz.kb.osbs.mcs.notificationreport.eventapi.v3.report;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Handlers.Result.Requests;

public class ConsumeResultRequest : IRequest<ConsumeResultResponse>
{
    public NotificationReport NotificationReport { get; set; } = null!;
}