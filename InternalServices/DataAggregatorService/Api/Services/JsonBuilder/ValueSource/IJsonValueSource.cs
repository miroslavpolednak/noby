namespace CIS.InternalServices.DataAggregatorService.Api.Services.JsonBuilder.ValueSource;

internal interface IJsonValueSource
{
    public string FieldPath { get; set; }

    object? ParseValue(object? value, object aggregatedData);
}