using _C4M = DomainServices.RiskIntegrationService.ExternalServices.LoanApplication.V3.Contracts;

namespace DomainServices.RiskIntegrationService.ExternalServices.LoanApplication.V3;

internal sealed class MockLoanApplicationClient
    : ILoanApplicationClient
{
    public Task<_C4M.LoanApplicationResult> Save(_C4M.LoanApplicationRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult<_C4M.LoanApplicationResult>(new _C4M.LoanApplicationResult
        {
            Id = null,
        });
    }
}
