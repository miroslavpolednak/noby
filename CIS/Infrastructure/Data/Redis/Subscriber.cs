using StackExchange.Redis;

namespace CIS.Infrastructure.Data.Redis;

public interface ISubscriber<T> where T : class
{
    Task<RedisValue?> DequeueAsync(string queueId);
    ISubscriber GetSubscriber();

}

public class Subscriber<T> : ISubscriber<T> where T : class
{
    private readonly IConnectionMultiplexer _connection;

    public Subscriber(IConnectionMultiplexer connection)
    {
        _connection = connection;
    }

    public async Task<RedisValue?> DequeueAsync(string queueId)
    {
        return await _connection.DequeueItem(queueId);
    }

    public ISubscriber GetSubscriber()
    {
        return _connection.GetSubscriber();
    }
}
