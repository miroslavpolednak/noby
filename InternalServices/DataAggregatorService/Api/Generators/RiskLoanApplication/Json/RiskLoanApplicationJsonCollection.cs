using System.Collections;
using CIS.InternalServices.DataAggregatorService.Api.Helpers;

namespace CIS.InternalServices.DataAggregatorService.Api.Generators.RiskLoanApplication.Json;

internal class RiskLoanApplicationJsonCollection : RiskLoanApplicationJsonObjectImpl
{
    private string _collectionPath = string.Empty;

    public override void Add(string[] propertyPath, string dataFieldPath)
    {
        if (_collectionPath == string.Empty)
            _collectionPath = CollectionPathHelper.GetCollectionPath(dataFieldPath);

        if (_collectionPath == string.Empty || _collectionPath != CollectionPathHelper.GetCollectionPath(dataFieldPath))
            throw new InvalidOperationException();

        var collectionMaxDepth = propertyPath.Take(Math.Max(Depth - 1, 0)).Count(path => path.EndsWith(ConfigurationConstants.CollectionMarker));

        base.Add(propertyPath, CollectionPathHelper.GetCollectionMemberPath(dataFieldPath, collectionMaxDepth));
    }

    public override object? GetJsonObject(object data)
    {
        var obj = MapperHelper.GetValue(data, _collectionPath);

        if (obj is null)
            return Enumerable.Empty<object>();

        if (obj is not IEnumerable collection)
            throw new InvalidOperationException();

        return collection.Cast<object>().Select(base.GetJsonObject).ToList();
    }
}