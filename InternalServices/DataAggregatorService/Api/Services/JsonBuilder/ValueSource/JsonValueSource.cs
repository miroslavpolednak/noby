namespace CIS.InternalServices.DataAggregatorService.Api.Services.JsonBuilder.ValueSource;

internal abstract class JsonValueSource<TSourceField> : IJsonValueSource where TSourceField : SourceFieldBase
{
    protected TSourceField SourceField { get; }

    protected JsonValueSource(TSourceField sourceField)
    {
        SourceField = sourceField;

        FieldPath = SourceField.FieldPath;
    }

    public string FieldPath { get; set; }

    public abstract object? ParseValue(object? value, object aggregatedData);
}