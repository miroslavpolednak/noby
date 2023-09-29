namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Document;

internal class DocumentTable
{
    public int DocumentTableId { get; init; }
    
    public string AcroFieldPlaceholder { get; init; } = null!;

    public DataService DataService { get; set; }

    public string TableSourcePath { get; init; } = null!;

    public List<Column> Columns { get; } = new List<Column>();

    public string? ConcludingParagraph { get; init; }

    public class Column
    {
        public required string CollectionFieldPath { get; init; }

        public required string Header { get; init; }

        public required float WidthPercentage { get; init; }

        public string? StringFormat { get; init; }
    }
}