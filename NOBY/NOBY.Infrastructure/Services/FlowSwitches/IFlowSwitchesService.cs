namespace NOBY.Infrastructure.Services.FlowSwitches;

public interface IFlowSwitchesService
{
    List<FlowSwitch> GetDefaultSwitches();

    Dictionary<CIS.Foms.Enums.FlowSwitchesGroups, FlowSwitchGroup> GetFlowSwitchesGroups(IList<DomainServices.SalesArrangementService.Contracts.FlowSwitch> flowSwitchesOnSA);
}
