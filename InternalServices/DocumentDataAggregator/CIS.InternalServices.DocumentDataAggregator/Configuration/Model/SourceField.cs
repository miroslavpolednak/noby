namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Model;

internal record SourceField
{
    public int? SourceFieldId { get; init; }

    public DataSource DataSource { get; init; }

    public string FieldPath { get; init; } = null!;

    public string TemplateFieldName { get; init; } = null!;

    public string? StringFormat { get; init; }
}