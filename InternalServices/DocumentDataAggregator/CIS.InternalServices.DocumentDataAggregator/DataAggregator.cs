using CIS.InternalServices.DocumentDataAggregator.Configuration;
using CIS.InternalServices.DocumentDataAggregator.DataServices;
using CIS.InternalServices.DocumentDataAggregator.Documents;
using CIS.InternalServices.DocumentDataAggregator.Mapper;

namespace CIS.InternalServices.DocumentDataAggregator;

internal class DataAggregator : IDataAggregator
{
    private readonly DocumentConfigurationManager _configurationManager;
    private readonly DataServicesLoader _dataServicesLoader;

    public DataAggregator(DocumentConfigurationManager configurationManager, DataServicesLoader dataServicesLoader)
    {
        _configurationManager = configurationManager;
        _dataServicesLoader = dataServicesLoader;
    }

    public async Task<IReadOnlyCollection<DocumentFieldData>> GetDocumentData(InputParameters input)
    {
        var config = await _configurationManager.LoadDocumentConfiguration();

        var documentData = DocumentDataFactory.Create(Document.Offer);

        await _dataServicesLoader.LoadData(config.InputConfig, input, documentData);


        DocumentMapperStatic.Test(config, documentData);

        var testOutput = DocumentMapperStatic.Map(config.SourceFields, documentData);

        return testOutput;
    }
}