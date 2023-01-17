namespace CIS.InternalServices.DataAggregatorService.Api.Configuration;

internal class InputConfig
{
    public required IEnumerable<DataSource> DataSources { get; init; }

    public required IEnumerable<DynamicInputParameter> DynamicInputParameters { get; init; }
}