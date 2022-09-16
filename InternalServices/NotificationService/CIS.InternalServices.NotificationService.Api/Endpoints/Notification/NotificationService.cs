using CIS.InternalServices.NotificationService.Contracts;
using CIS.InternalServices.NotificationService.Contracts.Sms;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.Notification;

public class NotificationService : INotificationService
{
    private readonly IMediator _mediator;

    public NotificationService(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async ValueTask<SmsPushResponse> PushSms(SmsPushRequest request, CancellationToken token)
        => await _mediator.Send(request, token);
    
    public async ValueTask<SmsFromTemplatePushResponse> PushSmsFromTemplate(SmsFromTemplatePushRequest request, CancellationToken token)
        => await _mediator.Send(request, token);
}