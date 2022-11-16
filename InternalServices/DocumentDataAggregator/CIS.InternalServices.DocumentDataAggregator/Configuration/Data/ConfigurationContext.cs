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

    public DbSet<DataService> DataServices => Set<DataService>();

    public DbSet<Entities.Document> Documents => Set<Entities.Document>();

    public DbSet<DocumentDataField> DocumentDataFields => Set<DocumentDataField>();

    public DbSet<DocumentDynamicInputParameter> DocumentDynamicInputParameters => Set<DocumentDynamicInputParameter>();

    public DbSet<DocumentSpecialDataField> DocumentSpecialDataFields => Set<DocumentSpecialDataField>();

    public DbSet<DynamicStringFormat> DynamicStringFormats => Set<DynamicStringFormat>();

    public DbSet<EasFormDataField> EasFormDataFields => Set<EasFormDataField>();

    public DbSet<EasFormDynamicInputParameter> EasFormDynamicInputParameters => Set<EasFormDynamicInputParameter>();

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
            "Data Source=localhost;Initial Catalog=DataAggregator;Persist Security Info=True;User ID=SA;Password=Test123456;TrustServerCertificate=True");
    }
}