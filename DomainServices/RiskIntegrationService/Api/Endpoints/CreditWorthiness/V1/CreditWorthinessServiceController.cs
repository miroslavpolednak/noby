using _V1 = DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V1;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness.V1;

[Authorize]
[ApiController]
[Route("v1/credit-worthiness")]
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
    [HttpPost()]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: Credit Worthiness" })]
    [ProducesResponseType(typeof(_V1.CreditWorthinessCalculateResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<_V1.CreditWorthinessCalculateResponse> Calculate([FromBody] _V1.CreditWorthinessCalculateRequest request, CancellationToken cancellationToken)
        => await _mediator.Send(request, cancellationToken);
}
