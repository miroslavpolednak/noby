using _V2 = DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;
using _shared = DomainServices.RiskIntegrationService.Contracts.Shared.V1;

[ServiceContract(Name = "DomainServices.RiskIntegrationService.RiskBusinessCaseService.V2")]
public interface IRiskBusinessCaseService
{
    ValueTask<_V2.RiskBusinessCaseCreateResponse> CreateCase(_V2.RiskBusinessCaseCreateRequest request, CancellationToken cancellationToken = default);

    ValueTask<_shared.LoanApplicationAssessmentResponse> CreateAssesment(_V2.RiskBusinessCaseCreateAssesmentRequest request, CancellationToken cancellationToken = default);

    ValueTask<_V2.RiskBusinessCaseCreateAssesmentAsynchronousResponse> CreateAssesmentAsynchronous(_V2.RiskBusinessCaseCreateAssesmentAsynchronousRequest request, CancellationToken cancellationToken = default);

    ValueTask<_V2.RiskBusinessCaseCommitCaseResponse> CommitCase(_V2.RiskBusinessCaseCommitCaseRequest request, CancellationToken cancellationToken = default);
}
