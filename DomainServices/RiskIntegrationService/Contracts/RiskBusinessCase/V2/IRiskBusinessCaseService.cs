using _V2 = DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;
using _shared = DomainServices.RiskIntegrationService.Contracts.Shared.V1;

namespace DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;

[ServiceContract(Name = "DomainServices.RiskIntegrationService.RiskBusinessCaseService.V2")]
public interface IRiskBusinessCaseService
{
    ValueTask<_V2.RiskBusinessCaseCreateResponse> CreateCase(_V2.RiskBusinessCaseCreateRequest request, CancellationToken cancellationToken = default);

    ValueTask<_shared.LoanApplicationAssessmentResponse> CreateAssessment(_V2.RiskBusinessCaseCreateAssessmentRequest request, CancellationToken cancellationToken = default);

    ValueTask<_V2.RiskBusinessCaseCreateAssessmentAsynchronousResponse> CreateAssessmentAsynchronous(_V2.RiskBusinessCaseCreateAssessmentAsynchronousRequest request, CancellationToken cancellationToken = default);

    ValueTask<_V2.RiskBusinessCaseCommitCaseResponse> CommitCase(_V2.RiskBusinessCaseCommitCaseRequest request, CancellationToken cancellationToken = default);

    ValueTask<_shared.LoanApplicationAssessmentResponse> GetAssessment(_V2.RiskBusinessCaseGetAssessmentRequest request, CancellationToken cancellationToken = default);
}
