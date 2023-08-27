using System.Collections;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.JsonBuilder.ValueSource;

internal class JsonValuePrimitiveTypesCollectionSource : IJsonValueSource
{
    private readonly int _depth;

    public JsonValuePrimitiveTypesCollectionSource(string fieldPath, int depth)
    {
        _depth = depth;
        FieldPath = fieldPath;
    }

    public string FieldPath { get; set; }

    public object ParseValue(object? value, object aggregatedData)
    {
        if (value is null)
            return Enumerable.Empty<object>();

        if (value is not IEnumerable collection)
            throw new InvalidOperationException();

        var memberPath = CollectionPathHelper.GetCollectionMemberPath(FieldPath, _depth);

        return collection.Cast<object>().Select(o => MapperHelper.GetValue(o, memberPath)).ToList();
    }
}