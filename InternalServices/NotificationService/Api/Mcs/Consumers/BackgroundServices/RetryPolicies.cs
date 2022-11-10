using Polly;

namespace CIS.InternalServices.NotificationService.Api.Mcs.Consumers.BackgroundServices;

public static class RetryPolicies
{
    private static readonly TimeSpan _period = new(0, 0, 8);

    public static readonly AsyncPolicy ForeverRetryPolicy = Policy
        .Handle<Exception>()
        .WaitAndRetryForeverAsync(i => _period);
}