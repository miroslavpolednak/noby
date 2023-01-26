namespace DomainServices.RiskIntegrationService.Clients.LoanApplication.V2;

public interface ILoanApplicationServiceClient
{
    /// <summary>
    /// Založí LoanApplication
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task<string> Save(Contracts.LoanApplication.V2.LoanApplicationSaveRequest request, CancellationToken cancellationToken = default(CancellationToken));
}
