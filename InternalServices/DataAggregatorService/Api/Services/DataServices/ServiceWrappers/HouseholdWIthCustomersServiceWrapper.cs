using CIS.Foms.Enums;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.Dto;
using DomainServices.HouseholdService.Clients;
using DomainServices.HouseholdService.Contracts;
using Household = DomainServices.HouseholdService.Contracts.Household;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.ServiceWrappers;

[TransientService, SelfService]
internal class HouseholdWIthCustomersServiceWrapper : IServiceWrapper
{
    private readonly IHouseholdServiceClient _householdService;
    private readonly ICustomerOnSAServiceClient _customerOnSaService;

    public HouseholdWIthCustomersServiceWrapper(IHouseholdServiceClient householdService, ICustomerOnSAServiceClient customerOnSaService)
    {
        _householdService = householdService;
        _customerOnSaService = customerOnSaService;
    }

    public async Task LoadData(InputParameters input, AggregatedData data, CancellationToken cancellationToken)
    {
        if (!input.SalesArrangementId.HasValue)
            throw new ArgumentNullException(nameof(InputParameters.SalesArrangementId));

        var households = await _householdService.GetHouseholdList(input.SalesArrangementId.Value, cancellationToken);

        var householdsByType = households.ToLookup(x => (HouseholdTypes)x.HouseholdTypeId);

        data.HouseholdMain = await CreateHouseholdDto(householdsByType[HouseholdTypes.Main].First(), cancellationToken);

        var householdCodebtor = householdsByType[HouseholdTypes.Codebtor].FirstOrDefault();
        if (householdCodebtor is not null)
            data.HouseholdCodebtor = await CreateHouseholdDto(householdCodebtor, cancellationToken);

    }

    private async Task<HouseholdDto> CreateHouseholdDto(Household household, CancellationToken cancellationToken)
    {
        CustomerOnSA? customer1 = null;
        CustomerOnSA? customer2 = null;

        if (household.CustomerOnSAId1.HasValue)
            customer1 = await _customerOnSaService.GetCustomer(household.CustomerOnSAId1.Value, cancellationToken);

        if (household.CustomerOnSAId2.HasValue)
            customer2 = await _customerOnSaService.GetCustomer(household.CustomerOnSAId2.Value, cancellationToken);

        return new HouseholdDto
        {
            Household = household,
            CustomerOnSa1 = customer1,
            CustomerOnSa2 = customer2
        };
    }
}