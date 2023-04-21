namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;

internal class DataLoaderStatus
{
    private readonly object _remainingDataSourcesSyncRoot = new();

    public DataLoaderStatus(IEnumerable<DataSource> requestedDataSources)
    {
        RemainingDataSources = new SynchronizedCollection<DataSource>(_remainingDataSourcesSyncRoot, requestedDataSources);
    }

    public SynchronizedCollection<DataSource> LoadedDataSources { get; } = new();

    public SynchronizedCollection<DataSource> RemainingDataSources { get; }

    public required List<DynamicInputParameter> RelatedInputParameters { get; init; }

    public required InputParameters InputParameters { get; init; }

    public required AggregatedData AggregatedData { get; init; }

    public void MarkAsLoaded(DataSource dataSource)
    {
        RemainingDataSources.Remove(dataSource);
        LoadedDataSources.Add(dataSource);
    }

    public ICollection<DataSource> GetRemainingDataSources() => RemainingDataSources.Except(RelatedInputParameters.Select(p => p.TargetDataSource)).ToList();
}