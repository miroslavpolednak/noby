using CIS.Foms.Types;
using CIS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.UserService.Api.Database;

internal sealed class UserServiceDbContext
    : BaseDbContext<UserServiceDbContext>
{
    public UserServiceDbContext(BaseDbContextAggregate<UserServiceDbContext> aggregate)
        : base(aggregate) { }

    public DbSet<UserIdentity> UserIdentities { get; set; }
}
