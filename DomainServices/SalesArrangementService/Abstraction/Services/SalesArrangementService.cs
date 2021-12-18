using CIS.Core.Results;
using DomainServices.SalesArrangementService.Contracts;
using Microsoft.Extensions.Logging;

namespace DomainServices.SalesArrangementService.Abstraction.Services;

internal class SalesArrangementService : ISalesArrangementServiceAbstraction
{
    public async Task<IServiceCallResult> CreateSalesArrangement(long caseId, int salesArrangementType, long? productInstanceId = null, int? offerInstanceId = null)
    {
        _logger.LogDebug("Abstraction CreateSalesArrangement for #{caseId} of type {salesArrangementType}", caseId, salesArrangementType);
        var result = await _userContext.AddUserContext(async () => await _service.CreateSalesArrangementAsync(
            new CreateSalesArrangementRequest() { 
                CaseId = caseId, 
                SalesArrangementType = salesArrangementType, 
                OfferInstanceId = offerInstanceId,
                UserId = _userAccessor.User.Id 
            })
        );
        return new SuccessfulServiceCallResult<int>(result.SalesArrangementId);
    }

    public async Task<IServiceCallResult> GetSalesArrangement(int salesArrangementId)
    {
        _logger.LogDebug("Abstraction GetSalesArrangement for #{salesArrangementId}", salesArrangementId);
        var result = await _userContext.AddUserContext(async () => await _service.GetSalesArrangementAsync(
            new SalesArrangementIdRequest()
            {
                SalesArrangementId = salesArrangementId
            })
        );
        return new SuccessfulServiceCallResult<GetSalesArrangementResponse>(result);
    }

    public async Task<IServiceCallResult> GetSalesArrangementData(int salesArrangementId)
    {
        _logger.LogDebug("Abstraction GetSalesArrangementData for #{salesArrangementId}", salesArrangementId);
        var result = await _userContext.AddUserContext(async () => await _service.GetSalesArrangementDataAsync(
            new SalesArrangementIdRequest()
            {
                SalesArrangementId = salesArrangementId
            })
        );
        return new SuccessfulServiceCallResult<string>(result.Data);
    }

    public async Task<IServiceCallResult> LinkModelationToSalesArrangement(int salesArrangementId, int offerInstanceId)
    {
        _logger.LogDebug("Abstraction LinkModelationToSalesArrangement for #{salesArrangementId}", salesArrangementId);
        var result = await _userContext.AddUserContext(async () => await _service.LinkModelationToSalesArrangementAsync(
            new LinkModelationToSalesArrangementRequest()
            {
                SalesArrangementId = salesArrangementId,
                OfferInstanceId = offerInstanceId
            })
        );
        return new SuccessfulServiceCallResult();
    }

    public Task<IServiceCallResult> GetSalesArrangementsByCaseId(long caseId, IEnumerable<int>? states)
    {
        throw new NotImplementedException();
    }

    public async Task<IServiceCallResult> UpdateSalesArrangementData(int salesArrangementId)
    {
        _logger.LogDebug("Abstraction UpdateSalesArrangementData for #{salesArrangementId}", salesArrangementId);
        /*var result = await _userContext.AddUserContext(async () => await _service.LinkModelationToSalesArrangementAsync(
            new LinkModelationToSalesArrangementRequest()
            {
                SalesArrangementId = salesArrangementId,
                OfferInstanceId = offerInstanceId
            })
        );*/
        return new SuccessfulServiceCallResult();
    }

    public async Task<IServiceCallResult> UpdateSalesArrangementState(int salesArrangementId, int state)
    {
        _logger.LogDebug("Abstraction UpdateSalesArrangementState for #{salesArrangementId}", salesArrangementId);
        var result = await _userContext.AddUserContext(async () => await _service.UpdateSalesArrangementStateAsync(
            new UpdateSalesArrangementStateRequest()
            {
                SalesArrangementId = salesArrangementId,
                State = state
            })
        );
        return new SuccessfulServiceCallResult();
    }

    public Task<IServiceCallResult> ValidateSalesArrangement(int salesArrangementId)
    {
        throw new NotImplementedException();
    }

    private readonly ILogger<SalesArrangementService> _logger;
    private readonly Contracts.v1.SalesArrangementService.SalesArrangementServiceClient _service;
    private readonly CIS.Security.InternalServices.ICisUserContextHelpers _userContext;
    private readonly CIS.Core.Security.ICurrentUserAccessor _userAccessor;

    public SalesArrangementService(
        CIS.Core.Security.ICurrentUserAccessor userAccessor,
        ILogger<SalesArrangementService> logger,
        Contracts.v1.SalesArrangementService.SalesArrangementServiceClient service,
        CIS.Security.InternalServices.ICisUserContextHelpers userContext)
    {
        _userAccessor = userAccessor;
        _userContext = userContext;
        _service = service;
        _logger = logger;
    }
}
