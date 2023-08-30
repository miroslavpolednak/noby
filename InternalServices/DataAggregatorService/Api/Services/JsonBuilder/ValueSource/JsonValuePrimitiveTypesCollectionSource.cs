using System.Collections;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.JsonBuilder.ValueSource;

internal class JsonValuePrimitiveTypesCollectionSource : IJsonValueSource
{
    private readonly IJsonValueSource _source;
    private readonly int _depth;

    public JsonValuePrimitiveTypesCollectionSource(IJsonValueSource source, int depth)
    {
        _source = source;
        _depth = depth;

        FieldPath = _source.FieldPath;
    }

    public string FieldPath { get; set; }

    public object ParseValue(object? value, object sourceData)
    {
        if (value is null)
            return Enumerable.Empty<object>();

        if (value is not IEnumerable collection)
            throw new InvalidOperationException();
        
        var memberPath = CollectionPathHelper.GetCollectionMemberPath(FieldPath, _depth);

        _source.FieldPath = memberPath;

        return collection.Cast<object>().Select(obj =>
        {
            var objValue = MapperHelper.GetValue(obj, memberPath);

            return _source.ParseValue(objValue, obj);
        });
    }
}