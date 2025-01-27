﻿namespace DomainServices.OfferService.Api.Extensions;

public sealed class LoggerEventIdCodes
{
    public const int BatchIdForProcessing = 10501;
    public const int KafkaMessageCaseIdIncorrectFormat = 10502;
    public const int KafkaMessageCurrentTaskIdIncorrectFormat = 10503;
    public const int KafkaCaseIdNotFound = 10504;
    public const int DatamartImportMaxAllowedTransactionExceeded = 10505;
}
