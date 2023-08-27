using DomainServices.HouseholdService.Clients;
using DomainServices.HouseholdService.Contracts;
using DomainServices.SalesArrangementService.Clients;

namespace NOBY.Services.FlowSwitchAtLeastOneIncomeMainHousehold;

[ScopedService, SelfService]
public sealed class FlowSwitchAtLeastOneIncomeMainHouseholdService
{
    public async Task SetFlowSwitchByCustomerOnSAId(int customerOnSAId, bool useFlowSwitchManager = false, CancellationToken cancellationToken = default)
    {
    }

    public async Task SetFlowSwitchByHouseholdId(int householdId, bool useFlowSwitchManager = false, CancellationToken cancellationToken = default)
    {
        List<IncomeInList> incomes = new();

        var household = await _householdService.GetHousehold(householdId, cancellationToken);
        
        if (household.CustomerOnSAId1.HasValue)
        {
            incomes.AddRange(await _customerOnSAService.GetIncomeList(household.CustomerOnSAId1.Value, cancellationToken));
        }
        if (household.CustomerOnSAId2.HasValue && !incomes.Any())
        {
            incomes.AddRange(await _customerOnSAService.GetIncomeList(household.CustomerOnSAId2.Value, cancellationToken));
        }

        if (incomes.Any() && household.HouseholdTypeId == (int)HouseholdTypes.Main)
        {
            await saveSwitch(household.SalesArrangementId, useFlowSwitchManager, true, cancellationToken);
        }
        else if (!incomes.Any() && household.HouseholdTypeId == (int)HouseholdTypes.Main)
        {
            await saveSwitch(household.SalesArrangementId, useFlowSwitchManager, false, cancellationToken);
        }
    }

    private async Task saveSwitch(int salesArrangementId, bool useFlowSwitchManager, bool flowSwitchValue, CancellationToken cancellationToken)
    {
        if (useFlowSwitchManager)
        {
            _flowSwitchManager.AddFlowSwitch(CIS.Foms.Enums.FlowSwitches.AtLeast1IncomeMainHousehold, flowSwitchValue);
        }
        else
        {
            await _salesArrangementService.SetFlowSwitches(salesArrangementId, new()
            {
                new() 
                {
                    FlowSwitchId = (int)CIS.Foms.Enums.FlowSwitches.AtLeast1IncomeMainHousehold, 
                    Value = flowSwitchValue 
                }
            }, cancellationToken);
        }
    }

    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IHouseholdServiceClient _householdService;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;
    private readonly IFlowSwitchManager _flowSwitchManager;

    public FlowSwitchAtLeastOneIncomeMainHouseholdService(
        ISalesArrangementServiceClient service,
        IHouseholdServiceClient householdService,
        ICustomerOnSAServiceClient customerOnSAService,
        IFlowSwitchManager flowSwitchManager)
    {
        _salesArrangementService = service;
        _householdService = householdService;
        _customerOnSAService = customerOnSAService;
        _flowSwitchManager = flowSwitchManager;
    }
}
