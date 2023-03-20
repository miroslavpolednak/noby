namespace NOBY.Api.Endpoints.Users;

internal static class UsersLoggerExtensions
{
    private static readonly Action<ILogger, string?, Exception> _userSigningInAs;
    private static readonly Action<ILogger, int, Exception> _userGetCurrentUserInfo;

    static UsersLoggerExtensions()
    {
        _userSigningInAs = LoggerMessage.Define<string?>(
            LogLevel.Debug,
            new EventId(Infrastructure.LoggerEventIdCodes.Endpoints_Users_SigningInAs, nameof(UserSigningInAs)),
            "Signing in as '{Username}'");

        _userGetCurrentUserInfo = LoggerMessage.Define<int>(
            LogLevel.Debug,
            new EventId(Infrastructure.LoggerEventIdCodes.Endpoints_Users_GetCurrentUser, nameof(UserGetCurrentUserInfo)),
            "Try to retrieve info for '{Id}'");
    }

    public static void UserSigningInAs(this ILogger logger, string? username)
        => _userSigningInAs(logger, username, null!);

    public static void UserGetCurrentUserInfo(this ILogger logger, int id)
        => _userGetCurrentUserInfo(logger, id, null!);
}
