namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Common;

internal class DynamicInputParameter
{
    public string InputParameter { get; init; } = null!;

    public DataService TargetDataService { get; init; }

    public DataService SourceDataService { get; init; }

    public string SourceFieldPath { get; init; } = null!;
}