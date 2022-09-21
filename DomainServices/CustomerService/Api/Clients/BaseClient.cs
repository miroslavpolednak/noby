namespace DomainServices.CustomerService.Api.Clients;

internal abstract class BaseClient<TClient>
{
    protected const string CallerSys = "{\"app\":\"DOMAIN_SERVICES\",\"appComp\":\"DOMAIN_SERVICES.CUSTOMER_SERVICE\"}";

    protected readonly HttpClient _httpClient;
    protected readonly ILogger _logger;

    protected BaseClient(HttpClient httpClient, ILogger logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    protected async Task<TResult> CallEndpoint<TResult>(Func<Task<TResult>> func)
    {
        try
        {
            return await func();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    protected abstract TClient CreateClient();
}