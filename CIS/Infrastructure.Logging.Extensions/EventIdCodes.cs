﻿namespace CIS.Infrastructure.Logging;

public sealed class EventIdCodes
{
    public const int RequestHandlerStarted = 501;
    public const int RequestHandlerStartedWithId = 502;
    public const int RequestHandlerStartedWithRequest = 517;
    public const int RequestHandlerFinished = 503;
    public const int EntityAlreadyExist = 504;
    public const int EntityNotFound = 505;
    public const int FoundItems = 506;
    public const int EntityCreated = 507;
    public const int ServiceUnavailable = 508;
    public const int ExtServiceUnavailable = 509;
    public const int GeneralException = 510;
    public const int GeneralException2 = 511;
    public const int InvalidArgument = 512;
    public const int ValidationException = 513;
    public const int ItemFoundInCache = 514;
    public const int TryAddItemToCache = 515;
    public const int LogSerializedObject = 516;
    public const int ExtServiceRetryCall = 517;
    public const int ExtServiceRequest = 518;
    public const int HttpRequestPayload = 519;
    public const int HttpResponsePayload = 520;
}