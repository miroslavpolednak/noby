namespace CIS.InternalServices.DataAggregator.Configuration.Document;

internal class DocumentConfiguration
{
    public required InputConfig InputConfig { get; init; }

    public required IReadOnlyCollection<DocumentSourceField> SourceFields { get; init; }

    public required ILookup<int, DocumentDynamicStringFormat> DynamicStringFormats { get; init; }

    public required IReadOnlyCollection<DocumentTable> Tables { get; init; }
}