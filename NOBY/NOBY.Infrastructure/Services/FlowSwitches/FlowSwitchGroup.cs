namespace NOBY.Infrastructure.Services.FlowSwitches;

internal sealed class FlowSwitchGroup
{ 
    public int FlowSwitchGroupId { get; set; }
    public bool IsActiveDefault { get; set; }
    public bool IsVisibleDefault { get; set; }
    public bool IsCompletedDefault { get; set; }
    public IReadOnlyDictionary<int, bool>? IsActiveFlowSwitches { get; set; }
    public IReadOnlyDictionary<int, bool>? IsVisibleFlowSwitches { get; set; }
    public IReadOnlyDictionary<int, bool>? IsCompletedFlowSwitches { get; set; }
}
