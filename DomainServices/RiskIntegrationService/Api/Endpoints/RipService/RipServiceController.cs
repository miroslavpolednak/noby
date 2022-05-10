using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace DomainServices.RiskIntegrationService.Api.Endpoints;

[Authorize]
[ApiController]
[Route("api/testservice")]
public class RipServiceController
    : ControllerBase
{
    private readonly IMediator _mediator;

    public RipServiceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Výpoèet rozšíøené bonity
    /// </summary>
    [HttpPost("credit-worthiness")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: Credit Worthiness" })]
    [ProducesResponseType(typeof(Contracts.CreditWorthinessResponse), StatusCodes.Status200OK)]
    public async Task<Contracts.CreditWorthinessResponse> CreditWorthiness([FromBody] Contracts.CreditWorthinessRequest request, CancellationToken cancellationToken)
        => await _mediator.Send(request, cancellationToken);
}
