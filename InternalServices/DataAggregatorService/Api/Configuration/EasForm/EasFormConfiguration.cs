namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.EasForm;

internal class EasFormConfiguration
{
    public required EasFormRequestType EasFormRequestType { get; init; }

    public required InputConfig InputConfig { get; init; }

    public required IReadOnlyCollection<EasFormSourceField> SourceFields { get; init; }
}