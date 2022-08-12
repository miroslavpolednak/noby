
namespace ExternalServices.CustomerManagement;

internal abstract class BaseClient<TClient>
    where TClient : class
{
    protected readonly HttpClient _httpClient;
    protected readonly ILogger<TClient> _logger;

    public BaseClient(HttpClient httpClient, ILogger<TClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    protected async Task<IServiceCallResult> callMethod(Func<Task<IServiceCallResult>> fce)
    {
        try
        {
            return await fce();
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError(e, e.Message);
            return new ErrorServiceCallResult(9000, $"CustomerManagement Endpoint '{_httpClient.BaseAddress}' unavailable");
        }
        catch (EndpointNotFoundException)
        {
            _logger.LogError("CustomerManagement Endpoint '{uri}' not found", _httpClient.BaseAddress);
            return new ErrorServiceCallResult(9001, $"CustomerManagement Endpoint '{_httpClient.BaseAddress}' not found");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
    }
}

