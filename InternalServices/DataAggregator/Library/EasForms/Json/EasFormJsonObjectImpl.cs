using CIS.InternalServices.DataAggregator.Configuration;

namespace CIS.InternalServices.DataAggregator.EasForms.Json;

internal class EasFormJsonObjectImpl : EasFormJsonObject
{
    private readonly Dictionary<string, EasFormJsonObject> _jsonData = new();

    public override void Add(string[] propertyPath, string dataFieldPath)
    {
        if (propertyPath.Length == Depth + 1)
        {
            _jsonData.Add(propertyPath[Depth], CreateValue(dataFieldPath));

            return;
        }

        var jsonObject = propertyPath[Depth].EndsWith(ConfigurationConstants.CollectionMarker)
            ? GetOrAddJsonObject(propertyPath[Depth].Replace(ConfigurationConstants.CollectionMarker, ""), CreateObject<EasFormJsonCollection>)
            : GetOrAddJsonObject(propertyPath[Depth], CreateObject<EasFormJsonObjectImpl>);

        jsonObject.Add(propertyPath, dataFieldPath);
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

    private EasFormJsonObject GetOrAddJsonObject(string key, Func<EasFormJsonObject> factory)
    {
        if (_jsonData.ContainsKey(key))
            return _jsonData[key];

        var jsonObject = factory();

        _jsonData.Add(key, jsonObject);

        return jsonObject;
    }
}