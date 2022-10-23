namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Data;

internal class DynamicInputParameter
{
    public string InputParameterName { get; set; } = null!;

    public DataSource TargetDataSource { get; set; }

    public DataSourceField SourceField { get; set; } = null!;
}