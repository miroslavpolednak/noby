namespace CIS.Infrastructure.Caching;

public record HashItem(string Name, string? Value)
{
    public static implicit operator HashItem(StackExchange.Redis.HashEntry d) => new HashItem(d.Name, d.Value);

    public static implicit operator StackExchange.Redis.HashEntry(HashItem d) => new StackExchange.Redis.HashEntry(d.Name, d.Value);
}
