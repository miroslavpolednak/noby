using CIS.Core.Exceptions;
using Grpc.Core;
using System.Text;
using System.Text.RegularExpressions;

namespace CIS.Infrastructure.gRPC;

/// <summary>
/// Helpery pro vytváření RpcException.
/// </summary>
public static class GrpcExceptionHelpers
{
    private const string _errorMessageFromRpcExceptionRegex = "Detail=\"(?<error>.*)\"\\)";

    public static RpcException CreateRpcException(StatusCode statusCode, string message)
    {
        return new RpcException(new Status(statusCode, message), message);
    }

    public static RpcException CreateRpcException(CisValidationException exception)
    {
        Metadata trailersCollection = new();

        trailersCollection.Add(ExceptionHandlingConstants.GrpcTrailerCisCodeKey, string.Join(',', exception.Errors.Select(t => t.ExceptionCode)));

        foreach (var item in exception.Errors)
            trailersCollection.Add(new($"ciserr-{item.ExceptionCode}-bin", TryConvertStringToTrailerValue(item.Message)));

        return new RpcException(new Status(StatusCode.InvalidArgument, exception.Errors[0].Message), trailersCollection);
    }

    public static RpcException CreateRpcException(StatusCode statusCode, BaseCisException exception)
    {
        Metadata trailersCollection = new();
        
        trailersCollection.Add(ExceptionHandlingConstants.GrpcTrailerCisCodeKey, exception.ExceptionCode);
        trailersCollection.Add(new($"ciserr-{exception.ExceptionCode}-bin", TryConvertStringToTrailerValue(exception.ExceptionCode)));

        return new RpcException(new Status(statusCode, exception.Message), trailersCollection);
    }

    public static IReadOnlyList<CisExceptionItem> GetErrorMessagesFromRpcException(this RpcException exception)
    {
        List<CisExceptionItem> list = new();

        var codes = exception.Trailers?.Get(ExceptionHandlingConstants.GrpcTrailerCisCodeKey)?.Value;
        if (!string.IsNullOrEmpty(codes))
        {
            var ids = codes.Split(',');
            for (int i = 0; i < ids.Length; i++)
            {
                // takovy kod uz v kolekci je
                if (list.Any(t => t.ExceptionCode == ids[i]))
                {
                    continue;
                }

                if (int.TryParse(ids[i], out int code))
                {
                    var message = TryConvertTrailerValueToString(exception.Trailers?.GetValueBytes($"ciserr-{code}-bin"));
                    if (!string.IsNullOrEmpty(message))
                        list.Add(new(ids[i], message));
                }
            }
        }

        return list.AsReadOnly();
    }

    public static int GetExceptionCodeFromTrailers(this RpcException exception)
    {
        if (int.TryParse(exception.Trailers?.Get(ExceptionHandlingConstants.GrpcTrailerCisCodeKey)?.Value, out int code))
            return code;
        else
            return 0;
    }

    public static string GetErrorMessageFromRpcException(this RpcException exception)
    {
        var matches = Regex.Match(exception.Message, _errorMessageFromRpcExceptionRegex, RegexOptions.Compiled);
        if (matches.Success)
            return matches.Groups["error"].Value;
        else
            return exception.Message;
    }

    // do Trailers je treba davat byte[] protoze jinak se rozhodi non-ascii kodovani
    public static byte[] TryConvertStringToTrailerValue(string? value)
        => Encoding.UTF8.GetBytes(value ?? "");
    public static string? TryConvertTrailerValueToString(byte[]? value)
        => value != null ? Encoding.UTF8.GetString(value) : null;
}
