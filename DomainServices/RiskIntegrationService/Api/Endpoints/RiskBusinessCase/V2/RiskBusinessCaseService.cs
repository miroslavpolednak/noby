using DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;
using DomainServices.RiskIntegrationService.Contracts.Shared.V1;
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

    public async ValueTask<CreateCaseResponse> CreateCase(CreateCaseRequest request, CancellationToken cancellationToken = default)
        => await _mediator.Send(request, cancellationToken);

    public async ValueTask<LoanApplicationAssessmentResponse> CreateAssesment(CreateAssesmentRequest request, CancellationToken cancellationToken = default)
        => await _mediator.Send(request, cancellationToken);

    //public async ValueTask<CaseCommitmentResponse> CaseCommitment(CaseCommitmentRequest request, CancellationToken cancellationToken = default)
    //    => await _mediator.Send(request, cancellationToken);
}
