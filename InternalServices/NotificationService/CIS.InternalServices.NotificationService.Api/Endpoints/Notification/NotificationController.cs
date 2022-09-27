using System.ComponentModel.DataAnnotations;
using CIS.InternalServices.NotificationService.Contracts.Email;
using CIS.InternalServices.NotificationService.Contracts.Result;
using CIS.InternalServices.NotificationService.Contracts.Sms;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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

    /// <summary>
    /// Odeslat sms notifikaci
    /// </summary>
    /// <remarks>
    /// Specs: <a target="_blank" href="https://wiki.kb.cz/display/HT/Notification+Service">https://wiki.kb.cz/display/HT/Notification+Service</a>
    /// </remarks>
    [HttpPost("sms")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: Risk Business Case" })]
    [ProducesResponseType(typeof(SmsSendResponse), StatusCodes.Status200OK)]
    public async Task<SmsSendResponse> SendSms([FromBody] SmsSendRequest request, CancellationToken token)
        => await _mediator.Send(request, token);

    /// <summary>
    /// Odeslat sms notifikaci ze šablony
    /// </summary>
    /// <remarks>
    /// Specs: <a target="_blank" href="https://wiki.kb.cz/display/HT/Notification+Service">https://wiki.kb.cz/display/HT/Notification+Service</a>
    /// </remarks>
    [HttpPost("smsFromTemplate")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: Risk Business Case" })]
    [ProducesResponseType(typeof(SmsFromTemplateSendResponse), StatusCodes.Status200OK)]
    public async Task<SmsFromTemplateSendResponse> SendSmsFromTemplate([FromBody] SmsFromTemplateSendRequest request, CancellationToken token)
        => await _mediator.Send(request, token);

    /// <summary>
    /// Odeslat email notifikaci
    /// </summary>
    /// <remarks>
    /// Specs: <a target="_blank" href="https://wiki.kb.cz/display/HT/Notification+Service">https://wiki.kb.cz/display/HT/Notification+Service</a>
    /// </remarks>
    [HttpPost("email")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: Risk Business Case" })]
    [ProducesResponseType(typeof(EmailSendResponse), StatusCodes.Status200OK)]
    public async Task<EmailSendResponse> SendEmail([FromBody] EmailSendRequest request, CancellationToken token)
        => await _mediator.Send(request, token);
    
    /// <summary>
    /// Odeslat email notifikaci ze šablony
    /// </summary>
    /// <remarks>
    /// Specs: <a target="_blank" href="https://wiki.kb.cz/display/HT/Notification+Service">https://wiki.kb.cz/display/HT/Notification+Service</a>
    /// </remarks>
    [HttpPost("emailFromTemplate")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: Risk Business Case" })]
    [ProducesResponseType(typeof(EmailFromTemplateSendResponse), StatusCodes.Status200OK)]
    public async Task<EmailFromTemplateSendResponse> SendEmailFromTemplate([FromBody] EmailFromTemplateSendRequest request, CancellationToken token)
        => await _mediator.Send(request, token);

    /// <summary>
    /// Získat výsledek o notifikaci
    /// </summary>
    /// <remarks>
    /// Specs: <a target="_blank" href="https://wiki.kb.cz/display/HT/Notification+Service">https://wiki.kb.cz/display/HT/Notification+Service</a>
    /// </remarks>
    [HttpGet("result/{id}")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: Risk Business Case" })]
    [ProducesResponseType(typeof(ResultGetResponse), StatusCodes.Status200OK)]
    public async Task<ResultGetResponse> GetResult([Required] string id, CancellationToken token)
        => await _mediator.Send(new ResultGetRequest { NotificationId = id }, token);
}