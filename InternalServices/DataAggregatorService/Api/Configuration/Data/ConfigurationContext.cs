using CIS.InternalServices.DataAggregatorService.Api.Configuration.Data.Entities;
using Microsoft.EntityFrameworkCore;
using EasFormType = CIS.InternalServices.DataAggregatorService.Api.Configuration.Data.Entities.EasFormType;

namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Data;

internal class ConfigurationContext : DbContext
{
    public ConfigurationContext(DbContextOptions<ConfigurationContext> options) : base(options)
    {
    }

    public DbSet<DataService> DataServices => Set<DataService>();

    public DbSet<DataField> DataFields => Set<DataField>();

    public DbSet<Entities.Document> Documents => Set<Entities.Document>();

    public DbSet<DocumentDataField> DocumentDataFields => Set<DocumentDataField>();

    public DbSet<DocumentDataFieldVariant> DocumentDataFieldVariants => Set<DocumentDataFieldVariant>();

    public DbSet<DocumentDynamicInputParameter> DocumentDynamicInputParameters => Set<DocumentDynamicInputParameter>();

    public DbSet<DocumentSpecialDataField> DocumentSpecialDataFields => Set<DocumentSpecialDataField>();

    public DbSet<DocumentSpecialDataFieldVariant> DocumentSpecialDataFieldVariants => Set<DocumentSpecialDataFieldVariant>();

    public DbSet<DocumentTable> DocumentTables => Set<DocumentTable>();

    public DbSet<DocumentTableColumn> DocumentTableColumns => Set<DocumentTableColumn>();

    public DbSet<DynamicStringFormat> DynamicStringFormats => Set<DynamicStringFormat>();

    public DbSet<EasFormDataField> EasFormDataFields => Set<EasFormDataField>();

    public DbSet<EasFormType> EasFormTypes => Set<EasFormType>();

    public DbSet<EasRequestType> EasRequestTypes => Set<EasRequestType>();

    public DbSet<EasFormDynamicInputParameter> EasFormDynamicInputParameters => Set<EasFormDynamicInputParameter>();

    public DbSet<EasFormSpecialDataField> EasFormSpecialDataFields => Set<EasFormSpecialDataField>(); 

    public DbSet<InputParameter> InputParameters => Set<InputParameter>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            entityType.SetTableName(entityType.DisplayName());
        }
    }
}