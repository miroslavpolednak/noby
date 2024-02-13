namespace DomainServices.CaseService.Api;

internal static class LoggerExtensions
{
    private static readonly Action<ILogger, long, Exception> _newCaseIdCreated;
    private static readonly Action<ILogger, CIS.Core.Types.Paginable, Exception> _searchCasesStart;
    private static readonly Action<ILogger, long, int, Exception> _updateCaseStateStart;
    private static readonly Action<ILogger, long, Exception> _caseStateChangedFailed;
    private static readonly Action<ILogger, int, long, Exception> _queueRequestIdSaved;
    private static readonly Action<ILogger, long, int, Exception> _starbuildStateUpdateFailed;
    private static readonly Action<ILogger, long, int, Exception> _starbuildStateUpdateSuccess;
    private static readonly Action<ILogger, string, string, Exception> _kafkaMessageCaseIdIncorrectFormat;
    private static readonly Action<ILogger, string, string, Exception> _kafkaMessageCurrentTaskIdIncorrectFormat;
    private static readonly Action<ILogger, string, long, Exception> _kafkaCaseIdNotFound;
    private static readonly Action<ILogger, long, Exception> _requestNotFoundInCache;
    private static readonly Action<ILogger, long, int, Exception> _updateActiveTaskStart;
    private static readonly Action<ILogger, bool, bool, Exception> _beforeUpdateActiveTasks;
    private static readonly Action<ILogger, string, Exception> _kafkaConsumerStarted;
    private static readonly Action<ILogger, string, long, decimal, Exception> _kafkaMortgageChangedFinished;

    static LoggerExtensions()
    {
        _kafkaMortgageChangedFinished = LoggerMessage.Define<string, long, decimal>(
            LogLevel.Debug,
            new EventId(LoggerEventIdCodes.KafkaConsumerStarted, nameof(KafkaMortgageChangedFinished)),
            "Kafka message processing in {Consumer} for CaseId {CaseId}: TargetAmount set to {Amount}");

        _kafkaConsumerStarted = LoggerMessage.Define<string>(
            LogLevel.Debug,
            new EventId(LoggerEventIdCodes.KafkaConsumerStarted, nameof(KafkaConsumerStarted)),
            "Kafka message processing in {Consumer}: Started");

        _newCaseIdCreated = LoggerMessage.Define<long>(
            LogLevel.Debug,
            new EventId(LoggerEventIdCodes.NewCaseIdCreated, nameof(NewCaseIdCreated)),
            "Case {CaseId} created");

        _searchCasesStart = LoggerMessage.Define<CIS.Core.Types.Paginable>(
            LogLevel.Debug,
            new EventId(LoggerEventIdCodes.SearchCasesStart, nameof(SearchCasesStart)),
            "Request in SearchCases started with {Pagination}");

        _updateCaseStateStart = LoggerMessage.Define<long, int>(
            LogLevel.Debug,
            new EventId(LoggerEventIdCodes.UpdateCaseStateStart, nameof(UpdateCaseStateStart)),
            "Update Case #{CaseId} state to {State}");

        _caseStateChangedFailed = LoggerMessage.Define<long>(
            LogLevel.Warning,
            new EventId(LoggerEventIdCodes.UpdateCaseStateStart, nameof(CaseStateChangedFailed)),
            "CaseStateChanged failed for {CaseId}");

        _queueRequestIdSaved = LoggerMessage.Define<int, long>(
            LogLevel.Information,
            new EventId(LoggerEventIdCodes.QueueRequestIdSaved, nameof(QueueRequestIdSaved)),
            "Saved RequestId {RequestId} for Case {CaseId}");

        _starbuildStateUpdateFailed = LoggerMessage.Define<long, int>(
            LogLevel.Warning,
            new EventId(LoggerEventIdCodes.StarbuildStateUpdateFailed, nameof(StarbuildStateUpdateFailed)),
            "Case state failed in Starbuild for {CaseId} with requestId {StateId}");

        _starbuildStateUpdateSuccess = LoggerMessage.Define<long, int>(
            LogLevel.Debug,
            new EventId(LoggerEventIdCodes.StarbuildStateUpdateSuccess, nameof(StarbuildStateUpdateSuccess)),
            "Case state changed in Starbuild for {CaseId} by requestId {StateId}");

        _kafkaMessageCaseIdIncorrectFormat = LoggerMessage.Define<string, string>(
            LogLevel.Error,
            new EventId(LoggerEventIdCodes.KafkaMessageCaseIdIncorrectFormat, nameof(KafkaMessageCaseIdIncorrectFormat)),
            "Kafka message processing in {Consumer}: CaseId {CaseId} is not in valid format");

        _kafkaMessageCurrentTaskIdIncorrectFormat = LoggerMessage.Define<string, string>(
            LogLevel.Error,
            new EventId(LoggerEventIdCodes.KafkaMessageCurrentTaskIdIncorrectFormat, nameof(KafkaMessageCurrentTaskIdIncorrectFormat)),
            "Kafka message processing in {Consumer}: CurrentTaskId {CurrentTaskId} is not in valid format");
        
        _kafkaCaseIdNotFound = LoggerMessage.Define<string, long>(
            LogLevel.Warning,
            new EventId(LoggerEventIdCodes.KafkaCaseIdNotFound, nameof(KafkaCaseIdNotFound)),
            "Kafka message processing in {Consumer}: Case {CaseId} not found");

        _requestNotFoundInCache = LoggerMessage.Define<long>(
            LogLevel.Error,
            new EventId(LoggerEventIdCodes.RequestNotFoundInCache, nameof(RequestNotFoundInCache)),
            "Kafka message processing in CaseStateChanged_ProcessingCompletedConsumer: Request {RequestId} not found in cache");
        
        _updateActiveTaskStart = LoggerMessage.Define<long, int>(
            LogLevel.Debug,
            new EventId(LoggerEventIdCodes.UpdateActiveTaskStart, nameof(UpdateActiveTaskStart)),
            "UpdateActiveTask started with CaseId = {CaseId} and TaskIdSb = {TaskIdSb}");
        
        _beforeUpdateActiveTasks = LoggerMessage.Define<bool, bool>(
            LogLevel.Debug,
            new EventId(LoggerEventIdCodes.BeforeUpdateActiveTasks, nameof(BeforeUpdateActiveTasks)),
            "UpdateActiveTask for isActive = {IsActive} and activeTaskFound = {ActiveTaskFound}");
    }

    public static void KafkaMortgageChangedFinished(this ILogger logger, string consumerTypeName, long caseId, decimal amount)
        => _kafkaMortgageChangedFinished(logger, consumerTypeName, caseId, amount, null!);

    public static void KafkaConsumerStarted(this ILogger logger, string consumerTypeName)
        => _kafkaConsumerStarted(logger, consumerTypeName, null!);

    public static void NewCaseIdCreated(this ILogger logger, long caseId)
        => _newCaseIdCreated(logger, caseId, null!);

    public static void SearchCasesStart(this ILogger logger, CIS.Core.Types.Paginable pagination)
        => _searchCasesStart(logger, pagination, null!);

    public static void UpdateCaseStateStart(this ILogger logger, long caseId, int state)
        => _updateCaseStateStart(logger, caseId, state, null!);

    public static void CaseStateChangedFailed(this ILogger logger, long caseId, Exception ex)
        => _caseStateChangedFailed(logger, caseId, ex);

    public static void QueueRequestIdSaved(this ILogger logger, int requestId, long caseId)
        => _queueRequestIdSaved(logger, requestId, caseId, null!);

    public static void StarbuildStateUpdateFailed(this ILogger logger, long caseId, int requestId)
        => _starbuildStateUpdateFailed(logger, caseId, requestId, null!);

    public static void StarbuildStateUpdateSuccess(this ILogger logger, long caseId, int requestId)
        => _starbuildStateUpdateSuccess(logger, caseId, requestId, null!);

    public static void KafkaMessageCaseIdIncorrectFormat(this ILogger logger, string consumerTypeName, string caseId)
        => _kafkaMessageCaseIdIncorrectFormat(logger, consumerTypeName, caseId, null!);

    public static void KafkaMessageCurrentTaskIdIncorrectFormat(this ILogger logger, string consumerTypeName, string currentTaskId)
        => _kafkaMessageCurrentTaskIdIncorrectFormat(logger, consumerTypeName, currentTaskId, null!);
    
    public static void KafkaCaseIdNotFound(this ILogger logger, string consumerTypeName, long caseId)
        => _kafkaCaseIdNotFound(logger, consumerTypeName, caseId, null!);

    public static void RequestNotFoundInCache(this ILogger logger, long caseId)
        => _requestNotFoundInCache(logger, caseId, null!);
    
    public static void UpdateActiveTaskStart(this ILogger logger, long caseId, int taskIdSb)
        => _updateActiveTaskStart(logger, caseId, taskIdSb, null!);
    
    public static void BeforeUpdateActiveTasks(this ILogger logger, bool isActive, bool activeTaskFound)
        => _beforeUpdateActiveTasks(logger, isActive, activeTaskFound, null!);

}
