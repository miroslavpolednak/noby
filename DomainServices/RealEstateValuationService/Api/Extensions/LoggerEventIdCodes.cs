namespace DomainServices.RealEstateValuationService.Api;

internal sealed class LoggerEventIdCodes
{
    public const int AttachmentDeleteFailed = 22501;
    public const int KafkaMessageCaseIdIncorrectFormat = 22502;
    public const int KafkaMessageCurrentTaskIdIncorrectFormat = 22503;
    public const int RealEstateValuationNotFound = 22504;
    public const int RevaluationFinished = 22505;
    public const int CreateKbmodelFlat = 22506;
    public const int RealEstateValuationStateIdChanged = 22507;
}