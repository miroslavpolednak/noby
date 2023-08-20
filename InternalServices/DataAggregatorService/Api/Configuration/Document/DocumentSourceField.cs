namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Document;

internal class DocumentSourceField : SourceFieldBase
{
    public string AcroFieldName { get; init; } = null!;

    public string? StringFormat { get; init; }

    public byte? TextAlign { get; init; }

    public string? DefaultTextIfNull { get; init; }
}