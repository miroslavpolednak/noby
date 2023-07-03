using CIS.Core.Types;
using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Clients.Services;

internal sealed class CaseServiceClient
    : ICaseServiceClient
{
    public async Task<ValidateCaseIdResponse> ValidateCaseId(long caseId, bool throwExceptionIfNotFound = false, CancellationToken cancellationToken = default(CancellationToken))
    {
        return await _service.ValidateCaseIdAsync(new ValidateCaseIdRequest
        {
            CaseId = caseId,
            ThrowExceptionIfNotFound = throwExceptionIfNotFound
        }, cancellationToken: cancellationToken);
    }

    public async Task<long> CreateCase(CreateCaseRequest model, CancellationToken cancellationToken = default(CancellationToken))
    {
        var result = await _service.CreateCaseAsync(model, cancellationToken: cancellationToken);
        return result.CaseId;
    }

    public async Task<List<GetCaseCountsResponse.Types.CaseCountsItem>> GetCaseCounts(int caseOwnerUserId, CancellationToken cancellationToken = default(CancellationToken))
    {
        var result = await _service.GetCaseCountsAsync(
            new()
            {
                CaseOwnerUserId = caseOwnerUserId
            }, cancellationToken: cancellationToken);
        return result.CaseCounts.ToList();
    }

    public Task CompleteTask(CompleteTaskRequest request, CancellationToken cancellationToken = default)
    {
        return _service.CompleteTaskAsync(request, cancellationToken: cancellationToken).ResponseAsync;
    }

    public async Task<Case> GetCaseDetail(long caseId, CancellationToken cancellationToken = default(CancellationToken))
    {
        return await _service.GetCaseDetailAsync(
            new()
            {
                CaseId = caseId
            }, cancellationToken: cancellationToken);
    }

    public async Task<SearchCasesResponse> SearchCases(IPaginableRequest pagination, int caseOwnerUserId, List<int>? states = null, string? searchTerm = null, CancellationToken cancellationToken = default(CancellationToken))
    {
        var request = new SearchCasesRequest
        {
            SearchTerm = searchTerm ?? "",
            Pagination = new CIS.Infrastructure.gRPC.CisTypes.PaginationRequest(pagination),
            CaseOwnerUserId = caseOwnerUserId,
        };
        if (states is not null)
            request.State.AddRange(states);
        return await _service.SearchCasesAsync(request, cancellationToken: cancellationToken);
    }

    public async Task LinkOwnerToCase(long caseId, int ownerUserId, CancellationToken cancellationToken = default(CancellationToken))
    {
        await _service.LinkOwnerToCaseAsync(
            new()
            {
                CaseId = caseId,
                CaseOwnerUserId = ownerUserId
            }, cancellationToken: cancellationToken);
    }

    public async Task UpdateCustomerData(long caseId, CustomerData customer, CancellationToken cancellationToken = default(CancellationToken))
    {
        await _service.UpdateCustomerDataAsync(
            new()
            {
                CaseId = caseId,
                Customer = customer
            }, cancellationToken: cancellationToken);
    }

    public async Task UpdateCaseData(long caseId, CaseData data, CancellationToken cancellationToken = default(CancellationToken))
    {
        await _service.UpdateCaseDataAsync(
            new()
            {
                CaseId = caseId,
                Data = data
            }, cancellationToken: cancellationToken);
    }

    public async Task UpdateCaseState(long caseId, int state, CancellationToken cancellationToken = default(CancellationToken))
    {
        await _service.UpdateCaseStateAsync(
            new()
            {
                CaseId = caseId,
                State = state
            },  cancellationToken: cancellationToken);
    }

    public async Task DeleteCase(long caseId, CancellationToken cancellationToken = default(CancellationToken))
    {
        await _service.DeleteCaseAsync(
            new()
            {
                CaseId = caseId
            }, cancellationToken: cancellationToken);
    }

    public Task<GetTaskDetailResponse> GetTaskDetail(int taskIdSb, CancellationToken cancellationToken = default)
    {
        return _service.GetTaskDetailAsync(new GetTaskDetailRequest { TaskIdSb = taskIdSb }, cancellationToken: cancellationToken).ResponseAsync;
    }

    public async Task<List<WorkflowTask>> GetTaskList(long caseId, CancellationToken cancellationToken = default(CancellationToken))
    {
        var result = await _service.GetTaskListAsync(
            new()
            {
                CaseId = caseId
            }, cancellationToken: cancellationToken);
        return result.Tasks.ToList();
    }

    public async Task<IList<ProcessTask>> GetProcessList(long caseId, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetProcessListAsync(new GetProcessListRequest { CaseId = caseId }, cancellationToken: cancellationToken);

        return result.Processes;
    }

    public async Task UpdateOfferContacts(long caseId, OfferContacts contacts, CancellationToken cancellationToken = default(CancellationToken))
    {
        await _service.UpdateOfferContactsAsync(
            new()
            {
                CaseId = caseId,
                OfferContacts = contacts
            }, cancellationToken: cancellationToken);
    }

    public async Task NotifyStarbuild(long caseId, string? riskBusinessCaseId, CancellationToken cancellationToken = default(CancellationToken))
    {
        await _service.NotifyStarbuildAsync(
            new()
            {
                CaseId = caseId,
                RiskBusinessCaseId = riskBusinessCaseId
            }, cancellationToken: cancellationToken);
    }

    public async Task CancelTask(int taskIdSB, CancellationToken cancellationToken = default(CancellationToken))
    {
        await _service.CancelTaskAsync(new CancelTaskRequest
        {
            TaskIdSB = taskIdSB
        }, cancellationToken: cancellationToken);
    }
    
    public async Task UpdateActiveTasks(UpdateActiveTasksRequest request, CancellationToken cancellationToken = default)
    {
        await _service.UpdateActiveTasksAsync(request, cancellationToken: cancellationToken);
    }

    public async Task<CreateTaskResponse> CreateTask(CreateTaskRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        return await _service.CreateTaskAsync(request, cancellationToken: cancellationToken);
    }

    private readonly Contracts.v1.CaseService.CaseServiceClient _service;
    public CaseServiceClient(Contracts.v1.CaseService.CaseServiceClient service)
        => _service = service;
}
