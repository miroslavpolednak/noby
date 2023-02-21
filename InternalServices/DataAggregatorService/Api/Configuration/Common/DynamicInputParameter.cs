namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Common;

internal class DynamicInputParameter
{
    public string InputParameterName { get; set; } = null!;

    public DataSource TargetDataSource { get; set; }

    public DataSource SourceDataSource { get; set; }

    public string SourceFieldPath { get; set; } = null!;
}