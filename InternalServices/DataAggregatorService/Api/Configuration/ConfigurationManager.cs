using CIS.InternalServices.DataAggregatorService.Api.Configuration.Data;
using CIS.InternalServices.DataAggregatorService.Api.Configuration.Document;
using CIS.InternalServices.DataAggregatorService.Api.Configuration.EasForm;

namespace CIS.InternalServices.DataAggregatorService.Api.Configuration;

[TransientService, SelfService]
internal class ConfigurationManager
{
    private readonly ConfigurationRepository _repository;

    public ConfigurationManager(ConfigurationRepository repository)
    {
        _repository = repository;
    }

    public async Task<DocumentConfiguration> LoadDocumentConfiguration(int documentId, string documentVersion)
    {
        var fields = await _repository.LoadDocumentSourceFields(documentId, documentVersion);
        var tables = await _repository.LoadDocumentTables(documentId, documentVersion);

        return new DocumentConfiguration
        {
            InputConfig = new InputConfig
            {
                DataSources = GetDataSources(UnionFieldAndTableSources()),
                DynamicInputParameters = await _repository.LoadDocumentDynamicInputFields(documentId, documentVersion)
            },
            SourceFields = fields,
            DynamicStringFormats = await _repository.LoadDocumentDynamicStringFormats(documentId, documentVersion),
            Tables = tables
        };

        IEnumerable<DataSource> UnionFieldAndTableSources() => 
            fields.Select(f => f.DataSource).Union(tables.Select(t => t.DataSource));
    }

    public async Task<EasFormConfiguration> LoadEasFormConfiguration(int easFormRequestType)
    {
        var fields = await _repository.LoadEasFormSourceFields(easFormRequestType);

        return new EasFormConfiguration
        {
            InputConfig = new InputConfig
            {
                DataSources = GetDataSources(fields.Select(f => f.DataSource)),
                DynamicInputParameters = await _repository.LoadEasFormDynamicInputFields(easFormRequestType)
            },
            SourceFields = fields
        };
    }

    private static IEnumerable<DataSource> GetDataSources(IEnumerable<DataSource> dataSources) =>
        dataSources.Where(d => d != DataSource.General).Distinct();
}