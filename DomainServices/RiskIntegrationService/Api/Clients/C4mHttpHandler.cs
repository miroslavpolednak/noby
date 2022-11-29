namespace DomainServices.RiskIntegrationService.Api.Clients;

internal class C4mHttpHandler : DelegatingHandler
{
    private readonly ILogger _logger;
    private readonly string _serviceName;

    public C4mHttpHandler(HttpMessageHandler innerHandler, ILogger logger, string serviceName)
        : base(innerHandler)
    {
        _serviceName = serviceName;
        _logger = logger;
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

        HttpResponseMessage response;
        try
        {
            response = await base.SendAsync(request, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            throw new CisServiceUnavailableException(_serviceName, request.RequestUri!.ToString(), ex.Message);
        }

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

        if (response!.IsSuccessStatusCode)
        {
            return response!;
        }
        else if (statusCode >= 400 && statusCode < 500)
        {
            // chyba spravne reportovana z c4m - bude to nekdy takto vypadat?
            Dto.ErrorModel? result = null;
            try
            {
                result = await response.Content.ReadFromJsonAsync<Dto.ErrorModel>(cancellationToken: cancellationToken);
            }
            catch { }

            if (result is null) // nepodarilo se deserializovat na korektni response type
            {
                var message = await getRawResponse();
                throw new CisExtServiceValidationException($"C4M unknown error {statusCode}: {message}");
            }
            else
            {
                throw new CisExtServiceValidationException(new List<(string Key, string Message)>
                {
                    (result.Code ?? "", result.Message ?? "")
                },
                $"C4M error: HttpStatusCode: {statusCode}, Category: {result.Category}; Code: {result.Code};");
            }
        }
        else if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
            throw new CisServiceUnavailableException(_serviceName, request.RequestUri!.ToString(), await getRawResponse());
        else
            throw new CisServiceServerErrorException(_serviceName, request.RequestUri!.ToString(), $"{response.StatusCode}: {await getRawResponse()}");

        async Task<string> getRawResponse()
            => response.Content == null ? "" : await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
    }
}
