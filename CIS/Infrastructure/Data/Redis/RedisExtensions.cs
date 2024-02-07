using StackExchange.Redis;
using System.Text.Json;

namespace CIS.Infrastructure.Data.Redis;

public static class RedisExtensions
{
    public static async Task<IEnumerable<T?>> GetGroupValues<T>(this IConnectionMultiplexer connection, string groupId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(groupId);

        var db = connection.GetDatabase();

        var values = await db.HashValuesAsync(groupId);

        return values.Select(x => JsonSerializer.Deserialize<T>(x!));
    }

    public static async Task<IEnumerable<string>> GetGroupKeys(this IConnectionMultiplexer connection, string groupId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(groupId);

        var db = connection.GetDatabase();

        var keys = await db.ExecuteAsync("keys", $"{groupId}*");

        if (keys.IsNull)
            return Enumerable.Empty<string>();

        return (string[])keys!;
    }

    public static async Task AddItem<T>(this IConnectionMultiplexer connection, string groupId, string key, T item)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(groupId);
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        ArgumentNullException.ThrowIfNull(item);

        var db = connection.GetDatabase();

        await db.HashSetAsync(groupId, key, JsonSerializer.Serialize(item));
    }


    public static async Task<TResource?> GetItem<TResource>(this IConnectionMultiplexer connection, string groupId, string key)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(groupId);
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        var db = connection.GetDatabase();

        var item = await db.HashGetAsync(groupId, key);
        if (item.IsNull)
            return default!;
        else return JsonSerializer.Deserialize<TResource>(item!)!;
    }

    public static async Task RemoveItem(this IConnectionMultiplexer connection, string groupId, string key)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(groupId);
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        var db = connection.GetDatabase();

        await db.HashDeleteAsync(groupId, key);
    }

    public static async Task Publish<TResource>(this IConnectionMultiplexer connection, string channel, TResource item)
    {
        ArgumentNullException.ThrowIfNull(item);
        ArgumentException.ThrowIfNullOrWhiteSpace(channel);

        var subscriber = connection.GetSubscriber();

        await subscriber.PublishAsync(RedisChannel.Pattern(channel), JsonSerializer.Serialize(item, item.GetType()));
    }

    public static async Task EnqueueItem<TResource>(this IConnectionMultiplexer connection, string queueId, TResource item)
    {
        ArgumentNullException.ThrowIfNull(item);
        ArgumentException.ThrowIfNullOrWhiteSpace(queueId);

        var database = connection.GetDatabase();

        await database.ListRightPushAsync(queueId, JsonSerializer.Serialize(item, item.GetType()));
    }

    public static async Task<RedisValue?> DequeueItem(this IConnectionMultiplexer connection, string queueId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(queueId);
        var database = connection.GetDatabase();

        var item = await database.ListLeftPopAsync(queueId);
        if (item.IsNull)
            return null;

        return item!;
    }

}
