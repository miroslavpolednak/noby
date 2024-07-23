namespace CIS.Infrastructure.Logging;

public static class InfrastructureLoggerExtensions
{
    private static readonly Action<ILogger, string, Exception> _dapperQueryException;
    private static readonly Action<ILogger, string?, List<string>?, Exception> _requestHasAdditionalPropsThanContract;

    static InfrastructureLoggerExtensions()
    {
        _dapperQueryException = LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(551, nameof(DapperQueryException)),
            "Dapper: {Message}");

        _requestHasAdditionalPropsThanContract = LoggerMessage.Define<string?, List<string>?>(
            LogLevel.Warning,
            new EventId(552, nameof(RequestHasAdditionalPropsThanContract)),
            "Request {RequestName} has additional properties than contract. Differences: {ExtraProperties}");
    }

    public static void DapperQueryException(this ILogger logger, Exception ex)
        => _dapperQueryException(logger, ex.Message, ex);

    public static void RequestHasAdditionalPropsThanContract(this ILogger logger, string? requestName, List<string>? props)
        => _requestHasAdditionalPropsThanContract(logger, requestName, props, null!);
}
