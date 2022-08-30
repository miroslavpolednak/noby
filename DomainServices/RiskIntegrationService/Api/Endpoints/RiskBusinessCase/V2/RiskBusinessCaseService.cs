using _V2 = DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;
using _sh = DomainServices.RiskIntegrationService.Contracts.Shared.V1;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.RiskBusinessCase.V2;

[Authorize]
public class RiskBusinessCaseService
    : IRiskBusinessCaseService
{
    private readonly IMediator _mediator;

    public RiskBusinessCaseService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async ValueTask<_V2.RiskBusinessCaseCreateResponse> CreateCase(_V2.RiskBusinessCaseCreateRequest request, CancellationToken cancellationToken = default)
        => await _mediator.Send(request, cancellationToken);

    public async ValueTask<_sh.LoanApplicationAssessmentResponse> CreateAssesment(_V2.RiskBusinessCaseCreateAssesmentRequest request, CancellationToken cancellationToken = default)
        => await _mediator.Send(request, cancellationToken);

    public async ValueTask<_V2.RiskBusinessCaseCreateAssesmentAsynchronousResponse> CreateAssesmentAsynchronous(_V2.RiskBusinessCaseCreateAssesmentAsynchronousRequest request, CancellationToken cancellationToken = default)
        => await _mediator.Send(request, cancellationToken);

    public async ValueTask<_V2.RiskBusinessCaseCommitCaseResponse> CommitCase(_V2.RiskBusinessCaseCommitCaseRequest request, CancellationToken cancellationToken = default)
        => await _mediator.Send(request, cancellationToken);

    public async ValueTask<_sh.LoanApplicationAssessmentResponse> GetAssesment(_V2.RiskBusinessCaseGetAssesmentRequest request, CancellationToken cancellationToken = default)
        => await _mediator.Send(request, cancellationToken);
}
