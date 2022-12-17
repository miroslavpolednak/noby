namespace CIS.InternalServices.DataAggregator.Configuration.Document;

internal class DocumentDynamicStringFormatCondition
{
    public string SourceFieldPath { get; init; } = null!;

    public string? EqualToValue { get; init; }
}