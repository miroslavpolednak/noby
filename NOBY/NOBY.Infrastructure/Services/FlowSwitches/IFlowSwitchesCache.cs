namespace NOBY.Infrastructure.Services.FlowSwitches;

internal interface IFlowSwitchesCache
{
    IReadOnlyCollection<FlowSwitchDefault> FlowSwitches { get; }
    IReadOnlyCollection<FlowSwitchGroupDefault> FlowSwitchesGroups { get; }
}
