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
    /// Výpoèet rozšíøené bonity
    /// </summary>
    [HttpPost()]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: Credit Worthiness" })]
    [ProducesResponseType(typeof(Contracts.CreditWorthiness.CalculateResponse), StatusCodes.Status200OK)]
    public async Task<Contracts.CreditWorthiness.CalculateResponse> Calculate([FromBody] Contracts.CreditWorthiness.CalculateRequest request, CancellationToken cancellationToken)
        => await _mediator.Send(request, cancellationToken);
}
