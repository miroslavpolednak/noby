using CIS.InternalServices.DataAggregatorService.Api.Configuration.Data;
using CIS.InternalServices.DataAggregatorService.Api.Configuration.Document;
using CIS.InternalServices.DataAggregatorService.Api.Configuration.EasForm;

namespace CIS.InternalServices.DataAggregatorService.Api.Configuration;

[TransientService, AsImplementedInterfacesService]
internal class ConfigurationManager : IConfigurationManager
{
    private readonly ConfigurationRepositoryFactory _repositoryFactory;

    public ConfigurationManager(ConfigurationRepositoryFactory repositoryFactory)
    {
        _repositoryFactory = repositoryFactory;
    }

    public async Task<DocumentConfiguration> LoadDocumentConfiguration(DocumentKey documentKey, CancellationToken cancellationToken)
    {
        var repository = _repositoryFactory.CreateDocumentRepository();

        var fields = await repository.LoadDocumentSourceFields(documentKey.TypeId, documentKey.VersionData.VersionName, documentKey.VersionData.VariantName, cancellationToken);
        var tables = await repository.LoadDocumentTables(documentKey.TypeId, documentKey.VersionData.VersionName, cancellationToken);

        return new DocumentConfiguration
        {
            DocumentTemplateVersionId = documentKey.VersionData.VersionId,
            DocumentTemplateVariantId = documentKey.VersionData.VariantId,
            InputConfig = new InputConfig
            {
                DataSources = GetDataSources(UnionFieldAndTableSources()),
                DynamicInputParameters = await repository.LoadDocumentDynamicInputFields(documentKey.TypeId, documentKey.VersionData.VersionName, documentKey.VersionData.VariantName, cancellationToken)
            },
            SourceFields = fields,
            DynamicStringFormats = await repository.LoadDocumentDynamicStringFormats(documentKey.TypeId, documentKey.VersionData.VersionName, cancellationToken),
            Tables = tables
        };

        IEnumerable<DataSource> UnionFieldAndTableSources() => 
            fields.Select(f => f.DataSource).Union(tables.Select(t => t.DataSource));
    }

    public async Task<EasFormConfiguration> LoadEasFormConfiguration(EasFormKey easFormKey, CancellationToken cancellationToken)
    {
        var repository = _repositoryFactory.CreateEasFormConfigurationRepository();

        var fields = await repository.LoadEasFormSourceFields(easFormKey.RequestTypeId, cancellationToken);

        return new EasFormConfiguration
        {
            EasFormKey = easFormKey,
            InputConfig = new InputConfig
            {
                DataSources = GetDataSources(fields.Select(f => f.DataSource)),
                DynamicInputParameters = await repository.LoadEasFormDynamicInputFields(easFormKey.RequestTypeId, cancellationToken)
            },
            SourceFields = fields
        };
    }

    private static IEnumerable<DataSource> GetDataSources(IEnumerable<DataSource> dataSources) =>
        dataSources.Where(d => d != DataSource.General).Distinct();
}