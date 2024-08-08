using DomainServices.HouseholdService.Clients;

namespace NOBY.Api.Endpoints.Household.GetHousehold;

internal sealed class GetHouseholdHandler(
    IHouseholdServiceClient _householdService,
    ICustomerOnSAServiceClient _customerOnSAService)
        : IRequestHandler<GetHouseholdRequest, HouseholdGetHouseholdResponse>
{
    public async Task<HouseholdGetHouseholdResponse> Handle(GetHouseholdRequest request, CancellationToken cancellationToken)
    {
        // nacist ulozenou domacnost
        var household = await _householdService.GetHousehold(request.HouseholdId, cancellationToken);
        var response = household.ToApiResponse();

        // nacist klienty
        if (household.CustomerOnSAId1.HasValue)
            response.Customer1 = await getCustomer(household.CustomerOnSAId1.Value, cancellationToken);
        if (household.CustomerOnSAId2.HasValue)
            response.Customer2 = await getCustomer(household.CustomerOnSAId2.Value, cancellationToken);

        bool isPartner = Helpers.AreCustomersPartners(response.Customer1?.MaritalStatusId, response.Customer2?.MaritalStatusId);
        response.AreCustomersPartners = isPartner;

        return response;
    }

    private async Task<HouseholdCustomerInHousehold?> getCustomer(int customerOnSAId, CancellationToken cancellationToken)
    {
        var customer = await _customerOnSAService.GetCustomer(customerOnSAId, cancellationToken);
        return customer?.ToApiResponse();
    }
}
