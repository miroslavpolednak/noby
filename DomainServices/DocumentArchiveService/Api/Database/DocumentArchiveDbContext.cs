using CIS.Infrastructure.Data;
using DomainServices.DocumentArchiveService.Api.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.DocumentArchiveService.Api.Database;

public class DocumentArchiveDbContext : BaseDbContext<DocumentArchiveDbContext>
{
    public DocumentArchiveDbContext(BaseDbContextAggregate<DocumentArchiveDbContext> aggregate) : base(aggregate)
    {
    }

    public DbSet<DocumentInterface> DocumentInterface { get; set; }

    public DbSet<FormInstanceInterface> FormInstanceInterface { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Program).Assembly);
    }

}
