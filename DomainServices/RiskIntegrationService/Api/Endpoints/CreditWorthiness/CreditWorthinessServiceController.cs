using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness;

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
    [ProducesResponseType(typeof(Contracts.CreditWorthiness.CreditWorthinessCalculateResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<Contracts.CreditWorthiness.CreditWorthinessCalculateResponse> Calculate([FromBody] Contracts.CreditWorthiness.CreditWorthinessCalculateRequest request, CancellationToken cancellationToken)
        => await _mediator.Send(request, cancellationToken);
}
