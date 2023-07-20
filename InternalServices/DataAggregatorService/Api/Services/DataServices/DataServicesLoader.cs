using CIS.InternalServices.DataAggregatorService.Api.Helpers;
using DomainServices.CodebookService.Clients;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;

[TransientService, SelfService]
internal class DataServicesLoader
{
    private readonly ServiceMap _serviceMap;
    private readonly IServiceProvider _serviceProvider;

    public DataServicesLoader(ServiceMap serviceMap, IServiceProvider serviceProvider)
    {
        _serviceMap = serviceMap;
        _serviceProvider = serviceProvider;
    }

    public async Task LoadData(InputConfig inputConfig, InputParameters parameters, AggregatedData aggregatedData, CancellationToken cancellationToken = default)
    {
        var status = new DataLoaderStatus(inputConfig.GetAllDataSources())
        {
            RelatedInputParameters = inputConfig.DynamicInputParameters.ToList(),
            AggregatedData = aggregatedData,
            InputParameters = parameters
        };

        using var serviceScope = _serviceProvider.CreateScope();

        while (status.RemainingDataSources.Any())
        {
            await ProcessRemainingDataSources(status, serviceScope.ServiceProvider, cancellationToken);
            SetInputParameters(status);
        }

        await aggregatedData.LoadCodebooks(_serviceProvider.GetRequiredService<ICodebookServiceClient>(), cancellationToken);
        await aggregatedData.LoadAdditionalData(cancellationToken);
    }

    private Task ProcessRemainingDataSources(DataLoaderStatus status, IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        var dataSources = status.GetRemainingDataSources();

        if (!dataSources.Any())
            throw new InvalidOperationException("An input parameter for the remaining data services is missing.");

        return Task.WhenAll(dataSources.Select(Load));

        async Task Load(DataSource dataSource)
        {
            await _serviceMap.GetServiceCallFunc(dataSource, serviceProvider).Invoke(status.InputParameters, status.AggregatedData, cancellationToken);

            status.MarkAsLoaded(dataSource);
        }
    }

    private static void SetInputParameters(DataLoaderStatus status)
    {
        var loadedParameters = status.RelatedInputParameters.Where(p => status.LoadedDataSources.Contains(p.SourceDataSource)).ToList();

        loadedParameters.ForEach(dynamicParameter =>
        {
            MapperHelper.MapInputParameters(status.InputParameters, dynamicParameter, status.AggregatedData);
            status.RelatedInputParameters.Remove(dynamicParameter);
        });
    }
}