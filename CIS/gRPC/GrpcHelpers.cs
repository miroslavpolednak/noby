using CIS.Core.Exceptions;
using Grpc.Core;

namespace CIS.Infrastructure.gRPC;

public static class GrpcHelpers
{
    public static string? GetValueFromTrailers(this RpcException exception, string key)
    {
        if (HasBinaryHeaderSuffix(key))
            return GrpcExceptionHelpers.TryConvertTrailerValueToString(exception.Trailers?.GetValueBytes(key));
        else
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

    /// <summary>
    /// Returns <c>true</c> if the key has "-bin" binary header suffix.
    /// </summary>
    private static bool HasBinaryHeaderSuffix(string key)
    {
        // We don't use just string.EndsWith because its implementation is extremely slow
        // on CoreCLR and we've seen significant differences in gRPC benchmarks caused by it.
        // See https://github.com/dotnet/coreclr/issues/5612

        int len = key.Length;
        if (len >= 4 &&
            key[len - 4] == '-' &&
            key[len - 3] == 'b' &&
            key[len - 2] == 'i' &&
            key[len - 1] == 'n')
        {
            return true;
        }
        return false;
    }
}
