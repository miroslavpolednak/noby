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
    public async Task<SmsPushResponse> PushSms([FromBody] SmsPushRequest request, CancellationToken token)
        => await _mediator.Send(request, token);

    [HttpPost("smsFromTemplate")]
    [Produces("application/json")]
    public async Task<SmsFromTemplatePushResponse> PushSmsFromTemplate([FromBody] SmsFromTemplatePushRequest request, CancellationToken token)
        => await _mediator.Send(request, token);
}