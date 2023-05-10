using _C4M = DomainServices.RiskIntegrationService.ExternalServices.LoanApplication.V3.Contracts;

namespace DomainServices.RiskIntegrationService.ExternalServices.LoanApplication.V3;

public interface ILoanApplicationClient
    : ILoanApplicationClientBase
{
    Task<_C4M.LoanApplicationResult> Save(_C4M.LoanApplicationRequest request, CancellationToken cancellationToken);

    const string Version = "V3";
}
