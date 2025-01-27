﻿using Microsoft.Extensions.Logging;

namespace NOBY.Infrastructure;

internal static class LoggerExtensions
{
    private static readonly Action<ILogger, string, Exception> _userWithoutAccess;
    private static readonly Action<ILogger, string, string, Exception> _openIdError;
    private static readonly Action<ILogger, string, Exception> _openIdAuthenticationFailed;
    private static readonly Action<ILogger, string, Exception> _openIdRemoteFailure;
    private static readonly Action<ILogger, string, Exception> _authenticationUserNotFound;

    static LoggerExtensions()
    {
        _userWithoutAccess = LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(LoggerEventIdCodes.SecurityUserWithoutAccess, nameof(UserWithoutAccess)),
            "Cookie handler: user '{Login}' does not have APPLICATION_BasicAccess");

        _openIdError = LoggerMessage.Define<string, string>(
            LogLevel.Warning,
            new EventId(LoggerEventIdCodes.SecurityOpenIdError, nameof(OpenIdError)),
            "OpenID error: {Operation}: {Message}");

        _openIdAuthenticationFailed = LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(LoggerEventIdCodes.SecurityOpenIdAuthenticationFailed, nameof(OpenIdAuthenticationFailed)),
            "OpenID OnAuthenticationFailed: {Message}");

        _openIdRemoteFailure = LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(LoggerEventIdCodes.SecurityOpenIdRemoteFailure, nameof(OpenIdRemoteFailure)),
            "OpenID OnRemoteFailure: {Message}");

        _authenticationUserNotFound = LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(LoggerEventIdCodes.SecurityUserNotFound, nameof(UserNotFound)),
            "Cookie handler: user '{Login}' does not exist");
    }

    public static void UserNotFound(this ILogger logger, string login, Exception ex)
        => _authenticationUserNotFound(logger, login, ex);

    public static void UserWithoutAccess(this ILogger logger, string login)
        => _userWithoutAccess(logger, login, null!);

    public static void OpenIdError(this ILogger logger, string eventName, string message)
        => _openIdError(logger, eventName, message, null!);

    public static void OpenIdAuthenticationFailed(this ILogger logger, Exception? ex)
#pragma warning disable CS8604 // Possible null reference argument.
        => _openIdAuthenticationFailed(logger, ex?.Message ?? "unknown reason", ex);
#pragma warning restore CS8604 // Possible null reference argument.

    public static void OpenIdRemoteFailure(this ILogger logger, Exception? ex)
#pragma warning disable CS8604 // Possible null reference argument.
        => _openIdRemoteFailure(logger, ex?.Message ?? "unknown reason", ex);
#pragma warning restore CS8604 // Possible null reference argument.
}