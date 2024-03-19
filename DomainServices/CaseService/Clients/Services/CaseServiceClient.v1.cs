﻿using CIS.Core.Types;
using SharedTypes.Enums;
using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Clients.v1;

internal sealed class CaseServiceClient
    : ICaseServiceClient
{
    public async Task<ValidateCaseIdResponse> ValidateCaseId(long caseId, bool throwExceptionIfNotFound = false, CancellationToken cancellationToken = default)
    {
        if (_cacheValidateCaseIdResponse is null || _cacheValidateCaseIdResponseId != caseId)
        {
            _cacheValidateCaseIdResponse = await _service.ValidateCaseIdAsync(new ValidateCaseIdRequest
            {
                CaseId = caseId,
                ThrowExceptionIfNotFound = throwExceptionIfNotFound
            }, cancellationToken: cancellationToken);
            _cacheValidateCaseIdResponseId = caseId;
        }
        return _cacheValidateCaseIdResponse;
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

    public async Task<List<GetCaseCountsResponse.Types.CaseCountsItem>> GetCaseCounts(int caseOwnerUserId, CancellationToken cancellationToken = default)
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

    public async Task<Case> GetCaseDetail(long caseId, CancellationToken cancellationToken = default)
    {
        if (_cacheGetCaseDetail is null || _cacheGetCaseDetail.CaseId != caseId)
        {
            _cacheGetCaseDetail = await _service.GetCaseDetailAsync(
            new()
            {
                CaseId = caseId
            }, cancellationToken: cancellationToken);
        }
        return _cacheGetCaseDetail;
    }

    public async Task<SearchCasesResponse> SearchCases(IPaginableRequest pagination, int caseOwnerUserId, List<int>? states = null, string? searchTerm = null, CancellationToken cancellationToken = default)
    {
        var request = new SearchCasesRequest
        {
            SearchTerm = searchTerm ?? "",
            Pagination = new SharedTypes.GrpcTypes.PaginationRequest(pagination),
            CaseOwnerUserId = caseOwnerUserId,
        };
        if (states is not null)
            request.State.AddRange(states);
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

    public async Task CancelTask(long caseId, int taskIdSB, CancellationToken cancellationToken = default(CancellationToken))
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

    public async Task<CaseStates> CancelCase(long caseId, bool isUserInvoked = false, CancellationToken cancellationToken = default)
    {
        return (CaseStates)(await _service.CancelCaseAsync(new CancelCaseRequest
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

    // kesovani vysledku validateCase
    private long? _cacheValidateCaseIdResponseId;
    private ValidateCaseIdResponse? _cacheValidateCaseIdResponse;
    private Case? _cacheGetCaseDetail;

    private readonly Contracts.v1.CaseService.CaseServiceClient _service;

    public CaseServiceClient(Contracts.v1.CaseService.CaseServiceClient service)
        => _service = service;
}
