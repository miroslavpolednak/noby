namespace DomainServices.CustomerService.Api.Clients.CustomerManagement.V1;

public interface ICustomerManagementClient
{
    Task<CustomerBaseInfo> GetDetail(long customerId, string traceId, CancellationToken cancellationToken);
    Task<ICollection<CustomerBaseInfo>> GetList(IEnumerable<long> customerIds, string traceId, CancellationToken cancellationToken);
    Task<ICollection<CustomerSearchResultRow>> Search(CustomerManagementSearchRequest searchRequest, string traceId, CancellationToken cancellationToken);
}