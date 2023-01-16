using CIS.InternalServices.DataAggregatorService.Api.Configuration.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Data;

internal class ConfigurationContext : DbContext
{
    public ConfigurationContext(DbContextOptions<ConfigurationContext> options) : base(options)
    {
    }

    public DbSet<DataService> DataServices => Set<DataService>();

    public DbSet<Entities.Document> Documents => Set<Entities.Document>();

    public DbSet<DocumentDataField> DocumentDataFields => Set<DocumentDataField>();

    public DbSet<DocumentDynamicInputParameter> DocumentDynamicInputParameters => Set<DocumentDynamicInputParameter>();

    public DbSet<DocumentSpecialDataField> DocumentSpecialDataFields => Set<DocumentSpecialDataField>();

    public DbSet<DocumentTable> DocumentTables => Set<DocumentTable>();

    public DbSet<DynamicStringFormat> DynamicStringFormats => Set<DynamicStringFormat>();

    public DbSet<EasFormDataField> EasFormDataFields => Set<EasFormDataField>();

    public DbSet<EasFormDynamicInputParameter> EasFormDynamicInputParameters => Set<EasFormDynamicInputParameter>();

    public DbSet<EasFormSpecialDataField> EasFormSpecialDataFields => Set<EasFormSpecialDataField>(); 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            entityType.SetTableName(entityType.DisplayName());
        }
    }
}