namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Document;

internal class DocumentDynamicStringFormatCondition
{
    public string SourceFieldPath { get; init; } = null!;

    public string? EqualToValue { get; init; }
}