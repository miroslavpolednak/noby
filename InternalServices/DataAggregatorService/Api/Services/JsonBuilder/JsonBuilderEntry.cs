namespace CIS.InternalServices.DataAggregatorService.Api.Services.JsonBuilder;

internal abstract class JsonBuilderEntry : IJsonBuilderEntry
{
    public int Depth { get; init; }

    public abstract object? GetJsonObject(object data);
}