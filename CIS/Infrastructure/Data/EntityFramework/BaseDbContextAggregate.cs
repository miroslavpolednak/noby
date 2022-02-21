using Microsoft.EntityFrameworkCore;

namespace CIS.Infrastructure.Data;

public sealed class BaseDbContextAggregate
{
    internal CisEntityFrameworkOptions CisOptions { get; init; }
    internal Core.Security.ICurrentUserAccessor CurrentUser { get; init; }
    internal CIS.Core.IDateTime DateTime { get; init; }
    internal DbContextOptions Options { get; init; }

    public BaseDbContextAggregate(DbContextOptions options, CisEntityFrameworkOptions cisOptions, Core.Security.ICurrentUserAccessor userProvider, CIS.Core.IDateTime dateTime)
    {
        CisOptions = cisOptions;
        Options = options;
        DateTime = dateTime;
        CurrentUser = userProvider;
    }
}