namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.EasForm;

internal class EasFormSourceField : SourceFieldBase
{
    public EasFormType EasFormType { get; init; }

    public string JsonPropertyName { get; init; } = null!;
}