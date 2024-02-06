namespace CIS.Infrastructure.Data.Redis;

public interface IHandler
{
    public bool CanHandle(object obj);
    Task Handle(object message);
}

public interface IHandler<T> : IHandler
{
}


public abstract class HandleMessageOfType<T> : IHandler<T> where T : RedisMessage
{
    public bool CanHandle(object obj)
    {
        return obj is T;
    }

    public async Task Handle(object message)
    {
        await HandleMessage((T)message);
    }

    public abstract Task HandleMessage(T message);
}
