using CIS.Core.Types;
using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Clients;

public interface ICaseServiceClient
{
    /// <summary>
    /// Vytvoreni Case
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13002; ProductTypeId must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13003; CaseOwnerUserId must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13018; Target amount must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13012; Customer Name must not be empty</exception>
    /// <exception cref="CIS.Core.Exceptions.CisAlreadyExistsException">Code: 13015; Case #{} already exists</exception>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 13022; User not found: {}</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: {SB error key}; {SB error message}</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task<long> CreateCase(CreateCaseRequest model, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Vraci pocet CASE pro daneho uzivatele v jednotlivych stavech
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13003; CaseOwnerUserId must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task<List<GetCaseCountsResponse.Types.CaseCountsItem>> GetCaseCounts(int caseOwnerUserId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Vraci detail Case
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 13000; Case #{} not found</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13016; CaseId must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task<Case> GetCaseDetail(long caseId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Seznam Case pro uzivatele
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13003; CaseOwnerUserId must be > 0</exception>
    Task<SearchCasesResponse> SearchCases(IPaginableRequest pagination, int userId, List<int>? states = null, string? searchTerm = null, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Zmena majitele Case
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 13000; Case #{} not found</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13003; CaseOwnerUserId must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13016; CaseId must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 13022; User not found: {}</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task LinkOwnerToCase(long caseId, int ownerUserId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Update zakladnich udaju Case - cislo smlouvy
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 13000; Case #{} not found</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13002; ProductTypeId must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13018; Target amount must be between 20_000 and 99_999_999</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13014; ProductTypeId {} not found</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13016; CaseId must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task UpdateCaseData(long caseId, CaseData data, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Update stavu Case
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 13000; Case #{} not found</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13005; Case state already set to the same value</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13006; Case state change not allowed</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13011; Case State {} does not exists</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13016; CaseId must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13017; Case State must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task UpdateCaseState(long caseId, int state, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Update infa o klientovi na case
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 13000; Case #{} not found</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13012; Customer Name must not be empty</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13016; CaseId must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task UpdateCustomerData(long caseId, CustomerData customer, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Smazani case
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 13000; Case #{} not found</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13016; CaseId must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13021; Unable to delete Case – one or more SalesArrangements exists for this case</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task DeleteCase(long caseId, CancellationToken cancellationToken = default(CancellationToken));


    /// <summary>
    /// Ziskani sezanmu ukolu a podukolu
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 13000; Case #{} not found</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13008; Found tasks [{taskIds}] with invalid TypeId [{invalidTypeIds}]. + Found tasks [{taskIds}] with invalid StateId [{invalidStateIds)}].</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13016; CaseId must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task<List<WorkflowTask>> GetTaskList(long caseId, CancellationToken cancellationToken = default(CancellationToken));

    Task<List<WorkflowTask>> GetTaskListByContract(string contractNumber, CancellationToken cancellationToken = default(CancellationToken));

    Task<IList<ProcessTask>> GetProcessList(long caseId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update infa o kontaktech
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 13000; Case #{} not found</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13016; CaseId must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task UpdateOfferContacts(long caseId, OfferContacts contacts, CancellationToken cancellationToken = default(CancellationToken));

    Task NotifyStarbuild(long caseId, string riskBusinessCaseId, CancellationToken cancellationToken = default(CancellationToken));

    Task CompleteTask(CompleteTaskRequest request, CancellationToken cancellationToken = default);

    Task<GetTaskDetailResponse> GetTaskDetail(int taskIdSb, CancellationToken cancellationToken = default);
    
    Task CancelTask(int taskIdSB, CancellationToken cancellationToken = default(CancellationToken));
    
    Task<CreateTaskResponse> CreateTask(CreateTaskRequest request, CancellationToken cancellationToken = default(CancellationToken));
}
