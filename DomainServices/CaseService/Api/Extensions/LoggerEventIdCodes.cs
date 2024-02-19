namespace DomainServices.CaseService.Api;

internal sealed class LoggerEventIdCodes
{
    public const int NewCaseIdCreated = 13501;
    public const int SearchCasesStart = 13503;
    public const int UpdateCaseStateStart = 13504;
    public const int QueueRequestIdSaved = 13505;
    public const int StarbuildStateUpdateFailed = 13506;
    public const int StarbuildStateUpdateSuccess = 13507;
    public const int KafkaMessageCaseIdIncorrectFormat = 13508;
    public const int KafkaMessageCurrentTaskIdIncorrectFormat = 13509;
    public const int KafkaCaseIdNotFound = 13511;
    public const int RequestNotFoundInCache = 13512;
    public const int UpdateActiveTaskStart = 13513;
    public const int BeforeUpdateActiveTasks = 13514;
    public const int KafkaConsumerStarted = 13515;
    public const int KafkaIndividualPricingProcessChangedSkipped = 13516;
    public const int KafkaLoanRetentionProcessChangedSkipped = 13517;
}