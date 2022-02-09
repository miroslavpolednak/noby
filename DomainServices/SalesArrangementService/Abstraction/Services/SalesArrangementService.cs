using CIS.Core.Results;
using DomainServices.SalesArrangementService.Contracts;
using Microsoft.Extensions.Logging;
using CIS.Infrastructure.Logging;

namespace DomainServices.SalesArrangementService.Abstraction.Services;

internal class SalesArrangementService : ISalesArrangementServiceAbstraction
{
    public async Task<IServiceCallResult> CreateSalesArrangement(long caseId, int salesArrangementTypeId, int? offerId = null, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(CreateSalesArrangement), caseId);
        var result = await _userContext.AddUserContext(async () => await _service.CreateSalesArrangementAsync(
            new CreateSalesArrangementRequest() { 
                CaseId = caseId, 
                SalesArrangementTypeId = salesArrangementTypeId, 
                OfferId = offerId
            }, cancellationToken: cancellationToken)
        );
        return new SuccessfulServiceCallResult<int>(result.SalesArrangementId);
    }

    public async Task<IServiceCallResult> GetSalesArrangement(int salesArrangementId, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(GetSalesArrangement), salesArrangementId);
        var result = await _userContext.AddUserContext(async () => await _service.GetSalesArrangementAsync(
            new GetSalesArrangementRequest()
            {
                SalesArrangementId = salesArrangementId
            }, cancellationToken: cancellationToken)
        );
        return new SuccessfulServiceCallResult<SalesArrangement>(result);
    }

    public async Task<IServiceCallResult> GetSalesArrangementData(int salesArrangementId, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(GetSalesArrangementData), salesArrangementId);
        var result = await _userContext.AddUserContext(async () => await _service.GetSalesArrangementDataAsync(
            new SalesArrangementIdRequest()
            {
                SalesArrangementId = salesArrangementId
            }, cancellationToken: cancellationToken)
        );
        return new SuccessfulServiceCallResult<GetSalesArrangementDataResponse>(result);
    }

    public async Task<IServiceCallResult> LinkModelationToSalesArrangement(int salesArrangementId, int offerId, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(LinkModelationToSalesArrangement), salesArrangementId);
        var result = await _userContext.AddUserContext(async () => await _service.LinkModelationToSalesArrangementAsync(
            new LinkModelationToSalesArrangementRequest()
            {
                SalesArrangementId = salesArrangementId,
                OfferId = offerId
            }, cancellationToken: cancellationToken)
        );
        return new SuccessfulServiceCallResult();
    }

    public async Task<IServiceCallResult> GetSalesArrangementsByCaseId(long caseId, IEnumerable<int>? states, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(GetSalesArrangementsByCaseId), caseId);
        var result = await _userContext.AddUserContext(async () => await _service.GetSalesArrangementsByCaseIdAsync(
            new GetSalesArrangementsByCaseIdRequest()
            {
                CaseId = caseId
            }, cancellationToken: cancellationToken)
        );
        return new SuccessfulServiceCallResult<GetSalesArrangementsByCaseIdResponse>(result);
    }

    public async Task<IServiceCallResult> UpdateSalesArrangementData(int salesArrangementId, string data, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(UpdateSalesArrangementData), salesArrangementId);
        var result = await _userContext.AddUserContext(async () => await _service.UpdateSalesArrangementDataAsync(
            new UpdateSalesArrangementDataRequest()
            {
                SalesArrangementId = salesArrangementId,
                Data = data
            }, cancellationToken: cancellationToken)
        );
        return new SuccessfulServiceCallResult();
    }

    public async Task<IServiceCallResult> UpdateSalesArrangementState(int salesArrangementId, int state, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(UpdateSalesArrangementState), salesArrangementId);
        var result = await _userContext.AddUserContext(async () => await _service.UpdateSalesArrangementStateAsync(
            new UpdateSalesArrangementStateRequest()
            {
                SalesArrangementId = salesArrangementId,
                State = state
            }, cancellationToken: cancellationToken)
        );
        return new SuccessfulServiceCallResult();
    }

    public Task<IServiceCallResult> ValidateSalesArrangement(int salesArrangementId, CancellationToken cancellationToken = default(CancellationToken))
    {
        throw new NotImplementedException();
    }

    private readonly ILogger<SalesArrangementService> _logger;
    private readonly Contracts.v1.SalesArrangementService.SalesArrangementServiceClient _service;
    private readonly CIS.Security.InternalServices.ICisUserContextHelpers _userContext;

    public SalesArrangementService(
        ILogger<SalesArrangementService> logger,
        Contracts.v1.SalesArrangementService.SalesArrangementServiceClient service,
        CIS.Security.InternalServices.ICisUserContextHelpers userContext)
    {
        _userContext = userContext;
        _service = service;
        _logger = logger;
    }
}
