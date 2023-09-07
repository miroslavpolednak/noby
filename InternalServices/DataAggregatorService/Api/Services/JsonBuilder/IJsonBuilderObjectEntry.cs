using CIS.InternalServices.DataAggregatorService.Api.Services.JsonBuilder.ValueSource;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.JsonBuilder;

internal interface IJsonBuilderObjectEntry<in TValueSource> : IJsonBuilderEntry where TValueSource : IJsonValueSource
{
    void Add(string[] jsonPropertyPath, TValueSource source);
}