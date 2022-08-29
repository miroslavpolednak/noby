namespace DomainServices.CustomerService.Api.Clients.CustomerManagement.V1;

public class RealCustomerManagementClient : ICustomerManagementClient
{
    private readonly HttpClient _httpClient;

    public RealCustomerManagementClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}