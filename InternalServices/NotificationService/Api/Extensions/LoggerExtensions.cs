namespace CIS.InternalServices.NotificationService.Api;

internal static class LoggerExtensions
{
    private static readonly Action<ILogger, Contracts.v2.NotificationChannels, Guid, Exception> _notificationRequestReceived;
    private static readonly Action<ILogger, Guid, Contracts.v2.NotificationChannels, Exception> _notificationSent;
    private static readonly Action<ILogger, Guid, Contracts.v2.NotificationChannels, Exception> _notificationFailedToSend;
    private static readonly Action<ILogger, Guid, Exception> _saveAttachmentFailed;
    private static readonly Action<ILogger, string, Exception> _kafkaNotificationResultIdEmpty;
    private static readonly Action<ILogger, Guid, Exception> _kafkaNotificationResultNotificationNotFound;
    private static readonly Action<ILogger, string, Guid, Exception> _kafkaNotificationResultUnknownState;

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

        _kafkaNotificationResultIdEmpty = LoggerMessage.Define<string>(
            LogLevel.Trace,
            new EventId(LoggerEventIdCodes.KafkaNotificationResultIdEmpty, nameof(KafkaNotificationResultIdEmpty)),
            "Kafka consumer NotificationReport: message id '{NotificationId}' is empty or is not guid");

        _kafkaNotificationResultNotificationNotFound = LoggerMessage.Define<Guid>(
            LogLevel.Trace,
            new EventId(LoggerEventIdCodes.KafkaNotificationResultNotificationNotFound, nameof(KafkaNotificationResultNotificationNotFound)),
            "Kafka consumer NotificationReport: notification {NotificationId} not found");

        _kafkaNotificationResultUnknownState = LoggerMessage.Define<string, Guid>(
            LogLevel.Warning,
            new EventId(LoggerEventIdCodes.KafkaNotificationResultUnknownState, nameof(KafkaNotificationResultUnknownState)),
            "Kafka consumer NotificationReport: unknown state '{State}' for notification {NotificationId}");
    }

    public static void NotificationRequestReceived(this ILogger logger, in Guid notificationId, in Contracts.v2.NotificationChannels channel)
        => _notificationRequestReceived(logger, channel, notificationId, null!);

    public static void NotificationSent(this ILogger logger, in Guid notificationId, in Contracts.v2.NotificationChannels channel)
        => _notificationSent(logger, notificationId, channel, null!);

    public static void NotificationFailedToSend(this ILogger logger, in Guid notificationId, in Contracts.v2.NotificationChannels channel, Exception ex)
        => _notificationFailedToSend(logger, notificationId, channel, ex);

    public static void SaveAttachmentFailed(this ILogger logger, in Guid notificationId, Exception ex)
        => _saveAttachmentFailed(logger, notificationId, ex);

    public static void KafkaNotificationResultIdEmpty(this ILogger logger, in string notificationId)
        => _kafkaNotificationResultIdEmpty(logger, notificationId, null!);

    public static void KafkaNotificationResultNotificationNotFound(this ILogger logger, in Guid notificationId)
        => _kafkaNotificationResultNotificationNotFound(logger, notificationId, null!);

    public static void KafkaNotificationResultUnknownState(this ILogger logger, in string state, in Guid notificationId)
        => _kafkaNotificationResultUnknownState(logger, state, notificationId, null!);
}
