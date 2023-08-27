namespace CIS.InternalServices.DataAggregatorService.Api.Services.JsonBuilder;

internal interface IJsonBuilderEntry
{
    int Depth { get; init; }

    object? GetJsonObject(object data);
}