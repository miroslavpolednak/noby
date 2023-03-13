using CIS.Core.Data;

namespace NOBY.Infrastructure.Services.FlowSwitches;

internal sealed class FlowSwitchesService
    : IFlowSwitchesService
{
    public List<FlowSwitch> GetDefaultSwitches()
    {
        return _flowSwitchesCache.FlowSwitches
            .Where(t => t.DefaultValue)
            .Select(t =>new FlowSwitch
            {
                FlowSwitchId = t.FlowSwitchId,
                Value = t.DefaultValue
            })
            .ToList();
    }

    public void GetFlowSwitchesGroups(int SalesArrangementId)
    {
        var allSwitches = _flowSwitchesCache.FlowSwitches;
    }

    private readonly IFlowSwitchesCache _flowSwitchesCache;

    public FlowSwitchesService(IFlowSwitchesCache flowSwitchesCache)
    {
        _flowSwitchesCache = flowSwitchesCache;
    }
}
