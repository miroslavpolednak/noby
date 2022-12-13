namespace CIS.InternalServices.DocumentDataAggregator.Configuration.EasForm;

internal class EasFormSourceField
{
    public int? SourceFieldId { get; init; }

    public DataSource DataSource { get; init; }

    public EasFormType FormType { get; set; }

    public string FieldPath { get; init; } = null!;

    public string JsonPropertyName { get; init; } = null!;
}