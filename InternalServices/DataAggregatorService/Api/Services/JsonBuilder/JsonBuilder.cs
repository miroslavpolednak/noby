using System.Collections.Immutable;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using CIS.InternalServices.DataAggregatorService.Api.Services.JsonBuilder.ValueSource;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.JsonBuilder;

internal class JsonBuilder<TValueSource> : JsonBuilderEntry, IJsonBuilderObjectEntry<TValueSource> where TValueSource : IJsonValueSource
{
    private readonly Dictionary<string, IJsonBuilderEntry> _valueProperties = new();
    private readonly Dictionary<string, IJsonBuilderObjectEntry<TValueSource>> _complexProperties = new();

    public void Add(string jsonPropertyPath, TValueSource source) => Add(jsonPropertyPath.Split('.'), source);

    public virtual void Add(string[] jsonPropertyPath, TValueSource source)
    {
        var actualPropertyPath = jsonPropertyPath[Depth];

        if (jsonPropertyPath.Length > Depth + 1)
        {
            var jsonObject = CollectionPathHelper.IsCollection(actualPropertyPath)
                ? GetOrAddJsonObject<JsonBuilderCollection<TValueSource>>(ClearCollectionMarker(actualPropertyPath))
                : GetOrAddJsonObject<JsonBuilder<TValueSource>>(actualPropertyPath);

            jsonObject.Add(jsonPropertyPath, source);

            return;
        }

        if (CollectionPathHelper.IsCollection(actualPropertyPath))
        {
            var primitiveTypesCollectionSource = new JsonValuePrimitiveTypesCollectionSource(source, Depth + 1);

            _valueProperties.Add(CollectionPathHelper.GetCollectionPath(actualPropertyPath), new JsonBuilderValue(primitiveTypesCollectionSource, isCollection: true));

            return;
        }

        _valueProperties.Add(actualPropertyPath, new JsonBuilderValue(source));
    }

    public override object? GetJsonObject(object data)
    {
        var valueProperties = _valueProperties.Select(r => Selector(r.Key, r.Value));
        var complexProperties = _complexProperties.Select(r => Selector(r.Key, r.Value));

        var dictionary = valueProperties.Concat(complexProperties)
                                        .Where(entry => entry.Value is not null)
                                        .ToImmutableDictionary(entry => entry.Key, entry => entry.Value!);

        return dictionary.Any() ? dictionary : null;

        KeyValuePair<string, object?> Selector(string key, IJsonBuilderEntry entry) => new(key, entry.GetJsonObject(data));
    }

    public string Serialize(object data, JsonSerializerOptions? jsonOptions = default)
    {
        jsonOptions ??= new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        return JsonSerializer.Serialize(GetJsonObject(data), jsonOptions);
    }

    private IJsonBuilderObjectEntry<TValueSource> GetOrAddJsonObject<TObject>(string key) where TObject : IJsonBuilderObjectEntry<TValueSource>, new()
    {
        if (_complexProperties.TryGetValue(key, out var jsonObject))
            return jsonObject;

        jsonObject = new TObject { Depth = Depth + 1 };

        _complexProperties.Add(key, jsonObject);

        return jsonObject;
    }

    private static string ClearCollectionMarker(string propertyPath) => 
        propertyPath.Replace(ConfigurationConstants.CollectionMarker, "");
}