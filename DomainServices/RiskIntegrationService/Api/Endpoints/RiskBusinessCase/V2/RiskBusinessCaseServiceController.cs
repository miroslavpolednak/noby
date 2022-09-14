using _V2 = DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;
using _sh = DomainServices.RiskIntegrationService.Contracts.Shared.V1;
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
    [SwaggerOperation(Tags = new[] { "UC: Risk Business Case" })]
    [ProducesResponseType(typeof(_V2.RiskBusinessCaseCreateResponse), StatusCodes.Status200OK)]
    public async Task<_V2.RiskBusinessCaseCreateResponse> CreateCase([FromBody] _V2.RiskBusinessCaseCreateRequest request, CancellationToken cancellationToken)
        => await _mediator.Send(request, cancellationToken);

    /// <summary>
    /// Žádost o vyhodnocení úvěrové žádosti
    /// </summary>
    /// <remarks>
    /// Specs: <a target="_blank" href="https://wiki.kb.cz/display/HT/RISK+BUSINESS+CASE+SERVICE">https://wiki.kb.cz/display/HT/RISK+BUSINESS+CASE+SERVICE</a>
    /// </remarks>
    [HttpPost("assessment")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: Risk Business Case" })]
    [ProducesResponseType(typeof(_sh.LoanApplicationAssessmentResponse), StatusCodes.Status200OK)]
    public async Task<_sh.LoanApplicationAssessmentResponse> CreateAssessment([FromBody] _V2.RiskBusinessCaseCreateAssessmentRequest request, CancellationToken cancellationToken)
        => await _mediator.Send(request, cancellationToken);

    ///// <summary>
    ///// Žádost o vyhodnocení úvěrové žádosti - asynchronní
    ///// </summary>
    ///// <remarks>
    ///// Specs: <a target="_blank" href="https://wiki.kb.cz/display/HT/RISK+BUSINESS+CASE+SERVICE">https://wiki.kb.cz/display/HT/RISK+BUSINESS+CASE+SERVICE</a>
    ///// </remarks>
    //[HttpPost("assessment-asynchronous")]
    //[Produces("application/json")]
    //[SwaggerOperation(Tags = new[] { "UC: Risk Business Case" })]
    //[ProducesResponseType(typeof(_V2.RiskBusinessCaseCreateAssessmentAsynchronousResponse), StatusCodes.Status200OK)]
    //public async Task<_V2.RiskBusinessCaseCreateAssessmentAsynchronousResponse> CreateAssessmentAsynchronous([FromBody] _V2.RiskBusinessCaseCreateAssessmentAsynchronousRequest request, CancellationToken cancellationToken)
    //    => await _mediator.Send(request, cancellationToken);

    /// <summary>
    /// Dokončení úvěrové žádosti
    /// </summary>
    /// <remarks>
    /// Specs: <a target="_blank" href="https://wiki.kb.cz/display/HT/RISK+BUSINESS+CASE+SERVICE">https://wiki.kb.cz/display/HT/RISK+BUSINESS+CASE+SERVICE</a>
    /// </remarks>
    [HttpPost("commitment")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: Risk Business Case" })]
    [ProducesResponseType(typeof(_V2.RiskBusinessCaseCommitCaseResponse), StatusCodes.Status200OK)]
    public async Task<_V2.RiskBusinessCaseCommitCaseResponse> CommitCase([FromBody] _V2.RiskBusinessCaseCommitCaseRequest request, CancellationToken cancellationToken)
        => await _mediator.Send(request, cancellationToken);

    /// <summary>
    /// Získání výsledků vyhodnocení
    /// </summary>
    /// <remarks>
    /// Specs: <a target="_blank" href="https://wiki.kb.cz/display/HT/LOAN+APPLICATION+ASSESSMENT">https://wiki.kb.cz/display/HT/LOAN+APPLICATION+ASSESSMENT</a>
    /// </remarks>
    [HttpPost("assessment-detail")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: Risk Business Case" })]
    [ProducesResponseType(typeof(_sh.LoanApplicationAssessmentResponse), StatusCodes.Status200OK)]
    public async Task<_sh.LoanApplicationAssessmentResponse> GetAssessment([FromBody] _V2.RiskBusinessCaseGetAssessmentRequest request, CancellationToken cancellationToken)
        => await _mediator.Send(request, cancellationToken);
}
