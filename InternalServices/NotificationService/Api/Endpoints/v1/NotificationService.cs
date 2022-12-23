using CIS.InternalServices.NotificationService.Contracts;
using CIS.InternalServices.NotificationService.Contracts.Email;
using CIS.InternalServices.NotificationService.Contracts.Result;
using CIS.InternalServices.NotificationService.Contracts.Sms;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v1;

public class NotificationService : INotificationService
{
    private readonly IMediator _mediator;

    public NotificationService(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task<SmsSendResponse> SendSms(SmsSendRequest request, CancellationToken token)
        => await _mediator.Send(request, token);
    
    public async Task<SmsFromTemplateSendResponse> SendSmsFromTemplate(SmsFromTemplateSendRequest request, CancellationToken token)
        => await _mediator.Send(request, token);

    public async Task<EmailSendResponse> SendEmail(EmailSendRequest request, CancellationToken token)
        => await _mediator.Send(request, token);

    public async Task<EmailFromTemplateSendResponse> SendEmailFromTemplate(EmailFromTemplateSendRequest request, CancellationToken token)
        => await _mediator.Send(request, token);

    public async Task<ResultGetResponse> GetResult(ResultGetRequest request, CancellationToken token)
        => await _mediator.Send(request, token);

    public async Task<ResultsSearchByResponse> SearchResults(ResultsSearchByRequest request, CancellationToken token)
        => await _mediator.Send(request, token);
}