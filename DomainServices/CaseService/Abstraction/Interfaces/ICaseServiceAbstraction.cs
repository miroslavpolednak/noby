using CIS.Core.Results;
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
    /// SuccessfulServiceCallResult[GetCaseDetailResponse] - OK;
    /// </returns>
    Task<IServiceCallResult> GetCaseDetail(long caseId);

    /// <summary>
    /// Seznam Case pro uzivatele
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[GetCaseListResponse] - OK;
    /// </returns>
    Task<IServiceCallResult> GetCaseList(int userId, int? state, CIS.Core.Types.PaginableRequest pagination);

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
