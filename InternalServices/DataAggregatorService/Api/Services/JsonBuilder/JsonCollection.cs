using System.Collections;
using CIS.InternalServices.DataAggregatorService.Api.Services.JsonBuilder.ValueSource;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.JsonBuilder;

internal class JsonCollection : JsonObject
{
    private string _collectionPath = string.Empty;

    public override void Add(string[] jsonPropertyPath, IJsonValueSource source)
    {
        if (_collectionPath == string.Empty)
            _collectionPath = CollectionPathHelper.GetCollectionPath(source.FieldPath);

        if (_collectionPath == string.Empty || _collectionPath != CollectionPathHelper.GetCollectionPath(source.FieldPath))
            throw new InvalidOperationException();

        var collectionMaxDepth = jsonPropertyPath.Take(Math.Max(Depth - 1, 0)).Count(path => path.EndsWith(ConfigurationConstants.CollectionMarker, StringComparison.InvariantCultureIgnoreCase));

        source.FieldPath = CollectionPathHelper.GetCollectionMemberPath(source.FieldPath, collectionMaxDepth);

        base.Add(jsonPropertyPath, source);
    }

    public override object GetJsonObject(object data)
    {
        var obj = MapperHelper.GetValue(data, _collectionPath);

        if (obj is null)
            return Enumerable.Empty<object>();

        if (obj is not IEnumerable collection)
            throw new InvalidOperationException();

        return collection.Cast<object>().Select(base.GetJsonObject).ToList();
    }
}