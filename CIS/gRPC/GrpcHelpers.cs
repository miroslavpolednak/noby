using CIS.Core.Exceptions;
using Grpc.Core;

namespace CIS.Infrastructure.gRPC;

public static class GrpcHelpers
{
    public static string? GetValueFromTrailers(this RpcException exception, string key)
    {
        return exception.Trailers?.Get(key)?.Value;
    }

    public static string? GetArgumentFromTrailers(this RpcException exception)
    {
        return exception.Trailers?.Get(ExceptionHandlingConstants.GrpcTrailerCisArgumentKey)?.Value;
    }

    public static int GetIntValueFromTrailers(this RpcException exception, string key)
    {
        int.TryParse(exception.Trailers?.Get(key)?.Value, out int i);
        return i;
    }
}
