using CIS.InternalServices.DocumentDataAggregator.Configuration;
using CIS.InternalServices.DocumentDataAggregator.DataServices;
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

    public async Task<ICollection<KeyValuePair<string, object>>> GetDocumentData(int offerId)
    {
        var config = await _configurationManager.LoadDocumentConfiguration();

        var data = await _dataServicesLoader.LoadData(config.InputConfig, new InputParameters
        {
            OfferId = 111
        });

        var testOutput = DocumentMapper.Map(config.SourceFields, data);

        return Enumerable.Empty<KeyValuePair<string, object>>().ToList();
    }
}