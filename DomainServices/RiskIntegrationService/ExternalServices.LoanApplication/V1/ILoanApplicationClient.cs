using CIS.Infrastructure.ExternalServicesHelpers;
using _C4M = DomainServices.RiskIntegrationService.ExternalServices.LoanApplication.V1.Contracts;

namespace DomainServices.RiskIntegrationService.ExternalServices.LoanApplication.V1;

internal interface ILoanApplicationClient
    : IExternalServiceClient
{
    Task<_C4M.LoanApplicationResult> Save(_C4M.LoanApplication request, CancellationToken cancellationToken);

    const string Version = "V1";
}
