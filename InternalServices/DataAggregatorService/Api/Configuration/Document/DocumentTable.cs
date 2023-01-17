namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Document;

internal class DocumentTable
{
    public required string AcroFieldPlaceholder { get; init; }

    public required DataSource DataSource { get; set; }

    public required string CollectionSourcePath { get; init; }

    public required ICollection<Column> Columns { get; init; }

    public string? ConcludingParagraph { get; init; }

    public class Column
    {
        public required string CollectionFieldPath { get; init; }

        public required string Header { get; init; }

        public required float WidthPercentage { get; init; }

        public string? StringFormat { get; init; }
    }
}