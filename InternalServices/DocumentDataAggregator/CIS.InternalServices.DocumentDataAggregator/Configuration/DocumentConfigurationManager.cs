using CIS.InternalServices.DocumentDataAggregator.Configuration.Data;

namespace CIS.InternalServices.DocumentDataAggregator.Configuration;

[TransientService, SelfService]
internal class DocumentConfigurationManager
{
    private readonly ConfigurationRepository _repository;

    public DocumentConfigurationManager(ConfigurationRepository repository)
    {
        _repository = repository;
    }

    public async Task<DocumentConfiguration> LoadDocumentConfiguration(int documentId, string documentVersion)
    {
        var fields = await _repository.LoadSourceFields(documentId, documentVersion);

        return new DocumentConfiguration
        {
            InputConfig = new InputConfig
            {
                DataSources = fields.Select(f => f.DataSource).Distinct(),
                DynamicInputParameters = await _repository.LoadDocumentDynamicInputFields(documentId, documentVersion)
            },
            SourceFields = fields,
            DynamicStringFormats = await _repository.LoadDocumentDynamicStringFormats(documentId, documentVersion)
        };
    }
}