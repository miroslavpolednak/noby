namespace CIS.Infrastructure.ExternalServicesHelpers.HttpHandlers;

/// <summary>
/// Middleware pro logování payloadu a hlavičke requestu a responsu.
/// </summary>
/// <remarks>
/// Vloží do kontextu logovaného záznamu klíče Payload a Headers s odpovídajícími objekty. Pokud např. response payload neobsahuje, není tento klíč do kontextu logovaného záznamu vložen.
/// </remarks>
internal sealed class LoggingHttpHandler 
    : DelegatingHandler
{
    private readonly ILogger _logger;

    public LoggingHttpHandler(ILoggerFactory loggerFactory)
    {
        if (loggerFactory == null)
            throw new ArgumentNullException(nameof(loggerFactory));

        _logger = loggerFactory.CreateLogger<LoggingHttpHandler>();
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Content is not null)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            using (_logger.BeginScope(new Dictionary<string, object>
            {
                { "Payload", await request.Content!.ReadAsStringAsync(cancellationToken) },
                { "Headers", request.Headers?.ToDictionary(x => x.Key, v => string.Join(';', v.Value)) }
            }))
            {
                _logger.HttpRequestPayload(request);
            }
#pragma warning restore CS8604 // Possible null reference argument.
        }
        else
        {
            _logger.HttpRequestStarted(request);
        }

        var response = await base.SendAsync(request, cancellationToken);

        int statusCode = (int)response.StatusCode;
        // logovat vsechen respones
        if (response?.Content is not null)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            using (_logger.BeginScope(new Dictionary<string, object>
            {
                { "Payload", await getRawResponse() },
                { "Headers", response.Headers?.ToDictionary(x => x.Key, v => string.Join(';', v.Value)) }
            }))
            {
                _logger.HttpResponsePayload(request, statusCode);
            }
#pragma warning restore CS8604 // Possible null reference argument.
        }

        return response!;

        async Task<string> getRawResponse()
            => response.Content == null ? "" : await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
    }
}

