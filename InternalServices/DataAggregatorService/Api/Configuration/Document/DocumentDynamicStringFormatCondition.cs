namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Document;

internal class DocumentDynamicStringFormatCondition
{
    public string FieldPath { get; init; } = null!;

    public string? EqualToValue { get; init; }
}