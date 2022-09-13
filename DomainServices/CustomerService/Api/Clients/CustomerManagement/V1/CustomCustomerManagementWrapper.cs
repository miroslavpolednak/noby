namespace DomainServices.CustomerService.Api.Clients.CustomerManagement.V1;

internal partial class CustomerManagementWrapper
{
    public CustomerManagementWrapper(string? baseUrl, HttpClient httpClient) : this(httpClient)
    {
        _baseUrl = baseUrl;
    }
}