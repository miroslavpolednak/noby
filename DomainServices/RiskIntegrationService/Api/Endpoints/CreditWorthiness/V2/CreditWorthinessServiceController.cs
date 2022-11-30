using _V2 = DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness.V2;

[Authorize]
[ApiController]
[Route("v2")]
public sealed class CreditWorthinessServiceController
    : ControllerBase
{
    private readonly IMediator _mediator;

    public CreditWorthinessServiceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Výpočet rozšířené bonity
    /// </summary>
    /// <remarks>
    /// Specs: <a target="_blank" href="https://wiki.kb.cz/confluence/display/HT/CREDIT+WORTHINESS+SERVICE">https://wiki.kb.cz/confluence/display/HT/CREDIT+WORTHINESS+SERVICE</a>
    /// </remarks>
    [HttpPost("credit-worthiness")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: Credit Worthiness" })]
    [ProducesResponseType(typeof(_V2.CreditWorthinessCalculateResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<_V2.CreditWorthinessCalculateResponse> Calculate([FromBody] _V2.CreditWorthinessCalculateRequest request, CancellationToken cancellationToken)
        => await _mediator.Send(request, cancellationToken);

    /// <summary>
    /// Výpočet zjednodušené bonity
    /// </summary>
    /// <remarks>
    /// Specs: <a target="_blank" href="https://wiki.kb.cz/display/HT/CREDIT+WORTHINESS+SERVICE+-+SIMPLE+v2">https://wiki.kb.cz/display/HT/CREDIT+WORTHINESS+SERVICE+-+SIMPLE+v2</a>
    /// </remarks>
    [HttpPost("credit-worthiness-simple")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: Credit Worthiness" })]
    [ProducesResponseType(typeof(_V2.CreditWorthinessSimpleCalculateResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<_V2.CreditWorthinessSimpleCalculateResponse> SimpleCalculate([FromBody] _V2.CreditWorthinessSimpleCalculateRequest request, CancellationToken cancellationToken)
        => await _mediator.Send(request, cancellationToken);
}
