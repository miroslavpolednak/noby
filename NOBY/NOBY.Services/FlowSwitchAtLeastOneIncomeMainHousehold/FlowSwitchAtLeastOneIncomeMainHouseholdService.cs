using DomainServices.HouseholdService.Clients.v1;
using DomainServices.HouseholdService.Contracts;
using DomainServices.SalesArrangementService.Clients;

namespace NOBY.Services.FlowSwitchAtLeastOneIncomeMainHousehold;

[ScopedService, SelfService]
public sealed class FlowSwitchAtLeastOneIncomeMainHouseholdService(
    ISalesArrangementServiceClient _salesArrangementService,
    IHouseholdServiceClient _householdService,
    ICustomerOnSAServiceClient _customerOnSAService)
{
    public async Task SetFlowSwitchByCustomerOnSAId(int customerOnSAId, IFlowSwitchManager? flowSwitchManager = null, CancellationToken cancellationToken = default)
    {
        var householdId = await _householdService.GetHouseholdIdByCustomerOnSAId(customerOnSAId, cancellationToken);
        await SetFlowSwitchByHouseholdId(householdId.Value, flowSwitchManager, cancellationToken);
    }

    public async Task SetFlowSwitchByHouseholdId(int householdId, IFlowSwitchManager? flowSwitchManager = null, CancellationToken cancellationToken = default)
    {
        List<IncomeInList> incomes = [];

        var household = await _householdService.GetHousehold(householdId, cancellationToken);
        
        if (household.CustomerOnSAId1.HasValue)
        {
            incomes.AddRange(await _customerOnSAService.GetIncomeList(household.CustomerOnSAId1.Value, cancellationToken));
        }
        if (household.CustomerOnSAId2.HasValue && incomes.Count == 0)
        {
            incomes.AddRange(await _customerOnSAService.GetIncomeList(household.CustomerOnSAId2.Value, cancellationToken));
        }

        if (incomes.Count != 0 && household.HouseholdTypeId == (int)HouseholdTypes.Main)
        {
            await saveSwitch(household.SalesArrangementId, flowSwitchManager, true, cancellationToken);
        }
        else if (incomes.Count == 0 && household.HouseholdTypeId == (int)HouseholdTypes.Main)
        {
            await saveSwitch(household.SalesArrangementId, flowSwitchManager, false, cancellationToken);
        }
    }

    private async Task saveSwitch(int salesArrangementId, IFlowSwitchManager? flowSwitchManager, bool flowSwitchValue, CancellationToken cancellationToken)
    {
        if (flowSwitchManager is not null)
        {
            flowSwitchManager.AddFlowSwitch(SharedTypes.Enums.FlowSwitches.AtLeast1IncomeMainHousehold, flowSwitchValue);
        }
        else
        {
            await _salesArrangementService.SetFlowSwitch(salesArrangementId, SharedTypes.Enums.FlowSwitches.AtLeast1IncomeMainHousehold, flowSwitchValue, cancellationToken);
        }
    }
}
