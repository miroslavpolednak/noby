using System.ComponentModel.DataAnnotations;
using CIS.InternalServices.NotificationService.Contracts.Email;
using CIS.InternalServices.NotificationService.Contracts.Result;
using CIS.InternalServices.NotificationService.Contracts.Sms;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.Notification;

[Authorize]
[ApiController]
[Route("v2/notification")]
public class NotificationController : ControllerBase
{
    private readonly IMediator _mediator;

    public NotificationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("sms")]
    [Produces("application/json")]
    public async Task<SmsSendResponse> SendSms([FromBody] SmsSendRequest request, CancellationToken token)
        => await _mediator.Send(request, token);

    [HttpPost("smsFromTemplate")]
    [Produces("application/json")]
    public async Task<SmsFromTemplateSendResponse> SendSmsFromTemplate([FromBody] SmsFromTemplateSendRequest request, CancellationToken token)
        => await _mediator.Send(request, token);

    [HttpPost("email")]
    [Produces("application/json")]
    public async Task<EmailSendResponse> SendEmail([FromBody] EmailSendRequest request, CancellationToken token)
        => await _mediator.Send(request, token);
    
    [HttpPost("emailFromTemplate")]
    [Produces("application/json")]
    public async Task<EmailFromTemplateSendResponse> SendEmailFromTemplate([FromBody] EmailFromTemplateSendRequest request, CancellationToken token)
        => await _mediator.Send(request, token);

    [HttpGet("result/{id}")]
    [Produces("application/json")]
    public async Task<ResultGetResponse> GetResult([Required] string id, CancellationToken token)
        => await _mediator.Send(new ResultGetRequest { NotificationId = id }, token);
}