namespace CIS.InternalServices.DocumentDataAggregator.Documents;

public class DocumentFieldData
{
    public string FieldName { get; init; } = null!;

    public object? Value { get; init; }

    public string? StringFormat { get; init; }
}