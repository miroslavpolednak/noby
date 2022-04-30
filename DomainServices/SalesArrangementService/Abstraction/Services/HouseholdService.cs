using CIS.DomainServicesSecurity.Abstraction;
using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Abstraction.Services;

internal class HouseholdService : IHouseholdServiceAbstraction
{
    public async Task<IServiceCallResult> CreateHousehold(CreateHouseholdRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(CreateHousehold), request.SalesArrangementId);
        var result = await _userContext.AddUserContext(async () => await _service.CreateHouseholdAsync(request, cancellationToken: cancellationToken)
        );
        return new SuccessfulServiceCallResult<int>(result.HouseholdId);
    }

    public async Task<IServiceCallResult> DeleteHousehold(int householdId, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(DeleteHousehold), householdId);
        var result = await _userContext.AddUserContext(async () => await _service.DeleteHouseholdAsync(
            new()
            {
                HouseholdId = householdId
            }, cancellationToken: cancellationToken)
        );
        return new SuccessfulServiceCallResult();
    }

    public async Task<IServiceCallResult> GetHousehold(int householdId, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(GetHousehold), householdId);
        var result = await _userContext.AddUserContext(async () => await _service.GetHouseholdAsync(
            new()
            {
                HouseholdId = householdId
            }, cancellationToken: cancellationToken)
        );
        return new SuccessfulServiceCallResult<Household>(result);
    }

    public async Task<IServiceCallResult> GetHouseholdList(int salesArrangementId, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(GetHouseholdList), salesArrangementId);
        var result = await _userContext.AddUserContext(async () => await _service.GetHouseholdListAsync(
            new()
            {
                SalesArrangementId = salesArrangementId
            }, cancellationToken: cancellationToken)
        );
        return new SuccessfulServiceCallResult<List<Household>>(result.Households.ToList());
    }

    public async Task<IServiceCallResult> UpdateHousehold(UpdateHouseholdRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(UpdateHousehold), request.HouseholdId);
        var result = await _userContext.AddUserContext(async () => await _service.UpdateHouseholdAsync(request, cancellationToken: cancellationToken));
        return new SuccessfulServiceCallResult();
    }

    public async Task<IServiceCallResult> LinkCustomerOnSAToHousehold(int householdId, int? customerOnSAId1, int? customerOnSAId2, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(LinkCustomerOnSAToHousehold), householdId);
        var result = await _userContext.AddUserContext(async () => await _service.LinkCustomerOnSAToHouseholdAsync(new LinkCustomerOnSAToHouseholdRequest
        {
            HouseholdId = householdId,
            CustomerOnSAId1 = customerOnSAId1,
            CustomerOnSAId2 = customerOnSAId2,
        }, cancellationToken: cancellationToken));
        return new SuccessfulServiceCallResult();
    }

    private readonly ILogger<HouseholdService> _logger;
    private readonly Contracts.v1.HouseholdService.HouseholdServiceClient _service;
    private readonly ICisUserContextHelpers _userContext;

    public HouseholdService(
        ILogger<HouseholdService> logger,
        Contracts.v1.HouseholdService.HouseholdServiceClient service,
        ICisUserContextHelpers userContext)
    {
        _userContext = userContext;
        _service = service;
        _logger = logger;
    }
}