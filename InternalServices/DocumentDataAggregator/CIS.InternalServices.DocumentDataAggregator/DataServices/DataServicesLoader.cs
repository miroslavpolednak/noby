using CIS.InternalServices.DocumentDataAggregator.Configuration.Model;
using CIS.InternalServices.DocumentDataAggregator.DataServices.Dto;
using CIS.InternalServices.DocumentDataAggregator.DataServices.ServiceWrappers;
using CIS.InternalServices.DocumentDataAggregator.Documents;
using CIS.InternalServices.DocumentDataAggregator.Mapper;
using Microsoft.Extensions.DependencyInjection;

namespace CIS.InternalServices.DocumentDataAggregator.DataServices;

[SingletonService, SelfService]
internal class DataServicesLoader
{
    private delegate Task ServiceCall(InputParameters input, AggregatedData data, CancellationToken cancellationToken);

    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<DataSource, ServiceCall> _serviceMap = new();

    public DataServicesLoader(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

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
            parameters.Map(dynamicParameter, aggregatedData);
            status.RelatedInputParameters.Remove(dynamicParameter);
        });
    }

    private void ConfigureServiceMap()
    {
        AddService<OfferServiceWrapper>(DataSource.OfferService);
        AddService<UserServiceWrapper>(DataSource.UserService);
    }

    private void AddService<TSource>(DataSource dataSource) where TSource : IServiceWrapper => 
        _serviceMap.Add(dataSource, _serviceProvider.GetRequiredService<TSource>().LoadData);
}