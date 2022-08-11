using CIS.Core.Results;
using CIS.Core.Types;
using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Abstraction;

public interface ICaseServiceAbstraction
{
    /// <summary>
    /// Vytvoreni Case
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult{TModel}">SuccessfulServiceCallResult&lt;long&gt;</see> (CaseId)</returns>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13002; ProductTypeId must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13003; CaseOwnerUserId must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13004; Unable to get CaseId from SB</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13018; Target amount must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13012; Customer Name must not be empty</exception>
    /// <exception cref="CIS.Core.Exceptions.CisAlreadyExistsException">Code: 13015; Case #{} already exists</exception>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 13022; User not found: {}</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: {SB error key}; {SB error message}</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task<IServiceCallResult> CreateCase(CreateCaseRequest model, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Vraci pocet CASE pro daneho uzivatele v jednotlivych stavech
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult{TModel}">SuccessfulServiceCallResult&lt;GetCaseCountsResponse&gt;</see></returns>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13003; CaseOwnerUserId must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task<IServiceCallResult> GetCaseCounts(int caseOwnerUserId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Vraci detail Case
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult{}"/> of type <see cref="Case" /></returns>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 13000; Case #{} not found</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13016; CaseId must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task<IServiceCallResult> GetCaseDetail(long caseId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Seznam Case pro uzivatele
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult{}"/> of type <see cref="SearchCasesResponse" /></returns>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13003; CaseOwnerUserId must be > 0</exception>
    Task<IServiceCallResult> SearchCases(IPaginableRequest pagination, int userId, List<int>? states = null, string? searchTerm = null, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Zmena majitele Case
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult"/></returns>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 13000; Case #{} not found</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13003; CaseOwnerUserId must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13016; CaseId must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 13022; User not found: {}</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task<IServiceCallResult> LinkOwnerToCase(long caseId, int ownerUserId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Update zakladnich udaju Case - cislo smlouvy
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult"/></returns>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 13000; Case #{} not found</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13002; ProductTypeId must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13018; Target amount must be between 20_000 and 99_999_999</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13014; ProductTypeId {} not found</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13016; CaseId must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task<IServiceCallResult> UpdateCaseData(long caseId, CaseData data, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Update stavu Case
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult"/></returns>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 13000; Case #{} not found</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13005; Case state already set to the same value</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13006; Case state change not allowed</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13011; Case State {} does not exists</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13016; CaseId must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13017; Case State must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task<IServiceCallResult> UpdateCaseState(long caseId, int state, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Update infa o klientovi na case
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult"/></returns>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 13000; Case #{} not found</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13012; Customer Name must not be empty</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13016; CaseId must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task<IServiceCallResult> UpdateCaseCustomer(long caseId, CustomerData customer, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Smazani case
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult"/></returns>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 13000; Case #{} not found</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13016; CaseId must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13021; Unable to delete Case – one or more SalesArrangements exists for this case</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task<IServiceCallResult> DeleteCase(long caseId, CancellationToken cancellationToken = default(CancellationToken));


    /// <summary>
    /// Ziskani sezanmu ukolu a podukolu
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult"/></returns>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 13000; Case #{} not found</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13008; Found tasks [{taskIds}] with invalid TypeId [{invalidTypeIds}]. + Found tasks [{taskIds}] with invalid StateId [{invalidStateIds)}].</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13016; CaseId must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task<IServiceCallResult> GetTaskList(long caseId, CancellationToken cancellationToken = default(CancellationToken));
}
