using Microsoft.EntityFrameworkCore;

namespace CIS.Infrastructure.Data;

public sealed class BaseDbContextAggregate<TDbContext> where TDbContext 
    : DbContext
{
    internal Core.Security.ICurrentUserAccessor CurrentUser { get; init; }
    internal TimeProvider ContextTimeProvider { get; init; }
    internal DbContextOptions Options { get; init; }
    internal CisEntityFrameworkOptions<TDbContext> CisOptions { get; init; }
    
    public BaseDbContextAggregate(
        DbContextOptions<TDbContext> options, 
        CisEntityFrameworkOptions<TDbContext> cisOptions, 
        Core.Security.ICurrentUserAccessor userProvider, 
        TimeProvider timeProvider)
    {
        CisOptions = cisOptions;
        Options = options;
        ContextTimeProvider = timeProvider;
        CurrentUser = userProvider;
    }
}