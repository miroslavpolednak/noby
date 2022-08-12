using _C4M = DomainServices.RiskIntegrationService.Api.Clients.LoanApplication.V1.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Clients.LoanApplication.V1;

internal sealed class MockLoanApplicationClient
    : ILoanApplicationClient
{
    public Task<_C4M.LoanApplicationResult> Save(_C4M.LoanApplication request, CancellationToken cancellationToken)
    {
        return Task.FromResult<_C4M.LoanApplicationResult>(new _C4M.LoanApplicationResult
        {
            Id = null,
            LoanApplicationDataVersion = "1"
        });
    }
}
