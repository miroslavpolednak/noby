using CIS.Core.Exceptions;
using Grpc.Core;
using System.Text;
using System.Text.RegularExpressions;

namespace CIS.Infrastructure.gRPC;

public static class GrpcExceptionHelpers
{
    private const string _errorMessageFromRpcExceptionRegex = "Detail=\"(?<error>.*)\"\\)";

    public static RpcException CreateRpcException(StatusCode statusCode, string message, int exceptionCode, List<(string Key, string Value)>? trailers, Exception? baseException = null)
    {
        //TODO nejsem si jisty, ze lze toto vyzadovat vzdy - napr. pri prekladu error kodu z ext systemu
        /*if (exceptionCode <= 0)
            throw new ArgumentOutOfRangeException(nameof(exceptionCode), "exceptionCode <= 0");*/

        Metadata trailersCollection = new();
        trailersCollection.Add(ExceptionHandlingConstants.GrpcTrailerCisCodeKey, exceptionCode.ToString(System.Globalization.CultureInfo.InvariantCulture));

        if (trailers != null)
        {
            foreach (var item in trailers.Where(t => !string.IsNullOrEmpty(t.Value)))
                trailersCollection.Add(new(item.Key + "-bin", TryConvertStringToTrailerValue(item.Value)));
        }

        return new RpcException(new Status(statusCode, message, baseException), trailersCollection, message);
    }

    public static RpcException CreateRpcException(StatusCode statusCode, string message, GrpcErrorCollection errorCollection, Exception? baseException = null)
        => new RpcException(new Status(statusCode, message, baseException), errorCollection.CreateTrailersFromErrors(), message);

    public static RpcException CreateRpcException(string message, int exceptionCode, Exception baseException)
        => CreateRpcException(StatusCode.Unknown, message, exceptionCode, null, baseException);

    public static RpcException CreateRpcException(StatusCode statusCode, string message, int exceptionCode)
        => CreateRpcException(statusCode, message, exceptionCode, null);

    public static RpcException CreateRpcException(BaseCisException exception)
        => CreateRpcException(exception.Message, exception.ExceptionCode, exception);

    #region argument exception
    public static RpcException CreateRpcException(BaseCisArgumentException exception)
    {
        if (string.IsNullOrEmpty(exception.ParamName))
            throw new ArgumentOutOfRangeException(nameof(exception.ParamName), "paramName is empty");

        return CreateRpcException(StatusCode.InvalidArgument, exception.Message, exception.ExceptionCode, new()
        {
            new(ExceptionHandlingConstants.GrpcTrailerCisArgumentKey, exception.ParamName)
        });
    }

    public static RpcException CreateArgumentRpcException(string message, int exceptionCode, string paramName)
    {
        if (string.IsNullOrEmpty(paramName))
            throw new ArgumentOutOfRangeException(nameof(paramName), "paramName is empty");

        return CreateRpcException(StatusCode.InvalidArgument, message, exceptionCode, new()
        {
            new(ExceptionHandlingConstants.GrpcTrailerCisArgumentKey, paramName)
        });
    }
    #endregion argument exception

    public static List<(string Key, string Message)> GetErrorMessagesFromRpcException(this RpcException exception)
    {
        List<(string Key, string Message)> list = new();

        var codes = exception.Trailers?.Get(ExceptionHandlingConstants.GrpcTrailerCisCodeKey)?.Value;
        if (!string.IsNullOrEmpty(codes))
        {
            var ids = codes.Split(',');
            for (int i = 0; i < ids.Length; i++)
            {
                if (int.TryParse(ids[i], out int code))
                {
                    var message = TryConvertTrailerValueToString(exception.Trailers?.GetValueBytes($"ciserr_{i}_{code}-bin"));
                    if (!string.IsNullOrEmpty(message))
                        list.Add((ids[i], message));
                }
            }
        }

        return list;
    }

    public static List<(int Key, string Message)> GetErrorMessagesFromRpcExceptionWithIntKeys(this RpcException exception)
    {
        List<(int Key, string Message)> list = new();

        var codes = exception.Trailers?.Get(ExceptionHandlingConstants.GrpcTrailerCisCodeKey)?.Value;
        if (!string.IsNullOrEmpty(codes))
        {
            var ids = codes.Split(',');
            for (int i = 0; i < ids.Length; i++)
            {
                if (int.TryParse(ids[i], out int code))
                {
                    var message = TryConvertTrailerValueToString(exception.Trailers?.GetValueBytes($"ciserr_{i}_{code}-bin"));
                    if (!string.IsNullOrEmpty(message))
                        list.Add((code, message));
                }
            }
        }

        return list;
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
