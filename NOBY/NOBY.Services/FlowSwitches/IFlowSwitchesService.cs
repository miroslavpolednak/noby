namespace NOBY.Services.FlowSwitches;

public interface IFlowSwitchesService
{
    Task<List<DomainServices.SalesArrangementService.Contracts.FlowSwitch>> GetFlowSwitchesForSA(int salesArrangementId, CancellationToken cancellationToken = default);

    Dictionary<FlowSwitchesGroups, ApiContracts.Dto.FlowSwitchGroup> GetFlowSwitchesGroups(IList<DomainServices.SalesArrangementService.Contracts.FlowSwitch> flowSwitchesOnSA);
}
