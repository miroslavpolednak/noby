using System.Collections;
using CIS.InternalServices.DataAggregatorService.Api.Helpers;

namespace CIS.InternalServices.DataAggregatorService.Api.Generators.RiskLoanApplication.Json;

internal class RiskLoanApplicationJsonPrimitiveTypesCollection : RiskLoanApplicationJsonObjectImpl
{
    public string DataFieldPath { get; set; } = string.Empty;

    public override void Add(string[] propertyPath, string dataFieldPath, bool useDefaultInsteadOfNull)
    {
        throw new NotImplementedException();
    }

    public override object? GetJsonObject(object data)
    {
        var obj = MapperHelper.GetValue(data, CollectionPathHelper.GetCollectionPath(DataFieldPath));

        if (obj is null)
            return Enumerable.Empty<object>();

        if (obj is not IEnumerable collection)
            throw new InvalidOperationException();

        var memberPath = CollectionPathHelper.GetCollectionMemberPath(DataFieldPath, Depth);

        return collection.Cast<object>().Select(o => MapperHelper.GetValue(o, memberPath)).ToList();
    }
}