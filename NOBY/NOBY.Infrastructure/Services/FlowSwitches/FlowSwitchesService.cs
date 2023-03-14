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

    public Dictionary<CIS.Foms.Enums.FlowSwitchesGroups, FlowSwitchGroup> GetFlowSwitchesGroups(IList<DomainServices.SalesArrangementService.Contracts.FlowSwitch> flowSwitchesOnSA)
    {
        flowSwitchesOnSA ??= new List<DomainServices.SalesArrangementService.Contracts.FlowSwitch>();
        var result = new Dictionary<CIS.Foms.Enums.FlowSwitchesGroups, FlowSwitchGroup>();

        foreach (var group in _flowSwitchesCache.FlowSwitchesGroups)
        {
            var resultGroup = new FlowSwitchGroup
            {
                IsVisible = resolveStatus(group.IsVisibleFlowSwitches, group.IsVisibleDefault),
                IsActive = resolveStatus(group.IsActiveFlowSwitches, group.IsActiveDefault),
                IsCompleted = resolveStatus(group.IsCompletedFlowSwitches, group.IsCompletedDefault)
            };

            result.Add((CIS.Foms.Enums.FlowSwitchesGroups)group.FlowSwitchGroupId, resultGroup);
        }

        return result;

        bool resolveStatus(IReadOnlyDictionary<int, bool>? groupSwitches, bool groupDefaultValue)
        {
            return groupSwitches is null || groupSwitches.Count == 0
                ? groupDefaultValue
                : groupSwitches.All(t => flowSwitchesOnSA.Any(x => x.FlowSwitchId == t.Key && x.Value == t.Value));
        }
    }

    private readonly IFlowSwitchesCache _flowSwitchesCache;

    public FlowSwitchesService(IFlowSwitchesCache flowSwitchesCache)
    {
        _flowSwitchesCache = flowSwitchesCache;
    }
}
