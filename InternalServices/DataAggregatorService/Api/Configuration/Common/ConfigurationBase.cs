namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Common;

internal class ConfigurationBase<TSourceField> where TSourceField : SourceFieldBase
{
    public InputConfig InputConfig { get; private init; } = null!;

    public required IReadOnlyCollection<TSourceField> SourceFields { get; init; }

    public required IList<DynamicInputParameter> DynamicInputParameters
    {
        get => InputConfig.DynamicInputParameters;
        init => InputConfig = new InputConfig(GetDataServices) { DynamicInputParameters = value };
    }

    protected virtual IEnumerable<DataService> GetDataServices()
    {
        return SourceFields.Select(s => s.DataService).Where(dataService => dataService != DataService.General);
    }
}