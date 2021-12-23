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
    /// SuccessfulServiceCallResult[long (CaseId)] - OK;
    /// </returns>
    Task<IServiceCallResult> CreateCase(CreateCaseRequest model);

    /// <summary>
    /// Vraci detail Case
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[CaseModel] - OK;
    /// </returns>
    Task<IServiceCallResult> GetCaseDetail(long caseId);

    /// <summary>
    /// Seznam Case pro uzivatele
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[SearchCasesResponse] - OK;
    /// </returns>
    Task<IServiceCallResult> SearchCases(CIS.Core.Types.PaginableRequest pagination, int userId, int? state = null, string? searchTerm = null);

    /// <summary>
    /// Zmena majitele Case
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult - OK;
    /// </returns>
    Task<IServiceCallResult> LinkOwnerToCase(long caseId, int userId);

    /// <summary>
    /// Update zakladnich udaju Case - cislo smlouvy
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult - OK;
    /// </returns>
    Task<IServiceCallResult> UpdateCaseData(long caseId, string contractNumber);

    /// <summary>
    /// Update stavu Case
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult - OK;
    /// </returns>
    Task<IServiceCallResult> UpdateCaseState(long caseId, int state);

    /// <summary>
    /// Update infa o klientovi na case
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult - OK;
    /// </returns>
    Task<IServiceCallResult> UpdateCaseCustomer(UpdateCaseCustomerRequest model);
}
