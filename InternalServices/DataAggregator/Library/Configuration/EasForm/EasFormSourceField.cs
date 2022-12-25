namespace CIS.InternalServices.DataAggregator.Configuration.EasForm;

internal class EasFormSourceField
{
    public int? SourceFieldId { get; init; }

    public DataSource DataSource { get; init; }

    public EasFormType FormType { get; private set; }

    public string FormTypeString
    {
        set => FormType = Enum.Parse<EasFormType>(value);
    }

    public string FieldPath { get; init; } = null!;

    public string JsonPropertyName { get; init; } = null!;
}