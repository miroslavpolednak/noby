using CIS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using NOBY.Database.Entities;

namespace NOBY.Database;

public sealed class FeApiDbContext
    : BaseDbContext<FeApiDbContext>
{
    public FeApiDbContext(BaseDbContextAggregate<FeApiDbContext> aggregate)
        : base(aggregate) { }

    public DbSet<FeAvailableUserPermission> FeAvailableUserPermissions { get; set; }
    public DbSet<FlowSwitch> FlowSwitches { get; set; }
    public DbSet<FlowSwitch2Group> FlowSwitches2Groups { get; set; }
    public DbSet<FlowSwitchGroup> FlowSwitchGroups { get; set; }
    public DbSet<TempFile> TempFiles { get; set; }
}
