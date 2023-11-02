namespace DomainServices.ProductService.Api;

internal static class LoggerExtensions
{
    private static readonly Action<ILogger, long, Exception> _cancelMortgageFailed;

    static LoggerExtensions()
    {
        _cancelMortgageFailed = LoggerMessage.Define<long>(
            LogLevel.Debug,
            new EventId(LoggerEventIdCodes.CancelMortgageFailed, nameof(CancelMortgageFailed)),
            "Cancel mortgage failed for product id {ProductId}");
    }

    public static void CancelMortgageFailed(this ILogger logger, long productId, Exception ex)
        => _cancelMortgageFailed(logger, productId, ex);

}
