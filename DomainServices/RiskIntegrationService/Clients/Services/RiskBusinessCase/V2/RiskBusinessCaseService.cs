using CIS.Infrastructure.Logging;
using DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;
using DomainServices.RiskIntegrationService.Contracts.Shared.V1;

namespace DomainServices.RiskIntegrationService.Clients.Services.RiskBusinessCase.V2;

internal sealed class RiskBusinessCaseService
    : Clients.RiskBusinessCase.V2.IRiskBusinessCaseServiceClient
{
    public async Task<RiskBusinessCaseCreateResponse> CreateCase(long salesArrangementId, string? resourceProcessId, CancellationToken cancellationToken = default)
    {
        return await _service.CreateCase(new RiskBusinessCaseCreateRequest
        {
            SalesArrangementId = salesArrangementId,
            ResourceProcessId = resourceProcessId
        }, cancellationToken: cancellationToken);
    }

    public async Task<LoanApplicationAssessmentResponse> CreateAssessment(RiskBusinessCaseCreateAssessmentRequest request, CancellationToken cancellationToken = default)
    {
        return await _service.CreateAssessment(request, cancellationToken: cancellationToken);
    }

    public async Task<LoanApplicationAssessmentResponse> GetAssessment(RiskBusinessCaseGetAssessmentRequest request, CancellationToken cancellationToken = default)
    {
        return await _service.GetAssessment(request, cancellationToken: cancellationToken);
    }

    public async Task<RiskBusinessCaseCommitCaseResponse> CommitCase(RiskBusinessCaseCommitCaseRequest request, CancellationToken cancellationToken = default)
    {
        return await _service.CommitCase(request, cancellationToken: cancellationToken);
    }

    private readonly IRiskBusinessCaseService _service;

    public RiskBusinessCaseService(IRiskBusinessCaseService service)
    {
        _service = service;
    }
}
