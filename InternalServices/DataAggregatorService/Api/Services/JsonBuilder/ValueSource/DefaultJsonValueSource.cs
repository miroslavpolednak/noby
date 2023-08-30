namespace CIS.InternalServices.DataAggregatorService.Api.Services.JsonBuilder.ValueSource;

internal class DefaultJsonValueSource : IJsonValueSource
{
    public static implicit operator DefaultJsonValueSource(string fieldPath) => new() { FieldPath = fieldPath };

    public string FieldPath { get; set; } = string.Empty;

    public object? ParseValue(object? value, object aggregatedData)
    {
        return value;
    }
}