namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Model;

internal class DocumentConfiguration
{
    public const string FieldPathCollectionMarker = "[]";

    public required InputConfig InputConfig { get; init; }

    public required IReadOnlyCollection<SourceField> SourceFields { get; init; }

    public required ILookup<int, DocumentDynamicStringFormat> DynamicStringFormats { get; init; }
}