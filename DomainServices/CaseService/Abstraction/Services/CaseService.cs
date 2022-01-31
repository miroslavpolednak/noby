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
            new GetCaseCountsRequest()
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
            new GetCaseDetailRequest()
            {
                CaseId = caseId
            }, cancellationToken: cancellationToken)
        );
        return new SuccessfulServiceCallResult<Case>(result);
    }

    public async Task<IServiceCallResult> SearchCases(IPaginableRequest pagination, int caseOwnerUserId, int? state = null, string? searchTerm = null, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(SearchCases), caseOwnerUserId);
        var result = await _userContext.AddUserContext(async () => await _service.SearchCasesAsync(new SearchCasesRequest
        {
            SearchTerm = searchTerm ?? "",
            State = state,
            Pagination = new CIS.Infrastructure.gRPC.CisTypes.PaginationRequest(pagination),
            CaseOwnerUserId = caseOwnerUserId,
        }, cancellationToken: cancellationToken));
        return new SuccessfulServiceCallResult<SearchCasesResponse>(result);
    }

    public async Task<IServiceCallResult> LinkOwnerToCase(long caseId, int ownerUserId, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(LinkOwnerToCase), caseId);
        var result = await _userContext.AddUserContext(async () => await _service.LinkOwnerToCaseAsync(
            new LinkOwnerToCaseRequest()
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
            new UpdateCaseCustomerRequest()
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
            new UpdateCaseDataRequest()
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
            new UpdateCaseStateRequest()
            {
                CaseId = caseId,
                State = state
            },  cancellationToken: cancellationToken)
        );
        return new SuccessfulServiceCallResult();
    }

    private readonly ILogger<CaseService> _logger;
    private readonly Contracts.v1.CaseService.CaseServiceClient _service;
    private readonly CIS.Security.InternalServices.ICisUserContextHelpers _userContext;
    private readonly CIS.Core.Security.ICurrentUserAccessor _userAccessor;

    public CaseService(
        CIS.Core.Security.ICurrentUserAccessor userAccessor,
        ILogger<CaseService> logger,
        Contracts.v1.CaseService.CaseServiceClient service,
        CIS.Security.InternalServices.ICisUserContextHelpers userContext)
    {
        _userAccessor = userAccessor;
        _userContext = userContext;
        _service = service;
        _logger = logger;
    }
}
