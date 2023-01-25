using CIS.Infrastructure.Data;
using DomainServices.DocumentOnSAService.Api.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.DocumentOnSAService.Api.Database;

public class DocumentOnSAServiceDbContext : BaseDbContext<DocumentOnSAServiceDbContext>
{
    public DocumentOnSAServiceDbContext(BaseDbContextAggregate<DocumentOnSAServiceDbContext> aggregate) : base(aggregate)
    {
    }

    public DbSet<GeneratedFormId> GeneratedFormId { get; set; }

    public DbSet<DocumentOnSa> DocumentOnSa { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Program).Assembly);
    }
}
