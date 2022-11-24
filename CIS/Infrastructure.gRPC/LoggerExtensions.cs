using CIS.Infrastructure.Logging;
using Microsoft.Extensions.Logging;

namespace CIS.Infrastructure.gRPC;

internal static class LoggerExtensions
{
    private static readonly Action<ILogger, string, string, Exception> _clientUncoughtRpcException;
    private static readonly Action<ILogger, string, Exception> _serverUncoughtRpcException;
    private static readonly Action<ILogger, int, string, string, Exception> _clientInvalidArgument;

    static LoggerExtensions()
    {
        _clientInvalidArgument = LoggerMessage.Define<int, string, string>(
            LogLevel.Debug,
            new EventId(706, nameof(ClientInvalidArgument)),
            "{Code} in {ArgumentName}: {Message}");

        _clientUncoughtRpcException = LoggerMessage.Define<string, string>(
            LogLevel.Error,
            new EventId(701, nameof(ClientUncoughtRpcException)),
            "Uncought client exception in {MethodName}: {Message}");

        _serverUncoughtRpcException = LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(705, nameof(ServerUncoughtRpcException)),
            "Uncought server exception: {Message}");
    }

    public static void ClientInvalidArgument(this ILogger logger, int code, string argumentName, Exception ex)
        => _clientInvalidArgument(logger, code, argumentName, ex.Message, ex);

    public static void ClientUncoughtRpcException(this ILogger logger, string methodName, Exception ex)
        => _clientUncoughtRpcException(logger, methodName, ex.Message, ex);

    public static void ServerUncoughtRpcException(this ILogger logger, Exception ex)
        => _serverUncoughtRpcException(logger, ex.Message, ex);
}