namespace DomainServices.CaseService.Api;

internal sealed class LoggerEventIdCodes
{
    public const int NewCaseIdCreated = 13501;
    public const int SearchCasesStart = 13503;
    public const int UpdateCaseStateStart = 13504;
    public const int QueueRequestIdSaved = 13505;
    public const int StarbuildStateUpdateFailed = 13023;
    public const int StarbuildStateUpdateSuccess = 13024;
    public const int KafkaMessageIncorrectFormat = 13002;
    public const int KafkaCaseIdNotFound = 13006;
    public const int RequestNotFoundInCache = 13007;
}