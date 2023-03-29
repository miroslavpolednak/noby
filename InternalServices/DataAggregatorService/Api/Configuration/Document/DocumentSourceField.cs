namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Document;

internal class DocumentSourceField
{
    public int? SourceFieldId { get; init; }

    public DataSource DataSource { get; init; }

    public string FieldPath { get; init; } = null!;

    public string AcroFieldName { get; init; } = null!;

    public string? StringFormat { get; init; }

    public string? DefaultTextIfNull { get; init; }

    public byte? TextAlign { get; init; }
}