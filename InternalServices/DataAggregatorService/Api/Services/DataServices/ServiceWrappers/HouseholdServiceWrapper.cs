using CIS.Foms.Enums;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.CustomModels;
using DomainServices.HouseholdService.Clients;
using DomainServices.HouseholdService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.ServiceWrappers;

[TransientService, SelfService]
internal class HouseholdServiceWrapper : IServiceWrapper
{
    private readonly IHouseholdServiceClient _householdService;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;

    public HouseholdServiceWrapper(IHouseholdServiceClient householdService, ICustomerOnSAServiceClient customerOnSAService)
    {
        _householdService = householdService;
        _customerOnSAService = customerOnSAService;
    }

    public DataSource DataSource => DataSource.HouseholdService;

    public async Task LoadData(InputParameters input, AggregatedData data, CancellationToken cancellationToken)
    {
        input.ValidateSalesArrangementId();

        var households = await _householdService.GetHouseholdList(input.SalesArrangementId!.Value, cancellationToken);

        var householdsByType = households.ToLookup(x => (HouseholdTypes)x.HouseholdTypeId);

        data.HouseholdMain = new HouseholdInfo { Household = householdsByType[HouseholdTypes.Main].First() };

        var householdCodebtor = householdsByType[HouseholdTypes.Codebtor].FirstOrDefault();
        if (householdCodebtor is not null)
            data.HouseholdCodebtor = new HouseholdInfo { Household = householdCodebtor };
    }

    public async Task LoadHouseholdWithCustomers(InputParameters input, AggregatedData data, CancellationToken cancellationToken)
    {
        input.ValidateSalesArrangementId();

        var households = await _householdService.GetHouseholdList(input.SalesArrangementId!.Value, cancellationToken);

        var householdsByType = households.ToLookup(x => (HouseholdTypes)x.HouseholdTypeId);

        data.HouseholdMain = await CreateHouseholdInfo(householdsByType[HouseholdTypes.Main].First(), cancellationToken);

        var householdCodebtor = householdsByType[HouseholdTypes.Codebtor].FirstOrDefault();
        if (householdCodebtor is not null)
            data.HouseholdCodebtor = await CreateHouseholdInfo(householdCodebtor, cancellationToken);
    }

    private async Task<HouseholdInfo> CreateHouseholdInfo(Household household, CancellationToken cancellationToken)
    {
        CustomerOnSA? customer1 = null;
        CustomerOnSA? customer2 = null;

        if (household.CustomerOnSAId1.HasValue)
            customer1 = await _customerOnSAService.GetCustomer(household.CustomerOnSAId1.Value, cancellationToken);

        if (household.CustomerOnSAId2.HasValue)
            customer2 = await _customerOnSAService.GetCustomer(household.CustomerOnSAId2.Value, cancellationToken);

        return new HouseholdInfo
        {
            Household = household,
            CustomerOnSa1 = customer1,
            CustomerOnSa2 = customer2
        };
    }
}