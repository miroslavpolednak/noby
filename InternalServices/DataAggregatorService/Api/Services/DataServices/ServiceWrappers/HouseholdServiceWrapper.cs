using CIS.Foms.Enums;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.Dto;
using DomainServices.HouseholdService.Clients;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.ServiceWrappers;

[TransientService, SelfService]
internal class HouseholdServiceWrapper : IServiceWrapper
{
    private readonly IHouseholdServiceClient _householdService;

    public HouseholdServiceWrapper(IHouseholdServiceClient householdService)
    {
        _householdService = householdService;
    }

    public async Task LoadData(InputParameters input, AggregatedData data, CancellationToken cancellationToken)
    {
        if (!input.SalesArrangementId.HasValue)
            throw new ArgumentNullException(nameof(InputParameters.SalesArrangementId));

        var households = await _householdService.GetHouseholdList(input.SalesArrangementId.Value, cancellationToken);

        var householdsByType = households.ToLookup(x => (HouseholdTypes)x.HouseholdTypeId);

        data.HouseholdMain = new HouseholdDto { Household = householdsByType[HouseholdTypes.Main].First() };

        var householdCodebtor = householdsByType[HouseholdTypes.Codebtor].FirstOrDefault();
        if (householdCodebtor is not null)
            data.HouseholdCodebtor = new HouseholdDto { Household = householdCodebtor };
    }
}