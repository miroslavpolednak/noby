using CIS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using NOBY.Api.Database.Entities;

namespace NOBY.Api.Database;

internal sealed class FeApiDbContext
    : BaseDbContext<FeApiDbContext>
{
    public FeApiDbContext(BaseDbContextAggregate<FeApiDbContext> aggregate)
        : base(aggregate) { }

    public DbSet<FeAvailableUserPermission> FeAvailableUserPermissions { get; set; }
}
