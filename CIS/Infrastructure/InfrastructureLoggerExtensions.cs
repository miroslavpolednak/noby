namespace CIS.Infrastructure.Logging;

public static class InfrastructureLoggerExtensions
{
    private static readonly Action<ILogger, string, Exception> _dapperQueryException;

    static InfrastructureLoggerExtensions()
    {
        _dapperQueryException = LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(551, nameof(DapperQueryException)),
            "Dapper: {Message}");
    }

    public static void DapperQueryException(this ILogger logger, Exception ex)
        => _dapperQueryException(logger, ex.Message, ex);
}
