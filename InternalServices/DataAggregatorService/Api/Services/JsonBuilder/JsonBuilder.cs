using System.Collections.Immutable;
using System.Collections.Specialized;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using CIS.InternalServices.DataAggregatorService.Api.Services.JsonBuilder.ValueSource;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.JsonBuilder;

internal class JsonBuilder : JsonBuilderEntry, IJsonBuilderObjectEntry
{
    private readonly Dictionary<string, IJsonBuilderEntry> _valueProperties = new();
    private readonly Dictionary<string, IJsonBuilderObjectEntry> _complexProperties = new();

    public void Add(string jsonPropertyPath, IJsonValueSource source) => Add(jsonPropertyPath.Split('.'), source);

    public virtual void Add(string[] jsonPropertyPath, IJsonValueSource source)
    {
        var actualPropertyPath = jsonPropertyPath[Depth];

        if (jsonPropertyPath.Length > Depth + 1)
        {
            var jsonObject = CollectionPathHelper.IsCollection(actualPropertyPath)
                ? GetOrAddJsonObject(ClearCollectionMarker(actualPropertyPath), CreateNew<JsonBuilderCollection>)
                : GetOrAddJsonObject(actualPropertyPath, CreateNew<JsonBuilder>);

            jsonObject.Add(jsonPropertyPath, source);

            return;
        }

        if (CollectionPathHelper.IsCollection(actualPropertyPath))
        {
            source = new JsonValuePrimitiveTypesCollectionSource(source.FieldPath, Depth + 1);

            _valueProperties.Add(CollectionPathHelper.GetCollectionPath(actualPropertyPath), new JsonBuilderValue(source, isCollection: true));

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

    private IJsonBuilderObjectEntry GetOrAddJsonObject(string key, Func<IJsonBuilderObjectEntry> factory)
    {
        if (_complexProperties.TryGetValue(key, out var jsonObject))
            return jsonObject;

        jsonObject = factory();

        _complexProperties.Add(key, jsonObject);

        return jsonObject;
    }

    private TObject CreateNew<TObject>() where TObject : IJsonBuilderEntry, new() => new() { Depth = Depth + 1 };

    private static string ClearCollectionMarker(string propertyPath) => 
        propertyPath.Replace(ConfigurationConstants.CollectionMarker, "");
}