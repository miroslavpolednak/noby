namespace CIS.InternalServices.NotificationService.Api;

internal static class LoggerExtensions
{
    private static readonly Action<ILogger, Contracts.v2.NotificationChannels, Guid, Exception> _notificationRequestReceived;
    private static readonly Action<ILogger, Guid, Contracts.v2.NotificationChannels, Exception> _notificationSent;
    private static readonly Action<ILogger, Guid, Contracts.v2.NotificationChannels, Exception> _notificationFailedToSend;
    private static readonly Action<ILogger, Guid, Exception> _saveAttachmentFailed;

    static LoggerExtensions()
    {
        _notificationRequestReceived = LoggerMessage.Define<Contracts.v2.NotificationChannels, Guid>(
            LogLevel.Information,
            new EventId(LoggerEventIdCodes.NotificationRequestReceived, nameof(NotificationRequestReceived)),
            "Notification request of type {NotificationType} has been received and ID {NotificationId} assigned");

        _notificationSent = LoggerMessage.Define<Guid, Contracts.v2.NotificationChannels>(
            LogLevel.Information,
            new EventId(LoggerEventIdCodes.NotificationSent, nameof(NotificationSent)),
            "Notification {NotificationId} of type {NotificationType} has been sent");

        _notificationFailedToSend = LoggerMessage.Define<Guid, Contracts.v2.NotificationChannels>(
            LogLevel.Error,
            new EventId(LoggerEventIdCodes.NotificationSent, nameof(NotificationFailedToSend)),
            "Notification {NotificationId} of type {NotificationType} send attempt failed");

        _saveAttachmentFailed = LoggerMessage.Define<Guid>(
            LogLevel.Error,
            new EventId(LoggerEventIdCodes.SaveAttachmentFailed, nameof(SaveAttachmentFailed)),
            "Saving attachment for notification {NotificationId} failed");
    }

    public static void NotificationRequestReceived(this ILogger logger, Guid notificationId, Contracts.v2.NotificationChannels channel)
        => _notificationRequestReceived(logger, channel, notificationId, null!);

    public static void NotificationSent(this ILogger logger, Guid notificationId, Contracts.v2.NotificationChannels channel)
        => _notificationSent(logger, notificationId, channel, null!);

    public static void NotificationFailedToSend(this ILogger logger, Guid notificationId, Contracts.v2.NotificationChannels channel, Exception ex)
        => _notificationFailedToSend(logger, notificationId, channel, ex);

    public static void SaveAttachmentFailed(this ILogger logger, Guid notificationId, Exception ex)
        => _saveAttachmentFailed(logger, notificationId, ex);
}
