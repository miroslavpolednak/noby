﻿using CIS.Core.Types;
using SharedTypes.Enums;
using DomainServices.CaseService.Contracts;
using SharedTypes.GrpcTypes;
using CIS.Infrastructure.Caching.Grpc;

namespace DomainServices.CaseService.Clients.v1;

internal sealed class CaseServiceClient(
    Contracts.v1.CaseService.CaseServiceClient _service,
    IGrpcClientResponseCache<CaseServiceClient> _cache)
    : ICaseServiceClient
{
    public async Task<ValidateCaseIdResponse> ValidateCaseId(long caseId, bool throwExceptionIfNotFound = false, CancellationToken cancellationToken = default)
    {
        return await _cache.GetLocalOrDistributed(
            caseId, 
            async (c) => await ValidateCaseIdWithoutCache(caseId, throwExceptionIfNotFound, c), 
            new Microsoft.Extensions.Caching.Distributed.DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(30)
            },
            cancellationToken);
    }

    public async Task<ValidateCaseIdResponse> ValidateCaseIdWithoutCache(long caseId, bool throwExceptionIfNotFound = false, CancellationToken cancellationToken = default)
    {
        return await _service.ValidateCaseIdAsync(new ValidateCaseIdRequest
        {
            CaseId = caseId,
            ThrowExceptionIfNotFound = throwExceptionIfNotFound
        }, cancellationToken: cancellationToken);
    }

    public async Task<long> CreateCase(CreateCaseRequest model, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateCaseAsync(model, cancellationToken: cancellationToken);
        return result.CaseId;
    }

    public async Task<long> CreateExistingCase(CreateExistingCaseRequest model, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateExistingCaseAsync(model, cancellationToken: cancellationToken);
        return result.CaseId;
    }

    public async Task<List<GetCaseCountsResponse.Types.CaseCountsItem>> GetCaseCounts(int caseOwnerUserId, int? stateUpdatedTimeLimitInDays, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetCaseCountsAsync(
            new()
            {
                CaseOwnerUserId = caseOwnerUserId,
                StateUpdatedTimeLimitInDays = stateUpdatedTimeLimitInDays
            }, cancellationToken: cancellationToken);
        return result.CaseCounts.ToList();
    }

    public Task CompleteTask(CompleteTaskRequest request, CancellationToken cancellationToken = default)
    {
        return _service.CompleteTaskAsync(request, cancellationToken: cancellationToken).ResponseAsync;
    }

    public async Task<Case> GetCaseDetail(long caseId, CancellationToken cancellationToken = default)
    {
        return await _cache.GetLocalOrDistributed(
            caseId, 
            async (c) => await GetCaseDetailWithoutCache(caseId, c),
            new Microsoft.Extensions.Caching.Distributed.DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(30)
            },
            cancellationToken);
    }

    public async Task<Case> GetCaseDetailWithoutCache(long caseId, CancellationToken cancellationToken = default)
    {
        return await _service.GetCaseDetailAsync(
            new()
            {
                CaseId = caseId
            }, cancellationToken: cancellationToken);
    }

    public async Task<SearchCasesResponse> SearchCases(IPaginableRequest pagination, int caseOwnerUserId, List<EnumCaseStates>? states, int? stateUpdatedTimeLimitInDays, string? searchTerm, CancellationToken cancellationToken = default)
    {
        var request = new SearchCasesRequest
        {
            SearchTerm = searchTerm ?? "",
            Pagination = new PaginationRequest(pagination),
            CaseOwnerUserId = caseOwnerUserId,
            StateUpdatedTimeLimitInDays = stateUpdatedTimeLimitInDays
        };
        if (states is not null)
            request.State.AddRange(states.Select(t => (int)t));
        return await _service.SearchCasesAsync(request, cancellationToken: cancellationToken);
    }

    public async Task LinkOwnerToCase(long caseId, int ownerUserId, CancellationToken cancellationToken = default)
    {
        await _service.LinkOwnerToCaseAsync(
            new()
            {
                CaseId = caseId,
                CaseOwnerUserId = ownerUserId
            }, cancellationToken: cancellationToken);
    }

    public async Task UpdateCustomerData(long caseId, CustomerData customer, CancellationToken cancellationToken = default)
    {
        await _service.UpdateCustomerDataAsync(
            new()
            {
                CaseId = caseId,
                Customer = customer
            }, cancellationToken: cancellationToken);
    }

    public async Task UpdateCaseData(long caseId, CaseData data, CancellationToken cancellationToken = default)
    {
        await _service.UpdateCaseDataAsync(
            new()
            {
                CaseId = caseId,
                Data = data
            }, cancellationToken: cancellationToken);
    }

    public async Task UpdateCaseState(long caseId, int state, CancellationToken cancellationToken = default)
    {
        await _service.UpdateCaseStateAsync(
            new()
            {
                CaseId = caseId,
                State = state
            }, cancellationToken: cancellationToken);
    }

    public async Task DeleteCase(long caseId, CancellationToken cancellationToken = default)
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

    public async Task<List<WorkflowTask>> GetTaskList(long caseId, CancellationToken cancellationToken = default)
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

    public async Task UpdateOfferContacts(long caseId, OfferContacts contacts, CancellationToken cancellationToken = default)
    {
        await _service.UpdateOfferContactsAsync(
            new()
            {
                CaseId = caseId,
                OfferContacts = contacts
            }, cancellationToken: cancellationToken);
    }

    public async Task NotifyStarbuild(long caseId, string? riskBusinessCaseId, CancellationToken cancellationToken = default)
    {
        await _service.NotifyStarbuildAsync(
            new()
            {
                CaseId = caseId,
                RiskBusinessCaseId = riskBusinessCaseId
            }, cancellationToken: cancellationToken);
    }

    public async Task CancelTask(long caseId, int taskIdSB, CancellationToken cancellationToken = default)
    {
        await _service.CancelTaskAsync(new CancelTaskRequest
        {
            CaseId = caseId,
            TaskIdSB = taskIdSB
        }, cancellationToken: cancellationToken);
    }

    public async Task<CreateTaskResponse> CreateTask(CreateTaskRequest request, CancellationToken cancellationToken = default)
    {
        return await _service.CreateTaskAsync(request, cancellationToken: cancellationToken);
    }

    public async Task<EnumCaseStates> CancelCase(long caseId, bool isUserInvoked = false, CancellationToken cancellationToken = default)
    {
        return (EnumCaseStates)(await _service.CancelCaseAsync(new CancelCaseRequest
        {
            CaseId = caseId,
            IsUserInvoked = isUserInvoked,
        }, cancellationToken: cancellationToken))
        .CaseState;
    }

    public async Task UpdateTask(UpdateTaskRequest request, CancellationToken cancellationToken = default)
    {
        await _service.UpdateTaskAsync(request, cancellationToken: cancellationToken);
    }

    public async Task<ICollection<Case>> GetCasesByIdentity(Identity identity, CancellationToken cancellationToken = default)
    {
        var response = await _service.GetCasesByIdentityAsync(new GetCasesByIdentityRequest { CustomerIdentity = identity }, cancellationToken: cancellationToken);

        return response.Cases;
    }
}
