using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.RiskBusinessCase.V1;

[Authorize]
[ApiController]
[Route("api/risk-business-case/v1")]
public class RiskBusinessCaseServiceController
    : ControllerBase
{
    private readonly IMediator _mediator;

    public RiskBusinessCaseServiceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Risk Business Case - Create
    /// </summary>
    [HttpPost()]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: Risk business case" })]
    [ProducesResponseType(typeof(Contracts.RiskBusinessCase.CreateCaseResponse), StatusCodes.Status200OK)]
    public async Task<Contracts.RiskBusinessCase.CreateCaseResponse> CreateCase([FromBody] Contracts.RiskBusinessCase.CreateCaseRequest request, CancellationToken cancellationToken)
        => await _mediator.Send(request, cancellationToken);

    [HttpPost("commit")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: Risk business case" })]
    [ProducesResponseType(typeof(Contracts.RiskBusinessCase.CaseCommitmentResponse), StatusCodes.Status200OK)]
    public async Task<Contracts.RiskBusinessCase.CaseCommitmentResponse> CaseCommitment([FromBody] Contracts.RiskBusinessCase.CaseCommitmentRequest request, CancellationToken cancellationToken)
        => await _mediator.Send(request, cancellationToken);
}
