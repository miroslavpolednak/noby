using CIS.InternalServices.NotificationService.Contracts;
using CIS.InternalServices.NotificationService.Contracts.Email;
using CIS.InternalServices.NotificationService.Contracts.Result;
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
    
    public async ValueTask<SmsSendResponse> SendSms(SmsSendRequest request, CancellationToken token)
        => await _mediator.Send(request, token);
    
    public async ValueTask<SmsFromTemplateSendResponse> SendSmsFromTemplate(SmsFromTemplateSendRequest request, CancellationToken token)
        => await _mediator.Send(request, token);

    public async ValueTask<EmailSendResponse> SendEmail(EmailSendRequest request, CancellationToken token)
        => await _mediator.Send(request, token);

    public async ValueTask<EmailFromTemplateSendResponse> SendEmailFromTemplate(EmailFromTemplateSendRequest request, CancellationToken token)
        => await _mediator.Send(request, token);

    public async ValueTask<ResultGetResponse> GetResult(ResultGetRequest request, CancellationToken token)
        => await _mediator.Send(request, token);
}