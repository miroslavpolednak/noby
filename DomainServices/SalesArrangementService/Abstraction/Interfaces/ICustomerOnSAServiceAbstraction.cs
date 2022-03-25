using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Abstraction;

public interface ICustomerOnSAServiceAbstraction
{
    /// <summary>
    /// Vytvoreni noveho klienta
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult{}"/> of type <see cref="int" /> (CustomerOnSAId)</returns>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16021; CustomerRoleId does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16000; Sales arrangement ID does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.ServiceUnavailableException">SalesArrangement service unavailable</exception>
    Task<IServiceCallResult> CreateCustomer(CreateCustomerRequest request, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Smazani klienta
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult"/></returns>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16020; CustomerOnSA ID does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.ServiceUnavailableException">SalesArrangement service unavailable</exception>
    Task<IServiceCallResult> DeleteCustomer(int customerOnSAId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Vraci instanci vybraneho klienta
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult{}"/> of type <see cref="CustomerOnSA" /></returns>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16020; CustomerOnSA ID does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.ServiceUnavailableException">SalesArrangement service unavailable</exception>
    Task<IServiceCallResult> GetCustomer(int customerOnSAId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Vraci seznam instanci klientu dle SA Id
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult{}"/> of type <see cref="List{}" /> where T : <see cref="CustomerOnSA" /></returns>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16000; Sales arrangement ID does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.ServiceUnavailableException">SalesArrangement service unavailable</exception>
    Task<IServiceCallResult> GetCustomerList(int salesArrangementId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Update dat o customerovi
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult"/></returns>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16020; CustomerOnSA ID does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.ServiceUnavailableException">SalesArrangement service unavailable</exception>
    Task<IServiceCallResult> UpdateCustomer(UpdateCustomerRequest request, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Update zavazku customera
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult"/></returns>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16020; CustomerOnSA ID does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.ServiceUnavailableException">SalesArrangement service unavailable</exception>
    Task<IServiceCallResult> UpdateObligations(UpdateObligationsRequest request, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Vytvoreni noveho prijmu
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult{}"/> of type <see cref="int" /> (IncomeId)</returns>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16024; CustomerOnSAId must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16028; IncomeTypeId must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.ServiceUnavailableException">SalesArrangement service unavailable</exception>
    Task<IServiceCallResult> CreateIncome(CreateIncomeRequest request, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Smazani prijmu
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult"/></returns>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16029; Income ID {IncomeId} does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.ServiceUnavailableException">SalesArrangement service unavailable</exception>
    Task<IServiceCallResult> DeleteIncome(int incomeId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Vraci instanci pozadovaneho prijmu
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult{}"/> of type <see cref="Income" /></returns>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16029; Income ID {request.IncomeId} does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.ServiceUnavailableException">SalesArrangement service unavailable</exception>
    Task<IServiceCallResult> GetIncome(int incomeId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Vraci vsechny prijmy pro daneho Customera
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult{}"/> of type <see cref="List{}" /> where T : <see cref="IncomeInList" /></returns>
    /// <exception cref="CIS.Core.Exceptions.ServiceUnavailableException">SalesArrangement service unavailable</exception>
    Task<IServiceCallResult> GetIncomeList(int customerOnSAId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Update detailu dat o prijmu
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult"/></returns>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16029; IncomeId must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.ServiceUnavailableException">SalesArrangement service unavailable</exception>
    Task<IServiceCallResult> UpdateIncome(UpdateIncomeRequest request, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Update zakladnich dat o prijmu
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult"/></returns>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16029; IncomeId must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.ServiceUnavailableException">SalesArrangement service unavailable</exception>
    Task<IServiceCallResult> UpdateIncomeBaseData(UpdateIncomeBaseDataRequest request, CancellationToken cancellationToken = default(CancellationToken));
}