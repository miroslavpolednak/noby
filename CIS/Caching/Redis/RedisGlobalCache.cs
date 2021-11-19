using StackExchange.Redis;
using CIS.Core.Exceptions;
using CIS.Core.Types;
using ProtoBuf;
using System.Text.Json;

namespace CIS.Infrastructure.Caching.Redis;

public class RedisGlobalCache<T> : RedisGlobalCache, IGlobalCache<T> where T : class
{
    public RedisGlobalCache(string connectionString, string keyPrefix)
        : base(connectionString, keyPrefix) { }

    public RedisGlobalCache(IConnectionMultiplexer connectionMultiplexer, string keyPrefix)
        : base(connectionMultiplexer, keyPrefix) { }

    public RedisGlobalCache(IConnectionMultiplexer connectionMultiplexer, ApplicationKey applicationKey, ApplicationEnvironmentName environment)
        : base(connectionMultiplexer, applicationKey, environment) { }
}

public class RedisGlobalCache : IGlobalCache
{
    protected readonly IDatabase _database;

    public RedisGlobalCache(string connectionString, string keyPrefix)
    {
        var multiplexer = ConnectionMultiplexer.Connect(connectionString);
        _database = multiplexer.GetDatabase();
        KeyPrefix = keyPrefix;
    }

    public RedisGlobalCache(IConnectionMultiplexer connectionMultiplexer, string keyPrefix)
    {
        _database = connectionMultiplexer.GetDatabase();
        KeyPrefix = keyPrefix;
    }

    public RedisGlobalCache(IConnectionMultiplexer connectionMultiplexer, ApplicationKey applicationKey, ApplicationEnvironmentName environment)
        : this(connectionMultiplexer, $"{environment}:{applicationKey}:") { }
    
    public string KeyPrefix { get; init; }

    public bool Exists(string key)
    {
        return _database.KeyExists(KeyPrefix + key);
    }

    public void Remove(string key)
    {
        _database.KeyDelete(KeyPrefix + key);
    }

    public string GetString(string key)
    {
        return _database.StringGet(KeyPrefix + key);
    }

    public List<HashItem> GetHashset(string key)
    {
        if (!Exists(key)) throw new CisArgumentNullException(7, $"Key '{key}' does not exists in Global Cache", "key");
        return _database.HashGetAll(KeyPrefix + key).Select(t => (HashItem)t).ToList();
    }

    public string GetHashset(string key, string hashField)
    {
        if (!Exists(key)) throw new CisArgumentNullException(7, $"Key '{key}' does not exists in Global Cache", "key");
        var item = _database.HashGet(KeyPrefix + key, hashField);
        if (!item.HasValue) throw new CisArgumentNullException(8, $"HashField '{hashField}' does not exists in Global Cache with key '{key}'", "hashField");
        return item;
    }

    public bool TryGetHashset(string key, string hashField, out string? value)
    {
        if (Exists(key))
        {
            var item = _database.HashGet(KeyPrefix + key, hashField);
            if (item.HasValue)
            {
                value = item;
                return true;
            }
        }
        value = null;
        return false;
    }

    public void Set(string key, string value, TimeSpan? expiry = null)
    {
        if (value is null)
            throw new CisArgumentNullException(0, "Value cannot be null", "value");
        _database.StringSet(KeyPrefix + key, value, expiry);
    }

    public void Set(string key, List<HashItem> hashset)
    {
        var x = hashset.Select(t => (HashEntry)t).ToArray();
        _database.HashSet(KeyPrefix + key, hashset.Select(t => (HashEntry)t).ToArray());
    }

    public void Set(string key, HashItem item)
    {
        if (string.IsNullOrWhiteSpace(item.Name))
            throw new CisArgumentNullException(9, "HashItem.Name can not be empty", "Name");
        _database.HashSet(KeyPrefix + key, item.Name, item.Value);
    }

    public async Task SetAllAsync(string key, IEnumerable<object> items, SerializationTypes serializationType)
    {
        List<RedisValue> serializedObjects = new();

        switch (serializationType)
        {
            case SerializationTypes.Protobuf:
                foreach (var item in items)
                {
                    using var ms = new MemoryStream();
                    Serializer.Serialize(ms, item);
                    serializedObjects.Add(ms.ToArray());
                }
                break;

            case SerializationTypes.Json:
                foreach (var item in items)
                    serializedObjects.Add(JsonSerializer.SerializeToUtf8Bytes(item, SerializationOptions.Flexible));
                break;

            default:
                throw new NotImplementedException("SerializationType has not been specified");
        }

        await _database.SetAddAsync(KeyPrefix + key, serializedObjects.ToArray());
    }

    public async Task<List<T>> GetAllAsync<T>(string key, SerializationTypes serializationType)
    {
        var members = await _database.SetMembersAsync(KeyPrefix + key);
        List<T> newList = new();

        switch (serializationType)
        {
            case SerializationTypes.Protobuf:
                for (int i = 0; i < members.Length; i++)
                {
                    using (var ms = new MemoryStream(members[i]))
                        newList.Add(Serializer.Deserialize<T>(ms));
                }
                break;

            case SerializationTypes.Json:
                for (int i = 0; i < members.Length; i++)
                    newList.Add(JsonSerializer.Deserialize<T>(members[i], SerializationOptions.Flexible) ?? throw new Exception("Could not deserialize RedisValue to JSON"));
                break;

            default:
                throw new NotImplementedException("SerializationType has not been specified");
        }

        return newList;
    }

    public async Task SetAsync(string key, object item, SerializationTypes serializationType)
    {
        switch (serializationType)
        {
            case SerializationTypes.Protobuf:
                using (var ms = new MemoryStream())
                {
                    Serializer.Serialize(ms, item);
                    await _database.SetAddAsync(KeyPrefix + key, ms.ToArray());
                }
                break;

            case SerializationTypes.Json:
                var bytes = JsonSerializer.SerializeToUtf8Bytes(item, SerializationOptions.Flexible);
                await _database.SetAddAsync(KeyPrefix + key, bytes);
                break;

            default:
                throw new NotImplementedException("SerializationType has not been specified");
        }
    }

    public async Task<T?> GetAsync<T>(string key, SerializationTypes serializationType)
    {
        switch (serializationType)
        {
            case SerializationTypes.Protobuf:
                var serializedObject = await _database.StringGetAsync(KeyPrefix + key);
                using (var ms = new MemoryStream(serializedObject))
                    return Serializer.Deserialize<T>(ms);

            case SerializationTypes.Json:
                var serializedObject2 = await _database.StringGetAsync(KeyPrefix + key);
                return JsonSerializer.Deserialize<T>(serializedObject2, SerializationOptions.Flexible);

            default:
                throw new NotImplementedException("SerializationType has not been specified");
        }
    }
}
