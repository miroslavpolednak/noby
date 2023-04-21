using Dapper;
using System.Data.SqlClient;

namespace NOBY.Infrastructure.Services.FlowSwitches;

internal sealed class FlowSwitchesCache
    : IFlowSwitchesCache
{
    public IReadOnlyCollection<FlowSwitchDefault> FlowSwitches { get; private set; }
    public IReadOnlyCollection<FlowSwitchGroupDefault> FlowSwitchesGroups { get; private set; }

    public FlowSwitchesCache(string connectionString)
    {
        // nahrat
        using var connection = new SqlConnection(connectionString);
        connection.Open();
        var grid = connection.QueryMultiple(_sqlQuery);

        FlowSwitches = grid.Read<FlowSwitchDefault>().ToArray().AsReadOnly();
        var groups = grid.Read<FlowSwitchGroupDefault>().ToArray();
        var bindings = grid.Read<dynamic>();

        foreach (var group in groups)
        {
            var isVisible = bindings.Where(t => t.FlowSwitchGroupId == group.FlowSwitchGroupId && t.GroupType == 1);
            if (isVisible.Any())
            {
                group.IsVisibleFlowSwitches = isVisible.ToDictionary(k => (int)k.FlowSwitchId, v => (bool)v.Value).AsReadOnly();
            }

            var isActive = bindings.Where(t => t.FlowSwitchGroupId == group.FlowSwitchGroupId && t.GroupType == 2);
            if (isActive.Any())
            {
                group.IsActiveFlowSwitches = isActive.ToDictionary(k => (int)k.FlowSwitchId, v => (bool)v.Value).AsReadOnly();
            }

            var isCompleted = bindings.Where(t => t.FlowSwitchGroupId == group.FlowSwitchGroupId && t.GroupType == 3);
            if (isCompleted.Any())
            {
                group.IsCompletedFlowSwitches = isCompleted.ToDictionary(k => (int)k.FlowSwitchId, v => (bool)v.Value).AsReadOnly();
            }
        }

        FlowSwitchesGroups = groups.AsReadOnly();
    }

    private const string _sqlQuery = @"
SELECT FlowSwitchId, DefaultValue FROM dbo.FlowSwitch;
SELECT FlowSwitchGroupId, IsActiveDefault, IsVisibleDefault, IsCompletedDefault FROM dbo.FlowSwitchGroup;
SELECT * FROM dbo.FlowSwitch2Group;";
}
