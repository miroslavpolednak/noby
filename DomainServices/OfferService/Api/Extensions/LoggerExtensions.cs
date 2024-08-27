namespace DomainServices.OfferService.Api.Extensions;

public static class LoggerExtensions
{
    private static readonly Action<ILogger, long, Exception> _batchIdForProcessing
        = LoggerMessage.Define<long>(
          LogLevel.Information,
          new EventId(LoggerEventIdCodes.BatchIdForProcessing, nameof(BatchIdForProcessing)),
         "BatchId {BatchId} of datamart import gonna be processed");

    public static void BatchIdForProcessing(this ILogger logger, long batchId)
    {
        _batchIdForProcessing(logger, batchId, null!);
    }
}
