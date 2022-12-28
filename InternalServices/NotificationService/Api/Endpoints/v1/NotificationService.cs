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
    
    public async Task<SendSmsResponse> SendSms(SendSmsRequest request, CancellationToken token)
        => await _mediator.Send(request, token);
    
    public async Task<SendSmsFromTemplateResponse> SendSmsFromTemplate(SendSmsFromTemplateRequest request, CancellationToken token)
        => await _mediator.Send(request, token);

    public async Task<SendEmailResponse> SendEmail(SendEmailRequest request, CancellationToken token)
        => await _mediator.Send(request, token);

    public async Task<SendEmailFromTemplateResponse> SendEmailFromTemplate(SendEmailFromTemplateRequest request, CancellationToken token)
        => await _mediator.Send(request, token);

    public async Task<GetResultResponse> GetResult(GetResultRequest request, CancellationToken token)
        => await _mediator.Send(request, token);

    public async Task<SearchResultsResponse> SearchResults(SearchResultsRequest request, CancellationToken token)
        => await _mediator.Send(request, token);
}