using SharedTypes.Enums;

namespace DomainServices.SalesArrangementService.Clients;

public interface IFlowSwitchManager
{
    void AddFlowSwitch(FlowSwitches flowSwitchId, bool? value);
    Task SaveFlowSwitches(int salesArrangementId, CancellationToken cancellationToken = default);
}
