using CIS.Infrastructure.Data;
using DomainServices.SalesArrangementService.Api.Database.DocumentArchiveService.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace DomainServices.SalesArrangementService.Api.Database.DocumentArchiveService;

public class DocumentArchiveServiceDbContext : BaseDbContext<DocumentArchiveServiceDbContext>
{
    public DocumentArchiveServiceDbContext(BaseDbContextAggregate<DocumentArchiveServiceDbContext> aggregate)
        : base(aggregate)
    {
    }
    
    public DbSet<DocumentInterface> DocumentInterface { get; set; }

    public DbSet<FormInstanceInterface> FormInstanceInterface { get; set; }

    protected override void OnModelCreating([NotNull] ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Program).Assembly);
    }

}
