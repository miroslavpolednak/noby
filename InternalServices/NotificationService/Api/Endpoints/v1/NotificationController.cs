using System.ComponentModel.DataAnnotations;
using CIS.InternalServices.NotificationService.LegacyContracts.Email;
using CIS.InternalServices.NotificationService.LegacyContracts.Result;
using CIS.InternalServices.NotificationService.LegacyContracts.Sms;
using CIS.InternalServices.NotificationService.LegacyContracts.Statistics;
using CIS.InternalServices.NotificationService.LegacyContracts.Resend;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Asp.Versioning;
using Microsoft.AspNetCore.RateLimiting;

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
    [ApiVersion("1", Deprecated = true)]
    [Obsolete("Replaced with v2")]
    [AuditLog]
    [HttpPost("sms")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Notification Business Case" })]
    [ProducesResponseType(typeof(SendSmsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<SendSmsResponse> SendSms([FromBody] SendSmsRequest request, CancellationToken token)
        => await _mediator.Send(request, token);

    /// <summary>
    /// Odeslat email notifikaci
    /// </summary>
    /// <remarks>
    /// Specs: <a target="_blank" href="https://wiki.kb.cz/display/HT/Notification+Service">https://wiki.kb.cz/display/HT/Notification+Service</a>
    /// </remarks>
    [ApiVersion("1", Deprecated = true)]
    [Obsolete("Replaced with v2")]
    [HttpPost("email")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Notification Business Case" })]
    [ProducesResponseType(typeof(SendEmailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<SendEmailResponse> SendEmail([FromBody] SendEmailRequest request, CancellationToken token)
        => await _mediator.Send(request, token);

    /// <summary>
    /// Získat výsledek o notifikaci
    /// </summary>
    /// <remarks>
    /// Specs: <a target="_blank" href="https://wiki.kb.cz/display/HT/Notification+Service">https://wiki.kb.cz/display/HT/Notification+Service</a>
    /// </remarks>
    [ApiVersion("1", Deprecated = true)]
    [Obsolete("Replaced with v2")]
    [HttpGet("result/{id}")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Notification Business Case" })]
    [ProducesResponseType(typeof(GetResultResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<LegacyContracts.Result.Dto.Result> GetResult([Required] Guid id, CancellationToken token)
    {
        var response = await _mediator.Send(new GetResultRequest
        {
            NotificationId = id
        }, token);
        return response.Result;
    }

    /// <summary>
    /// Vyhledat výsledky o notifikací podle vyhledávacích kritérií
    /// </summary>
    /// <remarks>
    /// Specs: <a target="_blank" href="https://wiki.kb.cz/display/HT/Notification+Service">https://wiki.kb.cz/display/HT/Notification+Service</a>
    /// </remarks>
    [ApiVersion("1", Deprecated = true)]
    [Obsolete("Replaced with v2")]
    [HttpGet("result/search")]
    [SwaggerOperation(Tags = new[] { "Notification Business Case" })]
    [ProducesResponseType(typeof(List<LegacyContracts.Result.Dto.Result>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [EnableRateLimiting("fixed")]
    public async Task<List<LegacyContracts.Result.Dto.Result>> SearchResults([FromQuery] string? identity, [FromQuery] string? identityScheme,
        [FromQuery] long? caseId, [FromQuery] string? customId, [FromQuery] string? documentId, CancellationToken token)
    {
        var response = await _mediator.Send(new SearchResultsRequest
        {
            Identity = identity,
            IdentityScheme = identityScheme,
            CaseId = caseId,
            CustomId = customId,
            DocumentId = documentId
        }, token);

        return response.Results;
    }

    /// <summary>
    /// Zobrazit statistiky podle zadaných kritérií
    /// </summary>
    /// <remarks>
    /// Specs: <a target="_blank" href="https://wiki.kb.cz/display/HT/Notification+Service">https://wiki.kb.cz/display/HT/Notification+Service</a>
    /// </remarks>
    [ApiVersion("1", Deprecated = true)]
    [Obsolete("Replaced with v2")]
    [HttpGet("result/statistics")]
    [SwaggerOperation(Tags = new[] { "Notification Business Case" })]
    [ProducesResponseType(typeof(GetStatisticsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<GetStatisticsResponse> GetStatistics([FromQuery] GetStatisticsRequest getStatisticsRequest, CancellationToken token)
        => await _mediator.Send(getStatisticsRequest, token);

    /// <summary>
    /// Zobrazit detailní statistiky podle zadaných kritérií
    /// </summary>
    /// <remarks>
    /// Specs: <a target="_blank" href="https://wiki.kb.cz/display/HT/Notification+Service">https://wiki.kb.cz/display/HT/Notification+Service</a>
    /// </remarks>
    [HttpGet("result/detailed-statistics")]
    [Obsolete("Replaced with v2")]
    [SwaggerOperation(Tags = new[] { "Notification Business Case" })]
    [ProducesResponseType(typeof(GetDetailedStatisticsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<GetDetailedStatisticsResponse> GetDetailedStatistics([FromQuery] GetDetailedStatisticsRequest getDetailedStatisticsRequest, CancellationToken token)
        => await _mediator.Send(getDetailedStatisticsRequest, token);

    /// <summary>
    /// Znovu odešle notifikaci
    /// </summary>
    /// <remarks>
    /// Specs: <a target="_blank" href="https://wiki.kb.cz/display/HT/Notification+Service">https://wiki.kb.cz/display/HT/Notification+Service</a>
    /// </remarks>
    [ApiVersion("1", Deprecated = true)]
    [Obsolete("Replaced with v2")]
    [HttpGet("resend/{id}")]
    [SwaggerOperation(Tags = new[] { "Notification Business Case" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task Resend([Required] Guid id, CancellationToken token)
        => await _mediator.Send(new ResendRequest { NotificationId = id }, token);

}