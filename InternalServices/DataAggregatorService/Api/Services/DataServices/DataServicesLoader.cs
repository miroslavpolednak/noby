using DomainServices.CodebookService.Clients;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;

[SingletonService, SelfService]
internal class DataServicesLoader
{
    private readonly ServiceMap _serviceMap;
    private readonly IServiceProvider _serviceProvider;

    public DataServicesLoader(ServiceMap serviceMap, IServiceProvider serviceProvider)
    {
        _serviceMap = serviceMap;
        _serviceProvider = serviceProvider;
    }

    public async Task LoadData(InputConfig inputConfig, InputParameters parameters, AggregatedData aggregatedData, CancellationToken cancellationToken)
    {
        var status = new DataLoaderStatus(inputConfig.GetAllDataSources())
        {
            RelatedInputParameters = inputConfig.DynamicInputParameters.ToList(),
            AggregatedData = aggregatedData,
            InputParameters = parameters
        };

        await aggregatedData.LoadCodebooks(_serviceProvider.GetRequiredService<ICodebookServiceClient>(), cancellationToken);

        while (status.RemainingDataSources.Any())
        {
            await ProcessRemainingDataSources(status, cancellationToken);
            SetInputParameters(status);
        }

        await aggregatedData.LoadAdditionalData(cancellationToken);
    }

    private Task ProcessRemainingDataSources(DataLoaderStatus status, CancellationToken cancellationToken)
    {
        var dataSources = status.GetRemainingDataSources();

        if (!dataSources.Any())
            throw new InvalidOperationException("An input parameter for the remaining data services is missing.");

        return Task.WhenAll(dataSources.Select(Load));

        async Task Load(DataService dataSource)
        {
            await _serviceMap.GetServiceCallFunc(dataSource, _serviceProvider).Invoke(status.InputParameters, status.AggregatedData, cancellationToken);

            status.MarkAsLoaded(dataSource);
        }
    }

    private static void SetInputParameters(DataLoaderStatus status)
    {
        var loadedParameters = status.RelatedInputParameters.Where(p => status.LoadedDataSources.Contains(p.SourceDataService)).ToList();

        loadedParameters.ForEach(dynamicParameter =>
        {
            MapperHelper.MapInputParameters(status.InputParameters, dynamicParameter, status.AggregatedData);
            status.RelatedInputParameters.Remove(dynamicParameter);
        });
    }
}