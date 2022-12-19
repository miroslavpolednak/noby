namespace CIS.InternalServices.DataAggregator.Documents.Table;

public class DocumentTableColumn
{
    public required string Header { get; init; }

    public required float WidthPercentage { get; init; }

    public string? StringFormat { get; init; }
}