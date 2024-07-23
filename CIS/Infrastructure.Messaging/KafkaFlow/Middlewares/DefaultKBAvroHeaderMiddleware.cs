using System.Diagnostics;
using CIS.Core.Configuration;
using CIS.Core.Security;
using KafkaFlow;

namespace CIS.Infrastructure.Messaging.KafkaFlow.Middlewares;

internal sealed class DefaultKBAvroHeaderMiddleware : IMessageMiddleware
{
    private const string DefaultAppValue = "NOBY";
    private const string DefaultAppCompOriginatorValue = "NOBY.FEAPI";

    private readonly ICisEnvironmentConfiguration _cisEnvironmentConfiguration;
    private readonly ICurrentUserAccessor? _currentUserAccessor;

    public DefaultKBAvroHeaderMiddleware(ICisEnvironmentConfiguration cisEnvironmentConfiguration, ICurrentUserAccessor? currentUserAccessor)
    {
        _cisEnvironmentConfiguration = cisEnvironmentConfiguration;
        _currentUserAccessor = currentUserAccessor;
    }

    public Task Invoke(IMessageContext context, MiddlewareDelegate next)
    {
        context.Headers.SetString("b3", $"{Activity.Current!.TraceId}-{Activity.Current.SpanId}-1-{Activity.Current.ParentSpanId}");
        context.Headers.SetString("X_HYPHEN_KB_HYPHEN_Orig_HYPHEN_System_HYPHEN_Identity", $$"""{"app":"{{DefaultAppValue}}","appComp":"{{DefaultAppCompOriginatorValue}}"}""");
        context.Headers.SetString("X_HYPHEN_KB_HYPHEN_Caller_HYPHEN_System_HYPHEN_Identity", $$"""{"app":"{{DefaultAppValue}}","appComp":"{{_cisEnvironmentConfiguration.DefaultApplicationKey}}"}""");
        context.Headers.SetString("messaging.id", Guid.NewGuid().ToString("N"));
        context.Headers.SetString("messaging.timestamp", DateTime.Now.ToUniversalTime().ToString("O"));
        context.Headers.SetString("contentType", "avro/binary");
        context.Headers.SetString("messaging.messageType", "SIMPLE");
        context.Headers.SetString("messaging.schemaRegistryType", "confluent");

        // user info
        if (!string.IsNullOrEmpty(_currentUserAccessor?.User?.Login))
        {
            var index = _currentUserAccessor.User.Login.IndexOf('=');

            if (index > 0)
            {
                context.Headers.SetString("X_HYPHEN_KB_HYPHEN_Party_HYPHEN_Identity_HYPHEN_In_HYPHEN_Service",
                                          $$"""{"partyIdIS":[{"partyId":{"idScheme":"{{_currentUserAccessor.User.Login[..index]}}","id":"{{_currentUserAccessor.User.Login[(index + 1)..]}}"},"usg":"AUTH"}]}""");
            }
        }

        return next(context);
    }
}