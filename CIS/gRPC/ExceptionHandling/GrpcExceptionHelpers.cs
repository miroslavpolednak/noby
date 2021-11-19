using CIS.Core.Exceptions;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CIS.Infrastructure.gRPC;

public static class GrpcExceptionHelpers
{
    private const string _errorMessageFromRpcExceptionRegex = "Detail=\"(?<error>.*)\"\\)";

    public static RpcException CreateRpcException(StatusCode statusCode, string message, int exceptionCode, List<(string Key, string Value)>? trailers, Exception? baseException = null)
    {
        if (exceptionCode <= 0)
            throw new ArgumentOutOfRangeException("exceptionCode", "exceptionCode <= 0");

        Metadata trailersCollection = new();
        trailersCollection.Add(ExceptionHandlingConstants.GrpcTrailerCisCodeKey, exceptionCode.ToString());

        if (trailers != null)
        {
            foreach (var item in trailers)
            {
                if (!string.IsNullOrEmpty(item.Value))
                    trailersCollection.Add(new(item.Key, cleanHeaderString(item.Value)));
                else
                    trailersCollection.Add(new(item.Key, ""));
            }
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
            throw new ArgumentOutOfRangeException("paramName", "paramName is empty");

        return CreateRpcException(StatusCode.InvalidArgument, exception.Message, exception.ExceptionCode, new()
        {
            new(ExceptionHandlingConstants.GrpcTrailerCisArgumentKey, exception.ParamName)
        });
    }

    public static RpcException CreateArgumentRpcException(string message, int exceptionCode, string paramName)
    {
        if (string.IsNullOrEmpty(paramName))
            throw new ArgumentOutOfRangeException("paramName", "paramName is empty");

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
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    var message = exception.Trailers.Get($"ciserr_{i}_{code}")?.Value;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
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
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    var message = exception.Trailers.Get($"ciserr_{i}_{code}")?.Value;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
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

    //TODO nahradit nejak jinak. Je potreba, aby se do headeru neposilaly nonascii znaky. Nebo nastavit grpc channel tak, aby prijimal utf8.
    private static string cleanHeaderString(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return "";
        }
        else
        {
            string stringFormD = value.Normalize(System.Text.NormalizationForm.FormD);
            System.Text.StringBuilder retVal = new System.Text.StringBuilder();
            for (int index = 0; index < stringFormD.Length; index++)
            {
                if (System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stringFormD[index]) != System.Globalization.UnicodeCategory.NonSpacingMark)
                    retVal.Append(stringFormD[index]);
            }
            string normalized = retVal.ToString().Normalize(System.Text.NormalizationForm.FormC);
            return Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(normalized));
        }
    }
}
