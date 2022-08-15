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
            using (_logger.BeginScope(new Dictionary<string, object>
            {
                { "Payload", await request.Content!.ReadAsStringAsync(cancellationToken) }
            }))
            {
                _logger.HttpRequestPayload(request);
            }
        }

        try
        {
            var response = await base.SendAsync(request, cancellationToken);

            if (response!.IsSuccessStatusCode)
            {
                if (response?.Content is not null)
                {
                    using (_logger.BeginScope(new Dictionary<string, object>
                    {
                        { "Payload", await getRawResponse() }
                    }))
                    {
                        _logger.HttpResponsePayload(request);
                    }
                }

                return response!;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var message = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new CisExtServiceValidationException($"C4M error: {message}");

                // chyba spravne reportovana z c4m - bude to nekdy takto vypadat?
                /*var result = await response.Content.ReadFromJsonAsync<Dto.ErrorModel>(cancellationToken: cancellationToken)
                    ?? throw new CisExtServiceResponseDeserializationException(0, CreditWorthinessStartupExtensions.ServiceName, nameof(C4mHttpHandler), nameof(Dto.ErrorModel));
                
                throw new CisExtServiceValidationException(new List<(string Key, string Message)> 
                { 
                    (result.Code ?? "", result.Message ?? "") 
                }, 
                $"C4M error: Category: {result.Category}; Code: {result.Code};");*/
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
                throw new CisServiceUnavailableException(_serviceName, request.RequestUri!.ToString(), await getRawResponse());
            else if ((int)response.StatusCode >= 500)
                throw new CisServiceServerErrorException(_serviceName, request.RequestUri!.ToString(), await getRawResponse());
            else
                throw new CisServiceServerErrorException(_serviceName, request.RequestUri!.ToString(), $"{response.StatusCode}: {await getRawResponse()}");

            async Task<string> getRawResponse()
                => response.Content == null ? "" : await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (HttpRequestException ex)
        {
            throw new CisServiceUnavailableException(_serviceName, request.RequestUri!.ToString(), ex.Message);
        }
    }
}
