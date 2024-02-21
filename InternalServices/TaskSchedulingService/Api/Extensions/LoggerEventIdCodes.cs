namespace CIS.InternalServices.TaskSchedulingService.Api;

internal sealed class LoggerEventIdCodes
{
    public const int TryToAcquireScheduleLock = 600;
    public const int TriggerIsDisabled = 601;
    public const int InvalidCronExpression = 602;
    public const int JobNotFound = 603;
    public const int SkippingTrigger = 604;
    public const int JobAlreadyRunning = 605;
    public const int EnqueingJob = 606;
    public const int JobFinished = 607;
    public const int JobFailed = 608;
    public const int JobLocked = 609;
    public const int InstanceLockAcquireFailed = 610;
    public const int InstanceUnableToAcquireLock = 611;
}