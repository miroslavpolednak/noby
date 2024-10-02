using KafkaFlow;
using System.Text;

namespace DomainServices.CaseService.Api;

internal static class LoggerExtensions
{
    private static readonly Action<ILogger, string, string, string, string?, int?, Exception> _tempMessageHeaderLog =
        LoggerMessage.Define<string, string, string, string?, int?>(
            LogLevel.Information,
            new EventId(1, nameof(TempMessageHeaderLog)),
            "Consuming message type {MessageType} with ID '{MessageId}'; EventId: {SbEventId}; State: {State}; processPhaseCode: {ProcessPhaseCode}");

    private static readonly Action<ILogger, long, Exception> _newCaseIdCreated = 
        LoggerMessage.Define<long>(
            LogLevel.Debug,
            new EventId(LoggerEventIdCodes.NewCaseIdCreated, nameof(NewCaseIdCreated)),
            "Case {CaseId} created");

    private static readonly Action<ILogger, CIS.Core.Types.Paginable, Exception> _searchCasesStart =
        LoggerMessage.Define<CIS.Core.Types.Paginable>(
            LogLevel.Debug,
            new EventId(LoggerEventIdCodes.SearchCasesStart, nameof(SearchCasesStart)),
            "Request in SearchCases started with {Pagination}");

    private static readonly Action<ILogger, long, int, Exception> _updateCaseStateStart =
        LoggerMessage.Define<long, int>(
            LogLevel.Debug,
            new EventId(LoggerEventIdCodes.UpdateCaseStateStart, nameof(UpdateCaseStateStart)),
            "Update Case #{CaseId} state to {State}");

    private static readonly Action<ILogger, long, Exception> _caseStateChangedFailed =
        LoggerMessage.Define<long>(
            LogLevel.Warning,
            new EventId(LoggerEventIdCodes.UpdateCaseStateStart, nameof(CaseStateChangedFailed)),
            "CaseStateChanged failed for {CaseId}");
        
    private static readonly Action<ILogger, int, long, Exception> _queueRequestIdSaved =
        LoggerMessage.Define<int, long>(
            LogLevel.Information,
            new EventId(LoggerEventIdCodes.QueueRequestIdSaved, nameof(QueueRequestIdSaved)),
            "Saved RequestId {RequestId} for Case {CaseId}");

    private static readonly Action<ILogger, long, int, Exception> _starbuildStateUpdateFailed =
        LoggerMessage.Define<long, int>(
            LogLevel.Warning,
            new EventId(LoggerEventIdCodes.StarbuildStateUpdateFailed, nameof(StarbuildStateUpdateFailed)),
            "Case state failed in Starbuild for {CaseId} with requestId {StateId}");

    private static readonly Action<ILogger, long, int, Exception> _starbuildStateUpdateSuccess =
        LoggerMessage.Define<long, int>(
            LogLevel.Debug,
            new EventId(LoggerEventIdCodes.StarbuildStateUpdateSuccess, nameof(StarbuildStateUpdateSuccess)),
            "Case state changed in Starbuild for {CaseId} by requestId {StateId}");

    private static readonly Action<ILogger, string, string, Exception> _kafkaMessageCaseIdIncorrectFormat =
        LoggerMessage.Define<string, string>(
            LogLevel.Error,
            new EventId(LoggerEventIdCodes.KafkaMessageCaseIdIncorrectFormat, nameof(KafkaMessageCaseIdIncorrectFormat)),
            "Kafka message processing in {Consumer}: CaseId {CaseId} is not in valid format");

    private static readonly Action<ILogger, string, string, Exception> _kafkaMessageCurrentTaskIdIncorrectFormat =
        LoggerMessage.Define<string, string>(
            LogLevel.Error,
            new EventId(LoggerEventIdCodes.KafkaMessageCurrentTaskIdIncorrectFormat, nameof(KafkaMessageCurrentTaskIdIncorrectFormat)),
            "Kafka message processing in {Consumer}: CurrentTaskId {CurrentTaskId} is not in valid format");

    private static readonly Action<ILogger, string, long, Exception> _kafkaCaseIdNotFound =
        LoggerMessage.Define<string, long>(
            LogLevel.Warning,
            new EventId(LoggerEventIdCodes.KafkaCaseIdNotFound, nameof(KafkaCaseIdNotFound)),
            "Kafka message processing in {Consumer}: Case {CaseId} not found");

    private static readonly Action<ILogger, long, Exception> _requestNotFoundInCache =
        LoggerMessage.Define<long>(
            LogLevel.Information,
            new EventId(LoggerEventIdCodes.RequestNotFoundInCache, nameof(RequestNotFoundInCache)),
            "Kafka message processing in CaseStateChanged_ProcessingCompletedConsumer: Request {RequestId} not found in cache");

    private static readonly Action<ILogger, long, int, Exception> _updateActiveTaskStart =
        LoggerMessage.Define<long, int>(
            LogLevel.Debug,
            new EventId(LoggerEventIdCodes.UpdateActiveTaskStart, nameof(UpdateActiveTaskStart)),
            "UpdateActiveTask started with CaseId = {CaseId} and TaskIdSb = {TaskIdSb}");

    private static readonly Action<ILogger, bool, bool, Exception> _beforeUpdateActiveTasks =
        LoggerMessage.Define<bool, bool>(
            LogLevel.Debug,
            new EventId(LoggerEventIdCodes.BeforeUpdateActiveTasks, nameof(BeforeUpdateActiveTasks)),
            "UpdateActiveTask for isActive = {IsActive} and activeTaskFound = {ActiveTaskFound}");

    private static readonly Action<ILogger, string, Exception> _kafkaConsumerStarted =
        LoggerMessage.Define<string>(
            LogLevel.Debug,
            new EventId(LoggerEventIdCodes.KafkaConsumerStarted, nameof(KafkaConsumerStarted)),
            "Kafka message processing in {Consumer}: Started");

    private static readonly Action<ILogger, string, long, decimal, Exception> _kafkaMortgageChangedFinished =
        LoggerMessage.Define<string, long, decimal>(
            LogLevel.Debug,
            new EventId(LoggerEventIdCodes.KafkaConsumerStarted, nameof(KafkaMortgageChangedFinished)),
            "Kafka message processing in {Consumer} for CaseId {CaseId}: TargetAmount set to {Amount}");

    private static readonly Action<ILogger, long, int, int, int, Exception> _kafkaIndividualPricingProcessChangedSkipped =
        LoggerMessage.Define<long, int, int, int>(
            LogLevel.Debug,
            new EventId(LoggerEventIdCodes.KafkaIndividualPricingProcessChangedSkipped, nameof(KafkaIndividualPricingProcessChangedSkipped)),
            "IndividualPricingProcessChanged skipped for Case {CaseId} and Task {TaskId} because of ProcessTypeId={ProcessTypeId}; Step={Step}");

    private static readonly Action<ILogger, string, long, long, string, Exception> _kafkaHandlerSkippedDueToState =
        LoggerMessage.Define<string, long, long, string>(
            LogLevel.Debug,
            new EventId(LoggerEventIdCodes.KafkaHandlerSkippedDueToState, nameof(KafkaHandlerSkippedDueToState)),
            "Kafka message processing in {Consumer} skipped for Case {CaseId} and Task {TaskId} because of state={State}");

    private static readonly Action<ILogger, long, long, Exception> _kafkaLoanRetentionProcessChangedSkipped =
        LoggerMessage.Define<long, long>(
            LogLevel.Debug,
            new EventId(LoggerEventIdCodes.KafkaLoanRetentionProcessChangedSkipped, nameof(KafkaLoanRetentionProcessChangedSkipped)),
            "KafkaLoanRetentionProcessChanged consumer skipped for Case {CaseId} and ProcessId {ProcessId} because SA not found");

    // public accessors
    public static void KafkaLoanRetentionProcessChangedSkipped(this ILogger logger, in long caseId, in long processId)
        => _kafkaLoanRetentionProcessChangedSkipped(logger, caseId, processId, null!);

    public static void KafkaMortgageChangedFinished(this ILogger logger, in string consumerTypeName, in long caseId, in decimal amount)
        => _kafkaMortgageChangedFinished(logger, consumerTypeName, caseId, amount, null!);

    public static void KafkaConsumerStarted(this ILogger logger, in string consumerTypeName)
        => _kafkaConsumerStarted(logger, consumerTypeName, null!);

    public static void NewCaseIdCreated(this ILogger logger, in long caseId)
        => _newCaseIdCreated(logger, caseId, null!);

    public static void SearchCasesStart(this ILogger logger, CIS.Core.Types.Paginable pagination)
        => _searchCasesStart(logger, pagination, null!);

    public static void UpdateCaseStateStart(this ILogger logger, in long caseId, in int state)
        => _updateCaseStateStart(logger, caseId, state, null!);

    public static void CaseStateChangedFailed(this ILogger logger, in long caseId, Exception ex)
        => _caseStateChangedFailed(logger, caseId, ex);

    public static void QueueRequestIdSaved(this ILogger logger, in int requestId, in long caseId)
        => _queueRequestIdSaved(logger, requestId, caseId, null!);

    public static void StarbuildStateUpdateFailed(this ILogger logger, in long caseId, in int requestId)
        => _starbuildStateUpdateFailed(logger, caseId, requestId, null!);

    public static void StarbuildStateUpdateSuccess(this ILogger logger, in long caseId, in int requestId)
        => _starbuildStateUpdateSuccess(logger, caseId, requestId, null!);

    public static void KafkaMessageCaseIdIncorrectFormat(this ILogger logger, in string consumerTypeName, in string caseId)
        => _kafkaMessageCaseIdIncorrectFormat(logger, consumerTypeName, caseId, null!);

    public static void KafkaMessageCurrentTaskIdIncorrectFormat(this ILogger logger, in string consumerTypeName, in string currentTaskId)
        => _kafkaMessageCurrentTaskIdIncorrectFormat(logger, consumerTypeName, currentTaskId, null!);
    
    public static void KafkaCaseIdNotFound(this ILogger logger, in string consumerTypeName, in long caseId)
        => _kafkaCaseIdNotFound(logger, consumerTypeName, caseId, null!);

    public static void RequestNotFoundInCache(this ILogger logger, in long caseId)
        => _requestNotFoundInCache(logger, caseId, null!);
    
    public static void UpdateActiveTaskStart(this ILogger logger, in long caseId, in int taskIdSb)
        => _updateActiveTaskStart(logger, caseId, taskIdSb, null!);
    
    public static void BeforeUpdateActiveTasks(this ILogger logger, in bool isActive, in bool activeTaskFound)
        => _beforeUpdateActiveTasks(logger, isActive, activeTaskFound, null!);

    public static void KafkaIndividualPricingProcessChangedSkipped(this ILogger logger, in long caseId, in int taskId, in int processTypeId, in int step)
        => _kafkaIndividualPricingProcessChangedSkipped(logger, caseId, taskId, processTypeId, step, null!);

    public static void KafkaHandlerSkippedDueToState(this ILogger logger, in string consumerTypeName, in long caseId, in long taskId, in string state)
        => _kafkaHandlerSkippedDueToState(logger, consumerTypeName, caseId, taskId, state, null!);

    public static void TempMessageHeaderLog(this ILogger logger, IMessageContext context, in string sbEventId, string? state = null, int? processPhaseCode = null)
        => _tempMessageHeaderLog(logger, context.Message.Value.GetType().FullName!, context.Headers.GetString("messaging.id", Encoding.UTF8), sbEventId, state, processPhaseCode, null!);
}
