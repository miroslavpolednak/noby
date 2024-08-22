namespace CIS.Infrastructure.Caching.Grpc;

public interface IGrpcServerResponseCache
{
    Task InvalidateEntry(string methodName, object key);

    Task InvalidateEntry(string serviceName, string methodName, object key);
}
