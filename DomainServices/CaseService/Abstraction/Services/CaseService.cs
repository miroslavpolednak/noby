using CIS.Core.Results;
using CIS.Core.Types;
using DomainServices.CaseService.Contracts;
using Microsoft.Extensions.Logging;
using CIS.Infrastructure.Logging;

namespace DomainServices.CaseService.Abstraction.Services;

internal class CaseService : ICaseServiceAbstraction
{
    public async Task<IServiceCallResult> CreateCase(CreateCaseRequest model, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStarted(nameof(CreateCase));
        var result = await _userContext.AddUserContext(async () => await _service.CreateCaseAsync(model, cancellationToken: cancellationToken));
        return new SuccessfulServiceCallResult<long>(result.CaseId);
    }

    public async Task<IServiceCallResult> GetCaseCounts(int caseOwnerUserId, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(GetCaseCounts), caseOwnerUserId);
        var result = await _userContext.AddUserContext(async () => await _service.GetCaseCountsAsync(
            new()
            {
                CaseOwnerUserId = caseOwnerUserId
            }, cancellationToken: cancellationToken)
        );
        return new SuccessfulServiceCallResult<GetCaseCountsResponse>(result);
    }

    public async Task<IServiceCallResult> GetCaseDetail(long caseId, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(GetCaseDetail), caseId);
        var result = await _userContext.AddUserContext(async () => await _service.GetCaseDetailAsync(
            new()
            {
                CaseId = caseId
            }, cancellationToken: cancellationToken)
        );
        return new SuccessfulServiceCallResult<Case>(result);
    }

    public async Task<IServiceCallResult> SearchCases(IPaginableRequest pagination, int caseOwnerUserId, List<int>? states = null, string? searchTerm = null, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(SearchCases), caseOwnerUserId);
        var request = new SearchCasesRequest
        {
            SearchTerm = searchTerm ?? "",
            Pagination = new CIS.Infrastructure.gRPC.CisTypes.PaginationRequest(pagination),
            CaseOwnerUserId = caseOwnerUserId,
        };
        if (states is not null)
            request.State.AddRange(states);
        var result = await _userContext.AddUserContext(async () => await _service.SearchCasesAsync(request, cancellationToken: cancellationToken));
        return new SuccessfulServiceCallResult<SearchCasesResponse>(result);
    }

    public async Task<IServiceCallResult> LinkOwnerToCase(long caseId, int ownerUserId, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(LinkOwnerToCase), caseId);
        var result = await _userContext.AddUserContext(async () => await _service.LinkOwnerToCaseAsync(
            new()
            {
                CaseId = caseId,
                CaseOwnerUserId = ownerUserId
            }, cancellationToken: cancellationToken)
        );
        return new SuccessfulServiceCallResult();
    }

    public async Task<IServiceCallResult> UpdateCaseCustomer(long caseId, CustomerData customer, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStarted(nameof(UpdateCaseCustomer));
        var result = await _userContext.AddUserContext(async () => await _service.UpdateCaseCustomerAsync(
            new()
            {
                CaseId = caseId,
                Customer = customer
            }, cancellationToken: cancellationToken)
        );
        return new SuccessfulServiceCallResult();
    }

    public async Task<IServiceCallResult> UpdateCaseData(long caseId, CaseData data, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(UpdateCaseData), caseId);
        var result = await _userContext.AddUserContext(async () => await _service.UpdateCaseDataAsync(
            new()
            {
                CaseId = caseId,
                Data = data
            }, cancellationToken: cancellationToken)
        );
        return new SuccessfulServiceCallResult();
    }

    public async Task<IServiceCallResult> UpdateCaseState(long caseId, int state, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(UpdateCaseState), caseId);
        var result = await _userContext.AddUserContext(async () => await _service.UpdateCaseStateAsync(
            new()
            {
                CaseId = caseId,
                State = state
            },  cancellationToken: cancellationToken)
        );
        return new SuccessfulServiceCallResult();
    }

    public async Task<IServiceCallResult> DeleteCase(long caseId, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(UpdateCaseState), caseId);
        var result = await _userContext.AddUserContext(async () => await _service.DeleteCaseAsync(
            new()
            {
                CaseId = caseId
            }, cancellationToken: cancellationToken)
        );
        return new SuccessfulServiceCallResult();
    }

    public async Task<IServiceCallResult> GetTaskList(long caseId, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(GetTaskList), caseId);
        var result = await _userContext.AddUserContext(async () => await _service.GetTaskListAsync(
            new()
            {
                CaseId = caseId
            }, cancellationToken: cancellationToken)
        );
        return new SuccessfulServiceCallResult<GetTaskListResponse>(result);
    }

    private readonly ILogger<CaseService> _logger;
    private readonly Contracts.v1.CaseService.CaseServiceClient _service;
    private readonly CIS.DomainServicesSecurity.Abstraction.ICisUserContextHelpers _userContext;

    public CaseService(
        ILogger<CaseService> logger,
        Contracts.v1.CaseService.CaseServiceClient service,
        CIS.DomainServicesSecurity.Abstraction.ICisUserContextHelpers userContext)
    {
        _userContext = userContext;
        _service = service;
        _logger = logger;
    }
}
