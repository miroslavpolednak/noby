namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents;

internal class DocumentSourceFieldData
{
    public int? SourceFieldId { get; init; }

    public required string AcroFieldName { get; init; }

    public string? StringFormat { get; init; }

    public required object? Value { get; init; }
}