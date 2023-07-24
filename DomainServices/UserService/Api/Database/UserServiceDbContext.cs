using CIS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.UserService.Api.Database;

internal sealed class UserServiceDbContext
    : BaseDbContext<UserServiceDbContext>
{
    public UserServiceDbContext(BaseDbContextAggregate<UserServiceDbContext> aggregate)
        : base(aggregate) { }

    public DbSet<Entities.DbUserIdentity> UserIdentities { get; set; }
    public DbSet<Entities.DbUserAttribute> DbUserAttributes { get; set; }
    public DbSet<Entities.DbUserPermission> DbUserPermissions { get; set; }
    public DbSet<Entities.DbUserRIPAttribute> DbUserRIPAttributes { get; set;}
}
