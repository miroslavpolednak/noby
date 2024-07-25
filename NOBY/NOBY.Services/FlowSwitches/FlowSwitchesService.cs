using DomainServices.SalesArrangementService.Clients;
using LazyCache;
using Microsoft.EntityFrameworkCore;

namespace NOBY.Services.FlowSwitches;

[TransientService, AsImplementedInterfacesService]
internal sealed class FlowSwitchesService(
    Database.FeApiDbContext _dbContext, 
    IAppCache _cache, 
    ISalesArrangementServiceClient _salesArrangementService)
        : IFlowSwitchesService
{
    public async Task<List<DomainServices.SalesArrangementService.Contracts.FlowSwitch>> GetFlowSwitchesForSA(int salesArrangementId, CancellationToken cancellationToken = default)
    {
        var existingSwitches = await _salesArrangementService.GetFlowSwitches(salesArrangementId, cancellationToken);

        foreach (var fs in getFlowSwitches())
        {
            if (!existingSwitches.Any(t => t.FlowSwitchId == fs.FlowSwitchId))
            {
                existingSwitches.Add(new()
                {
                    FlowSwitchId = fs.FlowSwitchId,
                    Value = fs.DefaultValue
                });
            }
        }

        return existingSwitches;
    }

    public Dictionary<FlowSwitchesGroups, ApiContracts.Dto.FlowSwitchGroup> GetFlowSwitchesGroups(IList<DomainServices.SalesArrangementService.Contracts.FlowSwitch> flowSwitchesOnSA)
    {
        flowSwitchesOnSA ??= [];
        var result = new Dictionary<FlowSwitchesGroups, ApiContracts.Dto.FlowSwitchGroup>();

        var allFlowSwitches = getFlowSwitches();
        var allFlowSwitchGroups = getFlowSwitchGroups();

        foreach (var group in allFlowSwitchGroups)
        {
            var resultGroup = new ApiContracts.Dto.FlowSwitchGroup
            {
                IsVisible = resolveStatus(group.IsVisibleFlowSwitches, group.IsVisibleDefault),
                IsActive = resolveStatus(group.IsActiveFlowSwitches, group.IsActiveDefault),
                State = resolveStatus(group.IsCompletedFlowSwitches, group.IsCompletedDefault) ? 1 : 2
            };

            result.Add((FlowSwitchesGroups)group.FlowSwitchGroupId, resultGroup);
        }

        return result;

        bool resolveStatus(IReadOnlyDictionary<int, bool>? groupSwitches, bool groupDefaultValue)
        {
            return groupSwitches is null || groupSwitches.Count == 0
                ? groupDefaultValue
                : groupSwitches.All(t =>
                {
                    var f = flowSwitchesOnSA.FirstOrDefault(x => x.FlowSwitchId == t.Key);
                    if (f is not null)
                    {
                        return f.Value == t.Value;
                    }
                    else
                    {
                        return allFlowSwitches.First(x => x.FlowSwitchId == t.Key).DefaultValue == t.Value;
                    }
                });
        }
    }

    private FlowSwitchDefault[] getFlowSwitches()
    {
        return _cache.GetOrAdd("FlowSwitches", () =>
        {
            return _dbContext
                .FlowSwitches
                .AsNoTracking()
                .Select(t => new FlowSwitchDefault(t.FlowSwitchId, t.DefaultValue))
                .ToArray();
        }, DateTime.Now.AddDays(1));
    }

    /// <summary>
    /// Vychozi nastaveni kategorii klapek. Nastavi default pro kazdou skupinu a seznam klapek, ktere musi byt nastaveny aby byla nastavena i kategorie.
    /// </summary>
    private FlowSwitchGroupDefault[] getFlowSwitchGroups()
    {
        return _cache.GetOrAdd("FlowSwitchesGroups", () =>
        {
            var groups = _dbContext.FlowSwitchGroups.AsNoTracking().Select(t => new FlowSwitchGroupDefault
            {
                FlowSwitchGroupId = t.FlowSwitchGroupId,
                IsActiveDefault = t.IsActiveDefault,
                IsVisibleDefault = t.IsVisibleDefault,
                IsCompletedDefault = t.IsCompletedDefault
            }).ToArray();

            var bindings = _dbContext.FlowSwitches2Groups.AsNoTracking().ToList();

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

            return groups;
        }, DateTime.Now.AddDays(1));
    }
}
