using Microsoft.EntityFrameworkCore;
using CIS.Infrastructure.Data;

namespace DomainServices.CaseService.Api.Repositories;

internal sealed class CaseServiceDbContext : BaseDbContext
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public CaseServiceDbContext(DbContextOptions<CaseServiceDbContext> options, CIS.Core.Security.ICurrentUserAccessor userProvider)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        : base(options, userProvider) { }

    public DbSet<Entities.CaseInstance> CaseInstances { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.RegisterCisTemporalTable<Entities.CaseInstance>();
    }
}
