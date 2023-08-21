namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Common;

internal class InputConfig
{
    private readonly Func<IEnumerable<DataService>> _dataServicesGetter;

    public InputConfig(Func<IEnumerable<DataService>> dataServicesGetter)
    {
        _dataServicesGetter = dataServicesGetter;
    }

    public required IList<DynamicInputParameter> DynamicInputParameters { get; init; }

    public IEnumerable<DataService> GetAllDataSources() => _dataServicesGetter().Concat(DynamicInputParameters.Select(i => i.SourceDataService)).Distinct();
}