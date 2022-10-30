namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Model;

internal record DocumentConfiguration
{
    public InputConfig InputConfig { get; init; } = null!;

    public IReadOnlyCollection<SourceField> SourceFields { get; init; } = null!;

    public ILookup<int, DynamicStringFormat> DynamicStringFormats { get; init; } = null!;
}