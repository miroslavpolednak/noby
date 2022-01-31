using StackExchange.Redis;
using CIS.Core.Exceptions;
using CIS.Core.Types;
using ProtoBuf;
using System.Text.Json;

namespace CIS.Infrastructure.Caching.Redis;

public class RedisGlobalCache<T> : RedisGlobalCache, IGlobalCache<T> where T : class
{
    public RedisGlobalCache(string connectionString, string? keyPrefix)
        : base(connectionString, keyPrefix) { }

    public RedisGlobalCache(IConnectionMultiplexer connectionMultiplexer, string? keyPrefix)
        : base(connectionMultiplexer, keyPrefix) { }

    public RedisGlobalCache(IConnectionMultiplexer connectionMultiplexer, ApplicationKey applicationKey, ApplicationEnvironmentName environment, string? keyPrefix = null)
        : base(connectionMultiplexer, applicationKey, environment, keyPrefix) { }
}

public class RedisGlobalCache : IGlobalCache
{
    public CacheTypes CacheType { get => CacheTypes.Redis; }
    protected IDatabase Database { get; init; }

    public RedisGlobalCache(string connectionString, string? keyPrefix)
    {
        var multiplexer = ConnectionMultiplexer.Connect(connectionString);
        Database = multiplexer.GetDatabase();
        KeyPrefix = keyPrefix;
    }

    public RedisGlobalCache(IConnectionMultiplexer connectionMultiplexer, string? keyPrefix)
    {
        Database = connectionMultiplexer.GetDatabase();
        KeyPrefix = keyPrefix;
    }

    public RedisGlobalCache(IConnectionMultiplexer connectionMultiplexer, ApplicationKey applicationKey, ApplicationEnvironmentName environment, string? keyPrefix = null)
        : this(connectionMultiplexer, $"{keyPrefix}{environment}:{applicationKey}:") { }
    
    public string? KeyPrefix { get; init; }

    public bool Exists(string key)
    {
        return Database.KeyExists(KeyPrefix + key);
    }

    public void Remove(string key)
    {
        Database.KeyDelete(KeyPrefix + key);
    }

    public string GetString(string key)
    {
        return Database.StringGet(KeyPrefix + key);
    }

    public List<HashItem> GetHashset(string key)
    {
        if (!Exists(key)) throw new CisArgumentNullException(7, $"Key '{key}' does not exists in Global Cache", nameof(key));
        return Database.HashGetAll(KeyPrefix + key).Select(t => (HashItem)t).ToList();
    }

    public string GetHashset(string key, string hashField)
    {
        if (!Exists(key)) throw new CisArgumentNullException(7, $"Key '{key}' does not exists in Global Cache", nameof(key));
        var item = Database.HashGet(KeyPrefix + key, hashField);
        if (!item.HasValue) throw new CisArgumentNullException(8, $"HashField '{hashField}' does not exists in Global Cache with key '{key}'", nameof(hashField));
        return item;
    }

    public bool TryGetHashset(string key, string hashField, out string? value)
    {
        if (Exists(key))
        {
            var item = Database.HashGet(KeyPrefix + key, hashField);
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
            throw new CisArgumentNullException(0, "Value cannot be null", nameof(value));
        Database.StringSet(KeyPrefix + key, value, expiry);
    }

    public void Set(string key, List<HashItem> hashset)
    {
        var x = hashset.Select(t => (HashEntry)t).ToArray();
        Database.HashSet(KeyPrefix + key, hashset.Select(t => (HashEntry)t).ToArray());
    }

    public void Set(string key, HashItem item)
    {
        if (string.IsNullOrWhiteSpace(item.Name))
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
            throw new CisArgumentNullException(9, "HashItem.Name can not be empty", nameof(item.Name));
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
        Database.HashSet(KeyPrefix + key, item.Name, item.Value);
    }

    public async Task SetAllAsync(string key, IEnumerable<object> items, SerializationTypes serializationType = SerializationTypes.Protobuf)
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

        await Database.SetAddAsync(KeyPrefix + key, serializedObjects.ToArray());
    }

    public async Task<List<T>> GetAllAsync<T>(string key, SerializationTypes serializationType = SerializationTypes.Protobuf)
    {
        var members = await Database.SetMembersAsync(KeyPrefix + key);
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
                    newList.Add(JsonSerializer.Deserialize<T>(members[i], SerializationOptions.Flexible) ?? throw new FormatException("Could not deserialize RedisValue to JSON"));
                break;

            default:
                throw new NotImplementedException("SerializationType has not been specified");
        }

        return newList;
    }

    public async Task SetAsync(string key, object item, SerializationTypes serializationType = SerializationTypes.Protobuf)
    {
        switch (serializationType)
        {
            case SerializationTypes.Protobuf:
                using (var ms = new MemoryStream())
                {
                    Serializer.Serialize(ms, item);
                    await Database.SetAddAsync(KeyPrefix + key, ms.ToArray());
                }
                break;

            case SerializationTypes.Json:
                var value = JsonSerializer.Serialize(item, SerializationOptions.Flexible);
                await Database.StringSetAsync(KeyPrefix + key, value);
                break;

            default:
                throw new NotImplementedException("SerializationType has not been specified");
        }
    }

    public async Task<T?> GetAsync<T>(string key, SerializationTypes serializationType = SerializationTypes.Protobuf)
    {
        switch (serializationType)
        {
            case SerializationTypes.Protobuf:
                var serializedObject = await Database.StringGetAsync(KeyPrefix + key);
                using (var ms = new MemoryStream(serializedObject))
                    return Serializer.Deserialize<T>(ms);

            case SerializationTypes.Json:
                var serializedObject2 = await Database.StringGetAsync(KeyPrefix + key);
                return JsonSerializer.Deserialize<T>(serializedObject2, SerializationOptions.Flexible);

            default:
                throw new NotImplementedException("SerializationType has not been specified");
        }
    }
}
