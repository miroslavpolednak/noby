using System.Runtime.Caching;
using CIS.Core.Types;
using CIS.Core.Exceptions;

namespace CIS.Infrastructure.Caching.InMemory;

public class InMemoryGlobalCache<T> : InMemoryGlobalCache, IGlobalCache<T> where T : class
{
    public InMemoryGlobalCache(string keyPrefix)
        : base(typeof(T).ToString(), keyPrefix) { }

    public InMemoryGlobalCache(ApplicationKey applicationKey, ApplicationEnvironmentName environment)
        : base(typeof(T).ToString(), applicationKey, environment) { }
}

public class InMemoryGlobalCache : IGlobalCache
{
    public CacheTypes CacheType { get => CacheTypes.InMemory; }
    public const string CisGlobalCacheName = "CisGlobalCache";

    protected readonly string _cacheName;
    protected readonly MemoryCache _cache;

    public InMemoryGlobalCache(string cacheName, string keyPrefix)
    {
        if (string.IsNullOrEmpty(cacheName))
            throw new CisArgumentNullException(6, "Name for InMemoryGlobalCache must be specified", "cacheName");

        _cacheName = cacheName;
        _cache = new MemoryCache(_cacheName);
        KeyPrefix = keyPrefix;
    }

    public InMemoryGlobalCache(string cacheName, ApplicationKey applicationKey, ApplicationEnvironmentName environment)
    {
        if (string.IsNullOrEmpty(cacheName))
            throw new CisArgumentNullException(6, "Name for InMemoryGlobalCache must be specified", "cacheName");

        _cacheName = cacheName;
        _cache = new MemoryCache(_cacheName);
        KeyPrefix = $"{applicationKey}:";
    }

    public string KeyPrefix { get; init; }

    public bool Exists(string key)
    {
        return _cache.Contains(KeyPrefix + key);
    }

    public void Remove(string key)
    {
        _cache.Remove(KeyPrefix + key);
    }

    public string? GetString(string key)
    {
        return _cache.Get(KeyPrefix + key) as string;
    }

    public List<HashItem> GetHashset(string key)
    {
        return _cache.Get(KeyPrefix + key) as List<HashItem> ?? throw new CisArgumentNullException(7, $"Key '{key}' does not exists in Global Cache", "key");
    }

    public string GetHashset(string key, string hashField)
    {
        var item = _cache.Get(KeyPrefix + key) as List<HashItem> ?? throw new CisArgumentNullException(7, $"Key '{key}' does not exists in Global Cache", "key");
        return item.FirstOrDefault(t => t.Name == hashField)?.Value ?? throw new CisArgumentNullException(8, $"HashField '{hashField}' does not exists in Global Cache with key '{key}'", "hashField");
    }

    public bool TryGetHashset(string key, string hashField, out string? value)
    {
        var item = _cache.Get(KeyPrefix + key) as List<HashItem>;
        if (item is not null)
        {
            value = item.FirstOrDefault(t => t.Name == hashField)?.Value;
            return value is not null;
        }
        value = null;
        return false;
    }

    public void Set(string key, string value, TimeSpan? expiry = null)
    {
        if (expiry.HasValue)
            _cache.Set(KeyPrefix + key, value, new DateTimeOffset(DateTime.Now, expiry.Value));
        else
            _cache.Set(KeyPrefix + key, value, new CacheItemPolicy() { AbsoluteExpiration = DateTime.Now.AddDays(1) });
    }

    public void Set(string key, List<HashItem> hashset)
    {
        _cache.Set(KeyPrefix + key, hashset, new CacheItemPolicy() { AbsoluteExpiration = DateTime.Now.AddDays(1) });
    }

    public void Set(string key, HashItem item)
    {
        if (string.IsNullOrWhiteSpace(item.Name))
            throw new CisArgumentNullException(9, "HashItem.Name can not be empty", "Name");

        if (Exists(key))
        {
            var items = (List<HashItem>)_cache.Get(KeyPrefix + key);
            int idx = items.FindIndex(t => t.Name == item.Name);
            if (idx >= 0)
                items.RemoveAt(idx);
            items.Add(item);
            Set(key, items);
        }
        else
            Set(key, new List<HashItem> { item });
    }

    public Task SetAllAsync(string key, IEnumerable<object> items, SerializationTypes serializationType = SerializationTypes.Protobuf)
        => SetAsync(key, items, serializationType);

    public Task<List<T>> GetAllAsync<T>(string key, SerializationTypes serializationType = SerializationTypes.Protobuf)
        => Task.FromResult((List<T>)_cache.Get(KeyPrefix + key));

    public Task SetAsync(string key, object item, SerializationTypes serializationType = SerializationTypes.Protobuf)
    {
        _cache.Set(KeyPrefix + key, item, new CacheItemPolicy() { AbsoluteExpiration = DateTime.Now.AddDays(1) });
        return Task.CompletedTask;
    }

    public Task<T?> GetAsync<T>(string key, SerializationTypes serializationType = SerializationTypes.Protobuf)
        => Task.FromResult((T?)_cache.Get(KeyPrefix + key));
}
