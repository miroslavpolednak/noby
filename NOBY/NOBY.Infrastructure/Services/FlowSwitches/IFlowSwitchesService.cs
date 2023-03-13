namespace NOBY.Infrastructure.Services.FlowSwitches;

public interface IFlowSwitchesService
{
    List<FlowSwitch> GetDefaultSwitches();

    void GetFlowSwitchesGroups(int SalesArrangementId);
}
