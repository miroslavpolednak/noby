namespace CIS.InternalServices.DataAggregatorService.Api.Services.JsonBuilder.ValueSource;

internal class DefaultJsonValueSource : IJsonValueSource
{
    public string FieldPath { get; set; } = string.Empty;

    public object? ParseValue(object? value, object aggregatedData)
    {
        return value;
    }
}