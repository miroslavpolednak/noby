namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Common;

internal class InputConfig
{
    public required IEnumerable<DataSource> DataSources { get; init; }

    public required IEnumerable<DynamicInputParameter> DynamicInputParameters { get; init; }

    public IEnumerable<DataSource> GetAllDataSources() => DataSources.Concat(DynamicInputParameters.Select(i => i.SourceDataSource)).Distinct().ToList();
}