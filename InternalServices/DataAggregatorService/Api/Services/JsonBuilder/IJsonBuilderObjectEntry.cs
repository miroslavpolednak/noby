using CIS.InternalServices.DataAggregatorService.Api.Services.JsonBuilder.ValueSource;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.JsonBuilder;

internal interface IJsonBuilderObjectEntry : IJsonBuilderEntry
{
    void Add(string[] jsonPropertyPath, IJsonValueSource source);
}