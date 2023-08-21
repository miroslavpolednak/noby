namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.EasForm;

internal class EasFormConfiguration : ConfigurationBase<EasFormSourceField>
{
    public required EasFormKey EasFormKey { get; init; }

    public bool IsCancelled { get; set; }
}