namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents;

internal class DocumentSourceFieldData
{
    public required string AcroFieldName { get; init; }

    public string? StringFormat { get; init; }

    public required object? Value { get; init; }

    public byte? TextAlign { get; init; }
}