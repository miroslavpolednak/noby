using ExternalServices.CustomerManagement.V1.CMWrapper;

namespace ExternalServices.CustomerManagement.V1;

internal sealed class RealCMClient : BaseClient<RealCMClient>, ICMClient
{
    public RealCMClient(HttpClient httpClient, ILogger<RealCMClient> logger) : base(httpClient, logger) { }

    public async Task<IServiceCallResult> Search(SearchCustomerRequest model)
    {
        _logger.LogDebug("Run inputs: CustomerManagement Search with data {contact}", System.Text.Json.JsonSerializer.Serialize(model));

        return await WithClient(async c => {

            return await callMethod(async () => {

                var result = await c.SearchCustomerAsync(model);

                return new SuccessfulServiceCallResult<CustomerSearchResult>(result);
            });

        });
    }

    private Client CreateClient()
        => new(_httpClient?.BaseAddress?.ToString(), _httpClient);

    private async Task<IServiceCallResult> WithClient(Func<Client, Task<IServiceCallResult>> fce)
    {
        try
        {
            return await fce(CreateClient());
        }
        catch (ApiException<Error> ex)
        {
            _logger.LogError(ex, ex.Message);
            return new SuccessfulServiceCallResult<ApiException<Error>>(ex);
        }
        catch (ApiException ex)
        {
            _logger.LogError(ex, ex.Message);
            return new SuccessfulServiceCallResult<ApiException>(ex);
        }
    }
}
