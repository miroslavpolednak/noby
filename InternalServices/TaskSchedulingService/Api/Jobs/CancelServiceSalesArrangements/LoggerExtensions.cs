namespace CIS.InternalServices.TaskSchedulingService.Api.Jobs.CancelServiceSalesArrangements;

internal static class LoggerExtensions
{
    private static readonly Action<ILogger, int, Exception> _deleteServiceSalesArrangements;

    static LoggerExtensions()
    {
        _deleteServiceSalesArrangements = LoggerMessage.Define<int>(
            LogLevel.Information,
            new EventId(633, nameof(DeleteServiceSalesArrangement)),
            "{SaForDeleteCount} SalesArrangements gonna be deleted");
    }

    public static void DeleteServiceSalesArrangement(this ILogger logger, int SaForDeleteCount)
        => _deleteServiceSalesArrangements(logger, SaForDeleteCount, null!);
}
