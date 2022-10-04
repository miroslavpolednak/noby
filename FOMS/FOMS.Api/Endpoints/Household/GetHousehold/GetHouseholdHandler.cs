using DomainServices.HouseholdService.Clients;
using contracts = DomainServices.HouseholdService.Contracts;

namespace FOMS.Api.Endpoints.Household.GetHousehold;

internal class GetHouseholdHandler
    : IRequestHandler<GetHouseholdRequest, GetHouseholdResponse>
{
    public async Task<GetHouseholdResponse> Handle(GetHouseholdRequest request, CancellationToken cancellationToken)
    {
        // nacist ulozenou domacnost
        var household = ServiceCallResult.ResolveAndThrowIfError<contracts.Household>(await _householdService.GetHousehold(request.HouseholdId, cancellationToken));
        GetHouseholdResponse response = household.ToApiResponse();

        // nacist klienty
        if (household.CustomerOnSAId1.HasValue)
            response.Customer1 = await getCustomer(household.CustomerOnSAId1.Value, cancellationToken);
        if (household.CustomerOnSAId2.HasValue)
            response.Customer2 = await getCustomer(household.CustomerOnSAId2.Value, cancellationToken);

        return response;
    }

    private async Task<CustomerInHousehold?> getCustomer(int customerOnSAId, CancellationToken cancellationToken)
    {
        var customer = ServiceCallResult.ResolveAndThrowIfError<contracts.CustomerOnSA>(await _customerOnSAService.GetCustomer(customerOnSAId, cancellationToken));
        return customer?.ToApiResponse();
    }

    private readonly IHouseholdServiceClient _householdService;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;

    public GetHouseholdHandler(
        IHouseholdServiceClient householdService,
        ICustomerOnSAServiceClient customerOnSAService)
    {
        _customerOnSAService = customerOnSAService;
        _householdService = householdService;
    }
}
