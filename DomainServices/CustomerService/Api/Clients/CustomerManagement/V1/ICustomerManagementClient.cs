namespace DomainServices.CustomerService.Api.Clients.CustomerManagement.V1;

public interface ICustomerManagementClient
{
    Task<CustomerBaseInfo> GetDetail(long customerId, string traceId, CancellationToken cancellationToken);
}