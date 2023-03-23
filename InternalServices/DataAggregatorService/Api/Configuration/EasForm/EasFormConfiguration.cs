namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.EasForm;

internal record EasFormConfiguration
{
    public required EasFormKey EasFormKey { get; init; }

    public required InputConfig InputConfig { get; init; }

    public required IReadOnlyCollection<EasFormSourceField> SourceFields { get; init; }
}