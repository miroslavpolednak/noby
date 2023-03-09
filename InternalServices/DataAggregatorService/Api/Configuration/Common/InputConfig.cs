namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Common;

internal class InputConfig
{
    public required IEnumerable<DataSource> DataSources { get; init; }

    public required IEnumerable<DynamicInputParameter> DynamicInputParameters { get; init; }
}