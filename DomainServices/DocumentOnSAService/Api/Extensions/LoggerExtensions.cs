﻿namespace DomainServices.DocumentOnSAService.Api.Extensions;

public static class LoggerExtensions
{
    private static readonly Action<ILogger, string, int, Exception> _unarchivedDocumentsOnSa;
    private static readonly Action<ILogger, string, int, int, Exception> _alreadyArchived;
    private static readonly Action<ILogger, int, Exception> _updateDocumentStatusFailed;
    private static readonly Action<ILogger, int, Exception> _updateCustomerFailed;

    static LoggerExtensions()
    {
        _unarchivedDocumentsOnSa = LoggerMessage.Define<string, int>(
          LogLevel.Information,
          new EventId(LoggerEventIdCodes.UnarchivedDocumentsOnSa, nameof(UnarchivedDocumentsOnSa)),
          "{ServiceName}: {Count} unarchived documentsOnSa");

        _alreadyArchived = LoggerMessage.Define<string, int, int>(
            LogLevel.Information,
            new EventId(LoggerEventIdCodes.AlreadyArchived, nameof(AlreadyArchived)),
          "{ServiceName}:From {UnArchCount} unarchived documentsOnSa, {ArchCount} have been already archived}");

        _updateDocumentStatusFailed = LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(LoggerEventIdCodes.UpdateDocumentStatusFailed, nameof(UpdateDocumentStatusFailed)),
           "Update documentOnSa {DocumentId} failed");

        _updateCustomerFailed = LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(LoggerEventIdCodes.UpdateCustomerFailed, nameof(UpdateCustomerFailed)),
           "Update customerOnSa {CustomerOnSaId} failed");
    }

    public static void UnarchivedDocumentsOnSa(this ILogger logger, string serviceName, int count)
     => _unarchivedDocumentsOnSa(logger, serviceName, count, default!);

    public static void AlreadyArchived(this ILogger logger, string serviceName, int unArchivedCount, int archivedCount)
     => _alreadyArchived(logger, serviceName, unArchivedCount, archivedCount, default!);

    public static void UpdateDocumentStatusFailed(this ILogger logger, int documentId, Exception exception)
     => _updateDocumentStatusFailed(logger, documentId, exception);

    public static void UpdateCustomerFailed(this ILogger logger, int customerOnSaId, Exception exception)
    => _updateCustomerFailed(logger, customerOnSaId, exception);

}