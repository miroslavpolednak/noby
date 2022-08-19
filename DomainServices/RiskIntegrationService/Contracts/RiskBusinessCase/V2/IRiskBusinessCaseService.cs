using _V2 = DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;
using _shared = DomainServices.RiskIntegrationService.Contracts.Shared.V1;

[ServiceContract(Name = "DomainServices.RiskIntegrationService.RiskBusinessCaseService.V2")]
public interface IRiskBusinessCaseService
{
    ValueTask<_V2.CreateCaseResponse> CreateCase(_V2.CreateCaseRequest request, CancellationToken cancellationToken = default);

    ValueTask<_shared.LoanApplicationAssessmentResponse> CreateAssesment(_V2.CreateAssesmentRequest request, CancellationToken cancellationToken = default);
}
