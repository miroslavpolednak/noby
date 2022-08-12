namespace DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;

[ServiceContract(Name = "DomainServices.RiskIntegrationService.LoanApplicationService.V2")]
public interface ILoanApplicationService
{
    ValueTask<LoanApplicationSaveResponse> Save(LoanApplicationSaveRequest request, CancellationToken cancellationToken = default);
}
