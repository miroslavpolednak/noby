namespace CIS.InternalServices.DataAggregator.Configuration.EasForm;

internal class EasFormConfiguration
{
    public required InputConfig InputConfig { get; init; }

    public required IReadOnlyCollection<EasFormSourceField> SourceFields { get; init; }
}