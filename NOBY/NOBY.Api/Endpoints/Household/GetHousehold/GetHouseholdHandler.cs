using DomainServices.HouseholdService.Clients;

namespace NOBY.Api.Endpoints.Household.GetHousehold;

internal sealed class GetHouseholdHandler
    : IRequestHandler<GetHouseholdRequest, GetHouseholdResponse>
{
    public async Task<GetHouseholdResponse> Handle(GetHouseholdRequest request, CancellationToken cancellationToken)
    {
        // nacist ulozenou domacnost
        var household = await _householdService.GetHousehold(request.HouseholdId, cancellationToken);
        GetHouseholdResponse response = household.ToApiResponse();

        // nacist klienty
        if (household.CustomerOnSAId1.HasValue)
            response.Customer1 = await getCustomer(household.CustomerOnSAId1.Value, cancellationToken);
        if (household.CustomerOnSAId2.HasValue)
            response.Customer2 = await getCustomer(household.CustomerOnSAId2.Value, cancellationToken);

        bool isPartner = DomainServices.HouseholdService.Clients.Helpers.AreCustomersPartners(response.Customer1?.MaritalStatusId, response.Customer2?.MaritalStatusId);
        response.AreCustomersPartners = isPartner;

        return response;
    }

    private async Task<CustomerInHousehold?> getCustomer(int customerOnSAId, CancellationToken cancellationToken)
    {
        var customer = await _customerOnSAService.GetCustomer(customerOnSAId, cancellationToken);
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
