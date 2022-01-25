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
    Task<IServiceCallResult> CreateCase(CreateCaseRequest model, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Vraci detail Case
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[CaseModel] - OK;
    /// </returns>
    Task<IServiceCallResult> GetCaseDetail(long caseId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Seznam Case pro uzivatele
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[SearchCasesResponse] - OK;
    /// </returns>
    Task<IServiceCallResult> SearchCases(CIS.Core.Types.PaginableRequest pagination, int userId, int? state = null, string? searchTerm = null, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Zmena majitele Case
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult - OK;
    /// </returns>
    Task<IServiceCallResult> LinkOwnerToCase(long caseId, int ownerUserId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Update zakladnich udaju Case - cislo smlouvy
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult - OK;
    /// </returns>
    Task<IServiceCallResult> UpdateCaseData(long caseId, CaseData data, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Update stavu Case
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult - OK;
    /// </returns>
    Task<IServiceCallResult> UpdateCaseState(long caseId, int state, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Update infa o klientovi na case
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult - OK;
    /// </returns>
    Task<IServiceCallResult> UpdateCaseCustomer(long caseId, CustomerData customer, CancellationToken cancellationToken = default(CancellationToken));
}
