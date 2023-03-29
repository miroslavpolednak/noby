namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;

internal class DataLoaderStatus
{
    public List<DataSource> LoadedDataSources { get; } = new();

    public required List<DataSource> RemainingDataSources { get; init; }

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