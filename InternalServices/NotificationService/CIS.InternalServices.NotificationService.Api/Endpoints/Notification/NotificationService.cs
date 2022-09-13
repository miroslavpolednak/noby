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
    
    public async Task<SmsPushResponse> PushSms(SmsPushRequest request, CancellationToken token)
        => await _mediator.Send(request, token);
    
    public async Task<SmsFromTemplatePushResponse> PushSmsFromTemplate(SmsFromTemplatePushRequest request, CancellationToken token)
        => await _mediator.Send(request, token);
}