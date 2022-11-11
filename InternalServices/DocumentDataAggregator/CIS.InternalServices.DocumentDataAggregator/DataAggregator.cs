using CIS.InternalServices.DocumentDataAggregator.DataServices;
using CIS.InternalServices.DocumentDataAggregator.Documents;
using CIS.InternalServices.DocumentDataAggregator.Documents.Mapper;

namespace CIS.InternalServices.DocumentDataAggregator;

internal class DataAggregator : IDataAggregator
{
    private readonly Configuration.DocumentConfigurationManager _configurationManager;
    private readonly DataServicesLoader _dataServicesLoader;

    public DataAggregator(Configuration.DocumentConfigurationManager configurationManager, DataServicesLoader dataServicesLoader)
    {
        _configurationManager = configurationManager;
        _dataServicesLoader = dataServicesLoader;
    }

    public async Task<IReadOnlyCollection<DocumentFieldData>> GetDocumentData(Document document, string documentVersion, InputParameters input)
    {
        var config = await _configurationManager.LoadDocumentConfiguration((int)document, documentVersion);

        var documentMapper = await LoadDocumentData(document, input, config);

        var dynamicStringFormats = documentMapper.GetDynamicStringFormats(config.DynamicStringFormats);

        return documentMapper.GetDocumentFields(config.SourceFields, dynamicStringFormats).ToList();
    }

    private async Task<DocumentMapper> LoadDocumentData(Document document, InputParameters inputParameters, DocumentConfiguration config)
    {
        var documentData = DocumentDataFactory.Create(document);

        await _dataServicesLoader.LoadData(config.InputConfig, inputParameters, documentData);

        return DocumentMapper.Create(documentData);
    }
}