namespace NOBY.Infrastructure.Services.FlowSwitches;

public interface IFlowSwitchesService
{
    List<Dto.FlowSwitches.FlowSwitch> GetDefaultSwitches();

    Dictionary<CIS.Foms.Enums.FlowSwitchesGroups, Dto.FlowSwitches.FlowSwitchGroup> GetFlowSwitchesGroups(IList<DomainServices.SalesArrangementService.Contracts.FlowSwitch> flowSwitchesOnSA);
}
