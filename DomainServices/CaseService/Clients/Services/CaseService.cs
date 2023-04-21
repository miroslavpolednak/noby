using CIS.Core.Types;
using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Clients.Services;

internal sealed class CaseService 
    : ICaseServiceClient
{
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

    public async Task<List<WorkflowTask>> GetTaskList(long caseId, CancellationToken cancellationToken = default(CancellationToken))
    {
        var result = await _service.GetTaskListAsync(
            new()
            {
                CaseId = caseId
            }, cancellationToken: cancellationToken);
        return result.Tasks.ToList();
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

    public async Task CancelTask(int taskSBId, CancellationToken cancellationToken = default(CancellationToken))
    {
        await _service.CancelTaskAsync(new CancelTaskRequest
        {
            TaskIdSB = taskSBId
        }, cancellationToken: cancellationToken);
    }

    private readonly Contracts.v1.CaseService.CaseServiceClient _service;
    public CaseService(Contracts.v1.CaseService.CaseServiceClient service)
        => _service = service;
}
