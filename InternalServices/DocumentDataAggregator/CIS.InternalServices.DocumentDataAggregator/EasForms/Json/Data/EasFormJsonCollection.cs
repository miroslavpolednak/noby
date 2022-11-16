using System.Collections;

namespace CIS.InternalServices.DocumentDataAggregator.EasForms.Json.Data;

internal class EasFormJsonCollection : EasFormJsonObjectImpl
{
    private string _collectionPath = string.Empty;

    public override void Add(string[] propertyPath, string dataFieldPath)
    {
        if (_collectionPath == string.Empty)
            _collectionPath = CollectionPathHelper.GetCollectionPath(dataFieldPath);

        if (_collectionPath == string.Empty || _collectionPath != CollectionPathHelper.GetCollectionPath(dataFieldPath))
            throw new InvalidOperationException();

        base.Add(propertyPath, CollectionPathHelper.GetCollectionMemberPath(dataFieldPath));
    }

    public override object? GetJsonObject(object data)
    {
        if (MapperHelper.GetValue(data, _collectionPath) is not IEnumerable collection)
            throw new InvalidOperationException();

        return collection.Cast<object>().Select(data1 => base.GetJsonObject(data1)).ToList();
    }
}