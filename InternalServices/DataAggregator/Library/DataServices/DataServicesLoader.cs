using CIS.InternalServices.DataAggregator.Configuration;
using CIS.InternalServices.DataAggregator.DataServices.ServiceWrappers;
using CIS.InternalServices.DataAggregator.Helpers;
using DomainServices.CodebookService.Clients;
using Microsoft.Extensions.DependencyInjection;

namespace CIS.InternalServices.DataAggregator.DataServices;

[TransientService, SelfService]
internal class DataServicesLoader
{
    private delegate Task ServiceCall(InputParameters input, AggregatedData data, CancellationToken cancellationToken);

    private readonly IServiceProvider _serviceProvider;
    private readonly ICodebookServiceClients _codebookService;
    private readonly Dictionary<DataSource, ServiceCall> _serviceMap = new();

    public DataServicesLoader(IServiceProvider serviceProvider, ICodebookServiceClients codebookService)
    {
        _serviceProvider = serviceProvider;
        _codebookService = codebookService;

        ConfigureServiceMap();
    }

    public async Task LoadData(InputConfig inputConfig, InputParameters parameters, AggregatedData aggregatedData)
    {
        var status = new DataLoaderStatus
        {
            RemainingDataSources = inputConfig.DataSources.ToList(),
            RelatedInputParameters = inputConfig.DynamicInputParameters.ToList()
        };

        while (status.RemainingDataSources.Any())
        {
            await ProcessRemainingDataSources(status, parameters, aggregatedData);
            SetInputParameters(status, parameters, aggregatedData);
        }

        await aggregatedData.LoadCodebooks(_codebookService);
    }

    private async Task ProcessRemainingDataSources(DataLoaderStatus status, InputParameters parameters, AggregatedData aggregatedData)
    {
        var dataSources = status.RemainingDataSources.Except(status.RelatedInputParameters.Select(p => p.TargetDataSource)).ToList();

        if (!dataSources.Any())
            throw new InvalidOperationException("Error");

        foreach (var dataSource in dataSources)
        {
            await _serviceMap[dataSource].Invoke(parameters, aggregatedData, CancellationToken.None);

            status.RemainingDataSources.Remove(dataSource);
            status.LoadedDataSources.Add(dataSource);
        }
    }

    private void SetInputParameters(DataLoaderStatus status, InputParameters parameters, AggregatedData aggregatedData)
    {
        var loadedParameters = status.RelatedInputParameters.Where(p => status.LoadedDataSources.Contains(p.SourceDataSource)).ToList();

        loadedParameters.ForEach(dynamicParameter =>
        {
            MapperHelper.MapInputParameters(parameters, dynamicParameter, aggregatedData);
            status.RelatedInputParameters.Remove(dynamicParameter);
        });
    }

    private void ConfigureServiceMap()
    {
        AddService<SalesArrangementServiceWrapper>(DataSource.SalesArrangementService);
        AddService<CaseServiceWrapper>(DataSource.CaseService);
        AddService<OfferServiceWrapper>(DataSource.OfferService);
        AddService<UserServiceWrapper>(DataSource.UserService);
        AddService<CustomerServiceWrapper>(DataSource.CustomerService);
        AddService<ProductServiceWrapper>(DataSource.ProductService);
        AddService<OfferPaymentScheduleServiceWrapper>(DataSource.OfferPaymentScheduleService);

        void AddService<TSource>(DataSource dataSource) where TSource : IServiceWrapper =>
            _serviceMap.Add(dataSource, _serviceProvider.GetRequiredService<TSource>().LoadData);
    }
}