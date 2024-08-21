namespace CIS.Infrastructure.Caching.Grpc;

internal static class GrpcClientResponseCacheHelpers
{
    public const Core.Configuration.ICisDistributedCacheConfiguration.SerializationTypes DistributedCacheSerializationType = Core.Configuration.ICisDistributedCacheConfiguration.SerializationTypes.Json;

    public static string CreateCacheKey(in string serviceName, in string methodName, in object key)
    {
        return $"GDCC:{serviceName}-{methodName}-{key}";
    }
}
