using CIS.Foms.Enums;
using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Clients.Services;

internal sealed class FlowSwitchManager
    : IFlowSwitchManager
{
    private readonly ISalesArrangementServiceClient _client;
    private List<EditableFlowSwitch> _flowSwitches = new();

    public FlowSwitchManager(ISalesArrangementServiceClient client)
    {
        _client = client;
    }

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
        if (_flowSwitches.Any())
        {
            await _client.SetFlowSwitches(salesArrangementId, _flowSwitches, cancellationToken);
        }
    }
}
