using CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Data;

internal class ConfigurationContext : DbContext
{
    public ConfigurationContext()
    {
    }

    public ConfigurationContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<DataField> DataFields => Set<DataField>();

    public DbSet<DataService> DataServices => Set<DataService>();

    public DbSet<Document> Documents => Set<Document>();

    public DbSet<DocumentDataField> DocumentDataFields => Set<DocumentDataField>();

    public DbSet<DocumentDynamicInputParameter> DocumentDynamicInputParameters => Set<DocumentDynamicInputParameter>();

    public DbSet<InputParameter> InputParameter => Set<InputParameter>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            entityType.SetTableName(entityType.DisplayName());
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Data Source=localhost;Initial Catalog=DataAggregator;Persist Security Info=True;User ID=SA;Password=Test123456");
    }
}