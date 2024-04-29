using SharedTypes.Enums;
using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Clients;

internal sealed class FlowSwitchManager(ISalesArrangementServiceClient _client)
    : IFlowSwitchManager
{
    private List<EditableFlowSwitch> _flowSwitches = [];

    public void AddFlowSwitch(FlowSwitches flowSwitchId, bool? value)
    {
        _flowSwitches.Add(new EditableFlowSwitch
        {
            FlowSwitchId = (int)flowSwitchId,
            Value = value
        });
    }

    public async Task SaveFlowSwitches(int salesArrangementId, CancellationToken cancellationToken = default)
    {
        if (_flowSwitches.Count != 0)
        {
            await _client.SetFlowSwitches(salesArrangementId, _flowSwitches, cancellationToken);
        }
    }
}
