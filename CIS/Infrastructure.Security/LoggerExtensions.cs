﻿namespace CIS.Infrastructure.Security;

internal static class LoggerExtensions
{
    private static readonly Action<ILogger, Exception> _authHeaderNotFound;
    private static readonly Action<ILogger, Exception> _authIsNotBasic;
    private static readonly Action<ILogger, Exception> _authMissingColon;
    private static readonly Action<ILogger, Exception> _authIncorrectAuthHeader;
    private static readonly Action<ILogger, string, Exception> _authParsedLogin;
    private static readonly Action<ILogger, string, Exception> _adConnectionFailed;
    private static readonly Action<ILogger, int, Exception> _contextUserAdded;
    private static readonly Action<ILogger, string, Exception> _authServiceUserNotFound;

    static LoggerExtensions()
    {
        _authHeaderNotFound = LoggerMessage.Define(
            LogLevel.Debug,
            new EventId(525, nameof(AuthHeaderNotFound)),
            "Authorization header not found");

        _authIsNotBasic = LoggerMessage.Define(
            LogLevel.Error,
            new EventId(521, nameof(AuthIsNotBasic)),
            "Authorization header is not Basic");

        _authMissingColon = LoggerMessage.Define(
           LogLevel.Error,
           new EventId(522, nameof(AuthMissingColon)),
           "Missing ':' in Base authentication");

        _authIncorrectAuthHeader = LoggerMessage.Define(
           LogLevel.Error,
           new EventId(523, nameof(AuthIncorrectAuthHeader)),
           "Incorrect Authorization header");

        _authParsedLogin = LoggerMessage.Define<string>(
           LogLevel.Trace,
           new EventId(524, nameof(AuthParsedLogin)),
           "Parsed as {Login}");

        _adConnectionFailed = LoggerMessage.Define<string>(
           LogLevel.Error,
           new EventId(526, nameof(AdConnectionFailed)),
           "AD query failed for '{Login}'");

        _contextUserAdded = LoggerMessage.Define<int>(
           LogLevel.Trace,
           new EventId(527, nameof(ContextUserAdded)),
           "Context user identity {PartyId} added");

        _authServiceUserNotFound = LoggerMessage.Define<string>(
           LogLevel.Warning,
           new EventId(528, nameof(AuthServiceUserNotFound)),
           "Service user {Username} not found in allowed users");
    }

    public static void AuthIsNotBasic(this ILogger logger)
        => _authIsNotBasic(logger, null!);

    public static void AuthMissingColon(this ILogger logger)
        => _authMissingColon(logger, null!);

    public static void AuthIncorrectAuthHeader(this ILogger logger)
        => _authIncorrectAuthHeader(logger, null!);

    public static void AuthParsedLogin(this ILogger logger, string login)
        => _authParsedLogin(logger, login, null!);

    public static void AuthHeaderNotFound(this ILogger logger)
        => _authHeaderNotFound(logger, null!);

    public static void AdConnectionFailed(this ILogger logger, string login, Exception? ex)
        => _adConnectionFailed(logger, login, ex!);

    public static void ContextUserAdded(this ILogger logger, int partyId)
        => _contextUserAdded(logger, partyId, null!);

    public static void AuthServiceUserNotFound(this ILogger logger, string username)
        => _authServiceUserNotFound(logger, username, null!);
}
