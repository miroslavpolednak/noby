using System.Diagnostics;

namespace CIS.Infrastructure.ExternalServicesHelpers.HttpHandlers;

/// <summary>
/// Přidá do requestu hlavičku s Correlation Id.
/// </summary>
public sealed class CorrelationIdForwardingHttpHandler
    : DelegatingHandler
{
    public const string DefaultHeaderKey = "X-Correlation-ID";

    private readonly string _headerKey;

    public CorrelationIdForwardingHttpHandler(string? headerKey = null)
    {
        _headerKey = headerKey ?? DefaultHeaderKey;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (Activity.Current?.Id is not null)
        {
            request.Headers.Add(_headerKey, Activity.Current?.Id);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
