using DomainServices.SalesArrangementService.Abstraction;
using contracts = DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.Household.GetHousehold;

internal class GetHouseholdHandler
    : IRequestHandler<GetHouseholdRequest, GetHouseholdResponse>
{
    public async Task<GetHouseholdResponse> Handle(GetHouseholdRequest request, CancellationToken cancellationToken)
    {
        _logger.RequestHandlerStartedWithId(nameof(GetHouseholdHandler), request.HouseholdId);

        // nacist ulozenou domacnost
        var household = ServiceCallResult.Resolve<contracts.Household>(await _householdService.GetHousehold(request.HouseholdId, cancellationToken));
        GetHouseholdResponse response = household.MapToResponse();

        // nacist klienty
        if (household.CustomerOnSAId1.HasValue)
            response.Customer1 = await getCustomer(household.CustomerOnSAId1.Value, cancellationToken);
        if (household.CustomerOnSAId2.HasValue)
            response.Customer2 = await getCustomer(household.CustomerOnSAId2.Value, cancellationToken);

        return response;
    }

    private async Task<Dto.CustomerInHousehold?> getCustomer(int customerOnSAId, CancellationToken cancellationToken)
    {
        var customer = ServiceCallResult.Resolve<contracts.CustomerOnSA>(await _customerOnSAService.GetCustomer(customerOnSAId, cancellationToken));
        return customer?.MapToResponse();
    }

    private readonly IHouseholdServiceAbstraction _householdService;
    private readonly ICustomerOnSAServiceAbstraction _customerOnSAService;
    private readonly ILogger<GetHouseholdHandler> _logger;

    public GetHouseholdHandler(
        IHouseholdServiceAbstraction householdService,
        ICustomerOnSAServiceAbstraction customerOnSAService,
        ILogger<GetHouseholdHandler> logger)
    {
        _logger = logger;
        _customerOnSAService = customerOnSAService;
        _householdService = householdService;
    }
}
