namespace DomainServices.RiskIntegrationService.Abstraction.LoanApplication.V2;

public interface ILoanApplicationServiceAbstraction
{
    /// <summary>
    /// Založí LoanApplication
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult{}"/> of type <see cref="Contracts.LoanApplication.V2.LoanApplicationSaveResponse" /></returns>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task<IServiceCallResult> Save(Contracts.LoanApplication.V2.LoanApplicationSaveRequest request, CancellationToken cancellationToken = default(CancellationToken));
}
