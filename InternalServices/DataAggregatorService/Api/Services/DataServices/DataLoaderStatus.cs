namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;

internal class DataLoaderStatus
{
    private readonly object _remainingDataSourcesSyncRoot = new();

    public DataLoaderStatus(IEnumerable<DataService> requestedDataSources)
    {
        RemainingDataSources = new SynchronizedCollection<DataService>(_remainingDataSourcesSyncRoot, requestedDataSources);
    }

    public SynchronizedCollection<DataService> LoadedDataSources { get; } = new();

    public SynchronizedCollection<DataService> RemainingDataSources { get; }

    public required List<DynamicInputParameter> RelatedInputParameters { get; init; }

    public required InputParameters InputParameters { get; init; }

    public required AggregatedData AggregatedData { get; init; }

    public void MarkAsLoaded(DataService dataService)
    {
        RemainingDataSources.Remove(dataService);
        LoadedDataSources.Add(dataService);
    }

    public ICollection<DataService> GetRemainingDataSources() => RemainingDataSources.Except(RelatedInputParameters.Select(p => p.TargetDataService)).ToList();
}