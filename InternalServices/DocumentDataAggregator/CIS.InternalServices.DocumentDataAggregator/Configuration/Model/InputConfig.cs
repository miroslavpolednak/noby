namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Model;

internal record InputConfig
{
    public IEnumerable<DataSource> DataSources { get; init; } = null!;

    public IEnumerable<DynamicInputParameter> DynamicInputParameters { get; init; } = null!;
}