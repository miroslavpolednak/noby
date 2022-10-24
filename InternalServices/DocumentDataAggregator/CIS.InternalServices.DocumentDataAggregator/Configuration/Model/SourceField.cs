namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Model;

internal record SourceField
{
    public DataSource DataSource { get; init; }

    public string FieldPath { get; init; } = null!;

    public string TemplateFieldName { get; init; } = null!;
}