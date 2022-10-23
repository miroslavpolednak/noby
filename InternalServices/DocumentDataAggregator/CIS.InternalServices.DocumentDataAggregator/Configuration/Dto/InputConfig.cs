using CIS.InternalServices.DocumentDataAggregator.Configuration.Data;

namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Dto;

internal record InputConfig
{
    public IEnumerable<DataSource> DataSources { get; init; } = null!;

    public IEnumerable<DynamicInputParameter> DynamicInputParameters { get; init; } = null!;
}