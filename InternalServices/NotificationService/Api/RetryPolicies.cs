using Polly;

namespace CIS.InternalServices.NotificationService.Api;

public static class RetryPolicies
{
    private static readonly TimeSpan _period = new(0, 1, 0);

    public static readonly AsyncPolicy ForeverRetryPolicy = Policy
        .Handle<Exception>()
        .WaitAndRetryForeverAsync(i => _period);
}