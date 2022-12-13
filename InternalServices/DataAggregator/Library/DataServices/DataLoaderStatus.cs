namespace CIS.InternalServices.DocumentDataAggregator.DataServices;

internal class DataLoaderStatus
{
    public List<DataSource> RemainingDataSources { get; init; } = null!;

    public List<DataSource> LoadedDataSources { get; } = new();

    public List<DynamicInputParameter> RelatedInputParameters { get; init; } = null!;
}