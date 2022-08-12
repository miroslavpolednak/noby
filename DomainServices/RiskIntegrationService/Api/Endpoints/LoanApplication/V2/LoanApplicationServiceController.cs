using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using _V2 = DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.LoanApplication.V2;

[Authorize]
[ApiController]
[Route("v2/loan-application")]
public sealed class LoanApplicationServiceController
    : ControllerBase
{
    private readonly IMediator _mediator;

    public LoanApplicationServiceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Založí LoanApplication
    /// </summary>
    /// <remarks>
    /// Specs: <a target="_blank" href="https://wiki.kb.cz/display/HT/LOAN+APPLICATION">https://wiki.kb.cz/display/HT/LOAN+APPLICATION</a>
    /// </remarks>
    [HttpPost()]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: Loan Application" })]
    [ProducesResponseType(typeof(_V2.LoanApplicationSaveResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<_V2.LoanApplicationSaveResponse> Save([FromBody] _V2.LoanApplicationSaveRequest request, CancellationToken cancellationToken)
        => await _mediator.Send(request, cancellationToken);
}
