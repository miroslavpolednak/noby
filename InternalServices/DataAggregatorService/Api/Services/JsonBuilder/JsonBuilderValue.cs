using CIS.InternalServices.DataAggregatorService.Api.Services.JsonBuilder.ValueSource;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.JsonBuilder;

internal class JsonBuilderValue : IJsonBuilderEntry
{
    private readonly IJsonValueSource _valueSource;

    public JsonBuilderValue(IJsonValueSource valueSource, bool isCollection = false)
    {
        _valueSource = valueSource;
        IsCollection = isCollection;
    }

    public bool IsCollection { get; }

    int IJsonBuilderEntry.Depth { get; init; }

    public object? GetJsonObject(object data)
    {
        var fieldPath = IsCollection ? CollectionPathHelper.GetCollectionPath(_valueSource.FieldPath) : _valueSource.FieldPath;

        var value = MapperHelper.GetValue(data, fieldPath);

        return _valueSource.ParseValue(value, data);
    }
}