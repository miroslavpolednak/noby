using DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.RiskBusinessCase.V1;

[Authorize]
public class RiskBusinessCaseService
    : v1.IRiskBusinessCaseService
{
    private readonly IMediator _mediator;

    public RiskBusinessCaseService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async ValueTask<CaseCommitmentResponse> CaseCommitment(CaseCommitmentRequest request, CancellationToken cancellationToken = default)
        => await _mediator.Send(request, cancellationToken);

    public async ValueTask<CreateCaseResponse> CreateCase(CreateCaseRequest request, CancellationToken cancellationToken = default)
        => await _mediator.Send(request, cancellationToken);
}
