namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Model;

internal class DocumentConfiguration
{
    public InputConfig InputConfig { get; init; } = null!;

    public IReadOnlyCollection<SourceField> SourceFields { get; init; } = null!;

    public ILookup<int, DocumentDynamicStringFormat> DynamicStringFormats { get; init; } = null!;
}