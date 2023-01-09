using System.ComponentModel.DataAnnotations;
using CIS.InternalServices.NotificationService.Contracts.Email;
using CIS.InternalServices.NotificationService.Contracts.Result;
using CIS.InternalServices.NotificationService.Contracts.Result.Dto;
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
    [ProducesResponseType(typeof(SendSmsResponse), StatusCodes.Status200OK)]
    public async Task<SendSmsResponse> SendSms([FromBody] SendSmsRequest request, CancellationToken token)
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
    [ProducesResponseType(typeof(SendSmsFromTemplateResponse), StatusCodes.Status200OK)]
    public async Task<SendSmsFromTemplateResponse> SendSmsFromTemplate([FromBody] SendSmsFromTemplateRequest request, CancellationToken token)
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
    [ProducesResponseType(typeof(SendEmailResponse), StatusCodes.Status200OK)]
    public async Task<SendEmailResponse> SendEmail([FromBody] SendEmailRequest request, CancellationToken token)
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
    [ProducesResponseType(typeof(SendEmailFromTemplateResponse), StatusCodes.Status200OK)]
    public async Task<SendEmailFromTemplateResponse> SendEmailFromTemplate([FromBody] SendEmailFromTemplateRequest request, CancellationToken token)
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
    [ProducesResponseType(typeof(GetResultResponse), StatusCodes.Status200OK)]
    public async Task<GetResultResponse> GetResult([Required] Guid id, CancellationToken token)
        => await _mediator.Send(new GetResultRequest { NotificationId = id }, token);

    /// <summary>
    /// Vyhledat výsledky o notifikací podle vyhledávacích kritérií
    /// </summary>
    /// <remarks>
    /// Specs: <a target="_blank" href="https://wiki.kb.cz/display/HT/Notification+Service">https://wiki.kb.cz/display/HT/Notification+Service</a>
    /// </remarks>
    [HttpGet("result/search")]
    [SwaggerOperation(Tags = new[] { "Notification Business Case" })]
    [ProducesResponseType(typeof(List<Result>), StatusCodes.Status200OK)]
    public async Task<List<Result>> SearchResults([FromQuery] string? identity, [FromQuery] string? identityScheme,
        [FromQuery] string? customId, [FromQuery] string? documentId, CancellationToken token)
    {
        var response = await _mediator.Send(new SearchResultsRequest
        {
            Identity = identity,
            IdentityScheme = identityScheme,
            CustomId = customId,
            DocumentId = documentId
        }, token);

        return response.Results;
    }
    
}