namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Model;

internal class DocumentDynamicStringFormatCondition
{
    public string SourceFieldPath { get; init; } = null!;

    public string? EqualToValue { get; init; }
}