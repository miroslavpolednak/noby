namespace CIS.InternalServices.DataAggregatorService.Api.Generators.Documents;

internal class DocumentSourceFieldData
{
    public required string AcroFieldName { get; init; }

    public string? StringFormat { get; init; }

    public required object? Value { get; init; }

    public byte? TextAlign { get; init; }

    public byte? VAlign { get; init; }

    public bool DefaultValueWasUsed { get; init; }
}