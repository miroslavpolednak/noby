using CIS.InternalServices.DocumentDataAggregator.Configuration;
using CIS.InternalServices.DocumentDataAggregator.Configuration.Data;
using CIS.InternalServices.DocumentDataAggregator.Configuration.Dto;
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
        var test = new InputConfig
        {
            DataSources = new[]
            {
                DataSource.OfferService, DataSource.UserService
            },
            DynamicInputParameters = new[]
            {
                new DynamicInputParameter
                {
                    InputParameterName = "UserId",
                    TargetDataSource = DataSource.UserService,
                    SourceField = new DataSourceField
                    {
                        DataSource = DataSource.OfferService,
                        Path = "Offer.Created.UserId"
                    }
                }
            }
        };

        var config = await _configurationManager.Load(CancellationToken.None);

        var data = await _dataServicesLoader.LoadData(test, new InputParameters
        {
            OfferId = 111
        });

        var testOutput = DocumentMapper.Map(config[DataSource.OfferService], data);

        return Enumerable.Empty<KeyValuePair<string, object>>().ToList();
    }
}