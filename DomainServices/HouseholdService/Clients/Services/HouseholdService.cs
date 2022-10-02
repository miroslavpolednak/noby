using CIS.Infrastructure.gRPC;
using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Clients.Services;

internal class HouseholdService : IHouseholdServiceClient
{
    public async Task<IServiceCallResult> CreateHousehold(CreateHouseholdRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(CreateHousehold), request.SalesArrangementId);
        var result = await _service.CreateHouseholdAsync(request, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult<int>(result.HouseholdId);
    }

    public async Task<IServiceCallResult> DeleteHousehold(int householdId, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(DeleteHousehold), householdId);
        var result = await _service.DeleteHouseholdAsync(
            new()
            {
                HouseholdId = householdId
            }, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult();
    }

    public async Task<IServiceCallResult> GetHousehold(int householdId, CancellationToken cancellationToken = default(CancellationToken))
        => new SuccessfulServiceCallResult<Household>(await _householdCache.GetOrFetch(householdId, async () =>
        {
            _logger.RequestHandlerStartedWithId(nameof(GetHousehold), householdId);
            var result = await _service.GetHouseholdAsync(
                new()
                {
                    HouseholdId = householdId
                }, cancellationToken: cancellationToken);
            return result;
        }));

    public async Task<IServiceCallResult> GetHouseholdList(int salesArrangementId, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(GetHouseholdList), salesArrangementId);
        var result = await _service.GetHouseholdListAsync(
            new()
            {
                SalesArrangementId = salesArrangementId
            }, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult<List<Household>>(result.Households.ToList());
    }

    public async Task<IServiceCallResult> UpdateHousehold(UpdateHouseholdRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(UpdateHousehold), request.HouseholdId);
        var result = await _service.UpdateHouseholdAsync(request, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult();
    }

    public async Task<IServiceCallResult> LinkCustomerOnSAToHousehold(int householdId, int? customerOnSAId1, int? customerOnSAId2, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(LinkCustomerOnSAToHousehold), householdId);
        var result = await _service.LinkCustomerOnSAToHouseholdAsync(new LinkCustomerOnSAToHouseholdRequest
        {
            HouseholdId = householdId,
            CustomerOnSAId1 = customerOnSAId1,
            CustomerOnSAId2 = customerOnSAId2,
        }, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult();
    }

    private readonly ILogger<HouseholdService> _logger;
    private readonly Contracts.v1.HouseholdService.HouseholdServiceClient _service;
    private readonly ServiceClientResultCache<Household> _householdCache;

    public HouseholdService(
        ILogger<HouseholdService> logger,
        Contracts.v1.HouseholdService.HouseholdServiceClient service)
    {
        _householdCache = new ServiceClientResultCache<Household>();
        _service = service;
        _logger = logger;
    }
}