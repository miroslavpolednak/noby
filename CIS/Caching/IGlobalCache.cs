namespace CIS.Infrastructure.Caching;

public interface IGlobalCache
{
    CacheTypes CacheType { get; }

    string? KeyPrefix { get; }

    bool Exists(string key);

    void Remove(string key);

    string? GetString(string key);

    List<HashItem> GetHashset(string key);

    string GetHashset(string key, string hashField);

    bool TryGetHashset(string key, string hashField, out string? value);

    void Set(string key, string value, TimeSpan? expiry = null);

    void Set(string key, List<HashItem> hashset);

    void Set(string key, HashItem item);

    Task<List<T>> GetAllAsync<T>(string key, SerializationTypes serializationType = SerializationTypes.Protobuf);

    Task SetAllAsync(string key, IEnumerable<object> items, SerializationTypes serializationType = SerializationTypes.Protobuf);

    Task SetAsync(string key, object item, SerializationTypes serializationType = SerializationTypes.Protobuf);

    Task<T?> GetAsync<T>(string key, SerializationTypes serializationType = SerializationTypes.Protobuf);
}

public interface IGlobalCache<T> : IGlobalCache { }
