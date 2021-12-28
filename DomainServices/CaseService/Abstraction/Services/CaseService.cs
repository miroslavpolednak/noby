using CIS.Core.Results;
using DomainServices.CaseService.Contracts;
using Microsoft.Extensions.Logging;

namespace DomainServices.CaseService.Abstraction.Services;

internal class CaseService : ICaseServiceAbstraction
{
    public async Task<IServiceCallResult> CreateCase(CreateCaseRequest model, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.LogDebug("Abstraction CreateCase");
        var result = await _userContext.AddUserContext(async () => await _service.CreateCaseAsync(
            new CreateCaseRequest()
            {
                DateOfBirthNaturalPerson = model.DateOfBirthNaturalPerson,
                FirstNameNaturalPerson = model.FirstNameNaturalPerson,
                Name = model.Name,
                ProductInstanceType = model.ProductInstanceType,
                TargetAmount = model.TargetAmount,
                UserId = _userAccessor.User.Id,
                Customer = model.Customer
            }, cancellationToken: cancellationToken)
        );
        return new SuccessfulServiceCallResult<long>(result.CaseId);
    }

    public async Task<IServiceCallResult> GetCaseDetail(long caseId, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.LogDebug("Abstraction GetCaseDetail for #{caseId}", caseId);
        var result = await _userContext.AddUserContext(async () => await _service.GetCaseDetailAsync(
            new GetCaseDetailRequest()
            {
                CaseId = caseId
            }, cancellationToken: cancellationToken)
        );
        return new SuccessfulServiceCallResult<CaseModel>(result);
    }

    public async Task<IServiceCallResult> SearchCases(CIS.Core.Types.PaginableRequest pagination, int userId, int? state = null, string? searchTerm = null, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.LogDebug("Abstraction SearchCases for #{userId}", userId);
        var result = await _userContext.AddUserContext(async () => await _service.SearchCasesAsync(new SearchCasesRequest
        {
            SearchTerm = searchTerm ?? "",
            State = state,
            Pagination = pagination,
            UserId = userId,
        }, cancellationToken: cancellationToken));
        return new SuccessfulServiceCallResult<SearchCasesResponse>(result);
    }

    public async Task<IServiceCallResult> LinkOwnerToCase(long caseId, int userId, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.LogDebug("Abstraction LinkOwnerToCase for #{caseId}", caseId);
        var result = await _userContext.AddUserContext(async () => await _service.LinkOwnerToCaseAsync(
            new LinkOwnerToCaseRequest()
            {
                CaseId = caseId,
                UserId = userId
            }, cancellationToken: cancellationToken)
        );
        return new SuccessfulServiceCallResult();
    }

    public async Task<IServiceCallResult> UpdateCaseCustomer(UpdateCaseCustomerRequest model, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.LogDebug("Abstraction UpdateCaseCustomer");
        var result = await _userContext.AddUserContext(async () => await _service.UpdateCaseCustomerAsync(
            new UpdateCaseCustomerRequest()
            {
                CaseId = model.CaseId,
                DateOfBirthNaturalPerson = model.DateOfBirthNaturalPerson,
                FirstNameNaturalPerson = model.FirstNameNaturalPerson,
                Name = model.Name,
                Customer = model.Customer
            }, cancellationToken: cancellationToken)
        );
        return new SuccessfulServiceCallResult();
    }

    public async Task<IServiceCallResult> UpdateCaseData(long caseId, string? contractNumber = null, int? targetAmount = null, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.LogDebug("Abstraction UpdateCaseData for #{caseId}", caseId);
        var result = await _userContext.AddUserContext(async () => await _service.UpdateCaseDataAsync(
            new UpdateCaseDataRequest()
            {
                CaseId = caseId,
                ContractNumber = contractNumber ?? "",
                TargetAmount = targetAmount
            }, cancellationToken: cancellationToken)
        );
        return new SuccessfulServiceCallResult();
    }

    public async Task<IServiceCallResult> UpdateCaseState(long caseId, int state, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.LogDebug("Abstraction UpdateCaseState for #{caseId}", caseId);
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
