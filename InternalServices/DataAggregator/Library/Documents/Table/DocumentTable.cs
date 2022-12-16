namespace CIS.InternalServices.DataAggregator.Documents.Table;

public class DocumentTable
{
    public required ICollection<DocumentTableColumn> Columns { get; init; }

    public required ICollection<ICollection<object?>> RowsValues { get; init; }

    public string? ConcludingParagraph { get; init; }
}