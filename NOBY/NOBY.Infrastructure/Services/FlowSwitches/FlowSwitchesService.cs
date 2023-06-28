using LazyCache;
using Microsoft.EntityFrameworkCore;

namespace NOBY.Infrastructure.Services.FlowSwitches;

[CIS.Core.Attributes.TransientService, CIS.Core.Attributes.AsImplementedInterfacesService]
internal sealed class FlowSwitchesService
    : IFlowSwitchesService
{
    public List<Dto.FlowSwitches.FlowSwitch> GetDefaultSwitches()
    {
        return getFlowSwitches()
            .Where(t => t.DefaultValue)
            .Select(t => new Dto.FlowSwitches.FlowSwitch
            {
                FlowSwitchId = t.FlowSwitchId,
                Value = t.DefaultValue
            })
            .ToList();
    }

    public Dictionary<CIS.Foms.Enums.FlowSwitchesGroups, Dto.FlowSwitches.FlowSwitchGroup> GetFlowSwitchesGroups(IList<DomainServices.SalesArrangementService.Contracts.FlowSwitch> flowSwitchesOnSA)
    {
        flowSwitchesOnSA ??= new List<DomainServices.SalesArrangementService.Contracts.FlowSwitch>();
        var result = new Dictionary<CIS.Foms.Enums.FlowSwitchesGroups, Dto.FlowSwitches.FlowSwitchGroup>();

        var allFlowSwitches = getFlowSwitches();
        var allFlowSwitchGroups = getFlowSwitchGroups();

        foreach (var group in allFlowSwitchGroups)
        {
            var resultGroup = new Dto.FlowSwitches.FlowSwitchGroup
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
            return _dbContext.FlowSwitches.AsNoTracking().Select(t => new FlowSwitchDefault(t.FlowSwitchId, t.DefaultValue)).ToArray();
        }, DateTime.Now.AddDays(1));
    }

    public FlowSwitchGroupDefault[] getFlowSwitchGroups()
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

    private readonly Database.FeApiDbContext _dbContext;
    private readonly IAppCache _cache;
    
    public FlowSwitchesService(Database.FeApiDbContext dbContext, IAppCache cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }
}
