namespace CIS.InternalServices.DataAggregatorService.Api.Generators.RiskLoanApplication.Json;

internal class RiskLoanApplicationJsonObjectImpl : RiskLoanApplicationJsonObject
{
    private readonly Dictionary<string, RiskLoanApplicationJsonObject> _jsonData = new();

    public override void Add(string[] propertyPath, string dataFieldPath, bool useDefaultInsteadOfNull)
    {
        if (propertyPath.Length == Depth + 1)
        {
            if (propertyPath.Last().EndsWith(ConfigurationConstants.CollectionMarker, StringComparison.InvariantCultureIgnoreCase))
            {
                var primitiveTypesCollection = (RiskLoanApplicationJsonPrimitiveTypesCollection)CreateObject<RiskLoanApplicationJsonPrimitiveTypesCollection>();
                primitiveTypesCollection.DataFieldPath = dataFieldPath;

                _jsonData.Add(propertyPath[Depth].Replace(ConfigurationConstants.CollectionMarker, ""), primitiveTypesCollection);

                return;
            }

            _jsonData.Add(propertyPath[Depth], CreateValue(dataFieldPath, useDefaultInsteadOfNull));

            return;
        }

        var jsonObject = propertyPath[Depth].EndsWith(ConfigurationConstants.CollectionMarker, StringComparison.InvariantCultureIgnoreCase)
            ? GetOrAddJsonObject(propertyPath[Depth].Replace(ConfigurationConstants.CollectionMarker, ""), CreateObject<RiskLoanApplicationJsonCollection>)
            : GetOrAddJsonObject(propertyPath[Depth], CreateObject<RiskLoanApplicationJsonObjectImpl>);

        jsonObject.Add(propertyPath, dataFieldPath, useDefaultInsteadOfNull);
    }

    public override object? GetJsonObject(object data)
    {
        var dictionary = _jsonData.Select(o => new
                                  {
                                      o.Key,
                                      Value = o.Value.GetJsonObject(data)
                                  })
                                  .Where(o => o.Value is not null)
                                  .ToDictionary(k => k.Key, v => v.Value!);

        return dictionary.Any() ? dictionary : null;
    }

    private RiskLoanApplicationJsonObject GetOrAddJsonObject(string key, Func<RiskLoanApplicationJsonObject> factory)
    {
        if (_jsonData.ContainsKey(key))
            return _jsonData[key];

        var jsonObject = factory();

        _jsonData.Add(key, jsonObject);

        return jsonObject;
    }
}