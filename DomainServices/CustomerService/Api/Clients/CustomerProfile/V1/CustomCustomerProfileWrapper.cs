namespace DomainServices.CustomerService.Api.Clients.CustomerProfile.V1;

internal partial class CustomerProfileWrapper
{
    public CustomerProfileWrapper(string? baseUrl, HttpClient httpClient) : this(httpClient)
    {
        _baseUrl = baseUrl;
    }
}