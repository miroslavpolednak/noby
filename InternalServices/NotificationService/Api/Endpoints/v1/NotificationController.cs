using System.ComponentModel.DataAnnotations;
using CIS.InternalServices.NotificationService.Contracts.Email;
using CIS.InternalServices.NotificationService.Contracts.Result;
using CIS.InternalServices.NotificationService.Contracts.Result.Dto.Abstraction;
using CIS.InternalServices.NotificationService.Contracts.Sms;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v1;

[Authorize]
[ApiController]
[Route("v1/notification")]
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
    [SwaggerOperation(Tags = new[] { "Notification Business Case" })]
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
    [SwaggerOperation(Tags = new[] { "Notification Business Case" })]
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
    [SwaggerOperation(Tags = new[] { "Notification Business Case" })]
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
    [SwaggerOperation(Tags = new[] { "Notification Business Case" })]
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
    [SwaggerOperation(Tags = new[] { "Notification Business Case" })]
    [ProducesResponseType(typeof(ResultGetResponse), StatusCodes.Status200OK)]
    public async Task<ResultGetResponse> GetResult([Required] Guid id, CancellationToken token)
        => await _mediator.Send(new ResultGetRequest { NotificationId = id }, token);

    /// <summary>
    /// Vyhledat výsledky o notifikací podle vyhledávacích kritérií
    /// </summary>
    /// <remarks>
    /// Specs: <a target="_blank" href="https://wiki.kb.cz/display/HT/Notification+Service">https://wiki.kb.cz/display/HT/Notification+Service</a>
    /// </remarks>
    [HttpGet("result/search")]
    [SwaggerOperation(Tags = new[] { "Notification Business Case" })]
    [ProducesResponseType(typeof(List<Result>), StatusCodes.Status200OK)]
    public async Task<List<Result>> SearchResults([FromQuery] string identity, [FromQuery] string identityScheme,
        [FromQuery] string customId, [FromQuery] string documentId, CancellationToken token)
    {
        var response = await _mediator.Send(new ResultsSearchByRequest
        {
            Identity = identity,
            IdentityScheme = identityScheme,
            CustomId = customId,
            DocumentId = documentId
        }, token);

        return response.Results;
    }
    
}