namespace DomainServices.CustomerService.Api.Clients.CustomerProfile.V1;

public class RealCustomerProfileClient : ICustomerProfileClient
{
    private readonly HttpClient _httpClient;

    public RealCustomerProfileClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}