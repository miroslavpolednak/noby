namespace NOBY.Api.Endpoints.Codebooks;

internal static class CodebooksModuleLoggerExtensions
{
    private static readonly Action<ILogger, List<(string Original, string Key)>, Exception> _codebooksGetAllStarted;

    static CodebooksModuleLoggerExtensions()
    {
        _codebooksGetAllStarted = LoggerMessage.Define<List<(string Original, string Key)>>(
            LogLevel.Debug,
            new EventId(Infrastructure.LoggerEventIdCodes.Endpoints_Codebooks_GetAllStarted, nameof(CodebooksGetAllStarted)),
            "Getting {Codebooks}");
    }

    public static void CodebooksGetAllStarted(this ILogger logger, List<(string Original, string Key)> codebooks)
        => _codebooksGetAllStarted(logger, codebooks, null!);
}