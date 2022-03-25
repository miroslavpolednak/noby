using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Abstraction;

public interface ICustomerOnSAServiceAbstraction
{
    /// <summary>
    /// Vytvoreni noveho klienta
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[int (CustomerOnSAId)] - OK;
    /// </returns>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16021; CustomerRoleId does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16000; Sales arrangement ID does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.ServiceUnavailableException">SalesArrangement service unavailable</exception>
    Task<IServiceCallResult> CreateCustomer(CreateCustomerRequest request, CancellationToken cancellationToken = default(CancellationToken));
    
    /// <summary>
    /// Smazani klienta
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult - OK;
    /// </returns>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16020; CustomerOnSA ID does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.ServiceUnavailableException">SalesArrangement service unavailable</exception>
    Task<IServiceCallResult> DeleteCustomer(int customerOnSAId, CancellationToken cancellationToken = default(CancellationToken));
    
    /// <summary>
    /// Vraci instanci vybraneho klienta
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[CustomerOnSA] - OK;
    /// </returns>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16020; CustomerOnSA ID does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.ServiceUnavailableException">SalesArrangement service unavailable</exception>
    Task<IServiceCallResult> GetCustomer(int customerOnSAId, CancellationToken cancellationToken = default(CancellationToken));
    
    /// <summary>
    /// Vraci seznam instanci klientu dle SA Id
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[List[CustomerOnSA]] - OK;
    /// </returns>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16000; Sales arrangement ID does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.ServiceUnavailableException">SalesArrangement service unavailable</exception>
    Task<IServiceCallResult> GetCustomerList(int salesArrangementId, CancellationToken cancellationToken = default(CancellationToken));
    
    /// <summary>
    /// Update dat o customerovi
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult - OK;
    /// </returns>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16020; CustomerOnSA ID does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.ServiceUnavailableException">SalesArrangement service unavailable</exception>
    Task<IServiceCallResult> UpdateCustomer(UpdateCustomerRequest request, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Update zavazku customera
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult - OK;
    /// </returns>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16020; CustomerOnSA ID does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.ServiceUnavailableException">SalesArrangement service unavailable</exception>
    Task<IServiceCallResult> UpdateObligations(UpdateObligationsRequest request, CancellationToken cancellationToken = default(CancellationToken));
}