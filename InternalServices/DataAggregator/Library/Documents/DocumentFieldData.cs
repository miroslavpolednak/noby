namespace CIS.InternalServices.DataAggregator.Documents;

public class DocumentFieldData
{
    public string FieldName { get; init; } = null!;

    public object Value { get; init; } = null!;

    public string? StringFormat { get; init; }
}