namespace DomainServices.RealEstateValuationService.Api;

internal static class LoggerExtensions
{
    private static readonly Action<ILogger, long, int, Exception> _attachmentDeleteFailed;

    static LoggerExtensions()
    {
        _attachmentDeleteFailed = LoggerMessage.Define<long, int>(
            LogLevel.Warning,
            new EventId(LoggerEventIdCodes.AttachmentDeleteFailed, nameof(AttachmentDeleteFailed)),
            "Attachment {RealEstateValuationAttachmentId} with ExternalId {ExternalId} failed to delete in Preorder service");
    }

    public static void AttachmentDeleteFailed(this ILogger logger, long externalId, int realEstateValuationAttachmentId, Exception ex)
        => _attachmentDeleteFailed(logger, externalId, realEstateValuationAttachmentId, ex);
}
