using CIS.Core.Results;
using System.ServiceModel;
using DomainServices.CustomerService.Api.Clients.CustomerProfile.V1;

namespace DomainServices.CustomerService.Api.Clients.CustomerProfile;

public abstract class BaseClient
{
    private readonly HttpClient _httpClient;
    protected readonly ILogger _logger;

    protected BaseClient(HttpClient httpClient, ILogger logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    protected async Task<IServiceCallResult> CallEndpoint(Func<CustomerProfileWrapper, Task<IServiceCallResult>> func)
    {
        try
        {
            return await func(CreateClientWrapper());
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
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    private CustomerProfileWrapper CreateClientWrapper() => new(_httpClient);
}