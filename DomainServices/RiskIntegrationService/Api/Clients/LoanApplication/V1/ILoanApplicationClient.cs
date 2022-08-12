using _C4M = DomainServices.RiskIntegrationService.Api.Clients.LoanApplication.V1.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Clients.LoanApplication.V1;

internal interface ILoanApplicationClient
{
    Task<_C4M.LoanApplicationResult> Save(_C4M.LoanApplication request, CancellationToken cancellationToken);
}
