using Microsoft.EntityFrameworkCore;

namespace CIS.Infrastructure.Data;

public sealed class BaseDbContextAggregate<TDbContext> where TDbContext : DbContext
{
    internal Core.Security.ICurrentUserAccessor CurrentUser { get; init; }
    internal CIS.Core.IDateTime DateTime { get; init; }
    internal DbContextOptions Options { get; init; }
    internal CisEntityFrameworkOptions<TDbContext> CisOptions { get; init; }
    
    public BaseDbContextAggregate(
        DbContextOptions<TDbContext> options, 
        CisEntityFrameworkOptions<TDbContext> cisOptions, 
        Core.Security.ICurrentUserAccessor userProvider, 
        CIS.Core.IDateTime dateTime)
    {
        CisOptions = cisOptions;
        Options = options;
        DateTime = dateTime;
        CurrentUser = userProvider;
    }
}