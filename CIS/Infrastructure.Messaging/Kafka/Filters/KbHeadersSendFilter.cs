using MassTransit;
using System.Diagnostics;

namespace CIS.Infrastructure.Messaging.Kafka.Filters;

public sealed class KbHeadersSendFilter<T>
    : IFilter<SendContext<T>>
    where T : class
{
    private const string DefaultAppValue = "NOBY";
    private const string DefaultAppCompOriginatorValue = "NOBY.FEAPI";

    private readonly CIS.Core.Configuration.ICisEnvironmentConfiguration _environmentConfiguration;
    private readonly CIS.Core.Security.ICurrentUserAccessor? _currentUserAccessor;

    public KbHeadersSendFilter(CIS.Core.Configuration.ICisEnvironmentConfiguration environmentConfiguration, Core.Security.ICurrentUserAccessor? currentUserAccessor)
    {
        _environmentConfiguration = environmentConfiguration;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task Send(SendContext<T> context, IPipe<SendContext<T>> next)
    {
        context.Headers.Set("b3", $"{Activity.Current!.TraceId}-{Activity.Current.SpanId}-1-{Activity.Current.ParentSpanId}");
        context.Headers.Set("X_HYPHEN_KB_HYPHEN_Orig_HYPHEN_System_HYPHEN_Identity", $$"""{"app":"{{DefaultAppValue}}","appComp":"{{DefaultAppCompOriginatorValue}}"}""");
        context.Headers.Set("X_HYPHEN_KB_HYPHEN_Caller_HYPHEN_System_HYPHEN_Identity", $$"""{"app":"{{DefaultAppValue}}","appComp":"{{_environmentConfiguration.DefaultApplicationKey}}"}""");
        context.Headers.Set("messaging.id", Guid.NewGuid().ToString("N"));
        context.Headers.Set("messaging.timestamp", DateTime.Now.ToUniversalTime().ToString("O"));
        context.Headers.Set("contentType", "avro/binary");
        context.Headers.Set("messaging.messageType", "SIMPLE");
        context.Headers.Set("messaging.schemaRegistryType", "confluent");

        // user info
        if (!string.IsNullOrEmpty(_currentUserAccessor?.User?.Login))
        {
            int index = _currentUserAccessor.User.Login.IndexOf('=');
            if (index > 0)
            {
                context.Headers.Set("X_HYPHEN_KB_HYPHEN_Party_HYPHEN_Identity_HYPHEN_In_HYPHEN_Service", $$"""{"partyIdIS":[{"partyId":{"idScheme":"{{_currentUserAccessor.User.Login[..index]}}","id":"{{_currentUserAccessor.User.Login[(index + 1)..]}}"},"usg":"AUTH"}]}""");
            }
        }

        await next.Send(context);
    }

    public void Probe(ProbeContext context) { }
}