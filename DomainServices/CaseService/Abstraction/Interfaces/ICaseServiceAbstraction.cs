using CIS.Core.Results;
using CIS.Core.Types;
using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Abstraction;

public interface ICaseServiceAbstraction
{
    /// <summary>
    /// Vytvoreni CaseInstance
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[long] - OK; returns CaseId;
    /// </returns>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13004; Unable to get CaseId from SB</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13003; Case Owner Id not must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13010; ContractNumber length must be 10</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13018; Target amount must be between 20_000 and 99_999_999</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13012; Customer Name must not be empty</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13013; ProductInstanceTypeId {} is not valid for this operation</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13014; ProductInstanceTypeId {} not found</exception>
    /// <exception cref="CIS.Core.Exceptions.CisAlreadyExistsException">Code: 13015; Case #{} already exists</exception>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 13017; User not found: {}</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: {SB error key}; {SB error message}</exception>
    /// <exception cref="CIS.Core.Exceptions.ServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.ServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task<IServiceCallResult> CreateCase(CreateCaseRequest model, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Vraci pocet CASE pro daneho uzivatele v jednotlivych stavech
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[GetCaseCountsResponse] - OK;
    /// </returns>
    /// <exception cref="CIS.Core.Exceptions.ServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.ServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task<IServiceCallResult> GetCaseCounts(int caseOwnerUserId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Vraci detail Case
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[CaseModel] - OK;
    /// </returns>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 13000; Case #{} not found</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13016; CaseId must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.ServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.ServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task<IServiceCallResult> GetCaseDetail(long caseId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Seznam Case pro uzivatele
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[SearchCasesResponse] - OK;
    /// </returns>
    Task<IServiceCallResult> SearchCases(IPaginableRequest pagination, int userId, int? state = null, string? searchTerm = null, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Zmena majitele Case
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult - OK;
    /// </returns>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 13000; Case #{} not found</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13003; CaseOwnerUserId must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13016; CaseId must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 13017; User not found: {}</exception>
    /// <exception cref="CIS.Core.Exceptions.ServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.ServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task<IServiceCallResult> LinkOwnerToCase(long caseId, int ownerUserId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Update zakladnich udaju Case - cislo smlouvy
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult - OK;
    /// </returns>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 13000; Case #{} not found</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13002; ProductInstanceTypeId must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13010; ContractNumber length must be 10</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13018; Target amount must be between 20_000 and 99_999_999</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13014; ProductInstanceTypeId {} not found</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13016; CaseId must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.ServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.ServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task<IServiceCallResult> UpdateCaseData(long caseId, CaseData data, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Update stavu Case
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult - OK;
    /// </returns>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 13000; Case #{} not found</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13011; Case State {} does not exists</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13016; CaseId must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13017; Case State must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.ServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.ServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task<IServiceCallResult> UpdateCaseState(long caseId, int state, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Update infa o klientovi na case
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult - OK;
    /// </returns>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 13000; Case #{} not found</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13012; Customer Name must not be empty</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13016; CaseId must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.ServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.ServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task<IServiceCallResult> UpdateCaseCustomer(long caseId, CustomerData customer, CancellationToken cancellationToken = default(CancellationToken));
}
