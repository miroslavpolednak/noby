using DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.RiskBusinessCase.V2;

[Authorize]
[ApiController]
[Route("v2/risk-business-case")]
public sealed class RiskBusinessCaseServiceController
    : ControllerBase
{
    private readonly IMediator _mediator;

    public RiskBusinessCaseServiceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Založení úvěrového případu
    /// </summary>
    /// <remarks>
    /// Specs: <a target="_blank" href="https://wiki.kb.cz/display/HT/RISK+BUSINESS+CASE+SERVICE">https://wiki.kb.cz/display/HT/RISK+BUSINESS+CASE+SERVICE</a>
    /// </remarks>
    [HttpPost()]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: Risk business case" })]
    [ProducesResponseType(typeof(CreateCaseResponse), StatusCodes.Status200OK)]
    public async Task<CreateCaseResponse> CreateCase([FromBody] CreateCaseRequest request, CancellationToken cancellationToken)
        => await _mediator.Send(request, cancellationToken);

    //[HttpPost("commit")]
    //[Produces("application/json")]
    //[SwaggerOperation(Tags = new[] { "UC: Risk business case" })]
    //[ProducesResponseType(typeof(Contracts.RiskBusinessCase.CaseCommitmentResponse), StatusCodes.Status200OK)]
    //public async Task<Contracts.RiskBusinessCase.CaseCommitmentResponse> CaseCommitment([FromBody] Contracts.RiskBusinessCase.CaseCommitmentRequest request, CancellationToken cancellationToken)
    //    => await _mediator.Send(request, cancellationToken);
}
