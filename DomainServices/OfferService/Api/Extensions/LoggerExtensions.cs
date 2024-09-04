namespace DomainServices.OfferService.Api.Extensions;

public static class LoggerExtensions
{
    private static readonly Action<ILogger, long, Exception> _batchIdForProcessing
        = LoggerMessage.Define<long>(
          LogLevel.Information,
          new EventId(LoggerEventIdCodes.BatchIdForProcessing, nameof(BatchIdForProcessing)),
         "BatchId {BatchId} of datamart import gonna be processed");

    private static readonly Action<ILogger, long, Exception> _datamartImportMaxAllowedTransactionExceeded
        = LoggerMessage.Define<long>(
          LogLevel.Information,
          new EventId(LoggerEventIdCodes.DatamartImportMaxAllowedTransactionExceeded, nameof(DatamartImportMaxAllowedTransactionExceeded)),
         "Max allowed number transactions exceeded, during import from datamart. BatchId: {BatchId}");

    public static void BatchIdForProcessing(this ILogger logger, long batchId)
    {
        _batchIdForProcessing(logger, batchId, null!);
    }

    public static void DatamartImportMaxAllowedTransactionExceeded(this ILogger logger, long batchId)
    {
        _datamartImportMaxAllowedTransactionExceeded(logger, batchId, null!);
    }
}
