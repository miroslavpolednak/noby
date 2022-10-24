using CIS.InternalServices.DocumentDataAggregator.Configuration.Data;
using CIS.InternalServices.DocumentDataAggregator.Configuration.Model;

namespace CIS.InternalServices.DocumentDataAggregator.Configuration;

[TransientService, SelfService]
internal class DocumentConfigurationManager
{
    private readonly ConfigurationRepository _repository;

    public DocumentConfigurationManager(ConfigurationRepository repository)
    {
        _repository = repository;
    }

    public async Task<DocumentConfiguration> LoadDocumentConfiguration()
    {
        var fields = await _repository.LoadSourceFields(1, 1);

        return new DocumentConfiguration
        {
            InputConfig = new InputConfig
            {
                DataSources = fields.Select(f => f.DataSource).Distinct(),
                DynamicInputParameters = await _repository.LoadDynamicInputFields(1, 1)
            },
            SourceFields = fields
        };
    }
}