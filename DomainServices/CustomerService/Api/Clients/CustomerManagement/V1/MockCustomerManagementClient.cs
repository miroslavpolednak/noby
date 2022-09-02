namespace DomainServices.CustomerService.Api.Clients.CustomerManagement.V1;

public class MockCustomerManagementClient : ICustomerManagementClient
{
    public Task<CustomerBaseInfo> GetDetail(long customerId, string traceId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<CustomerBaseInfo>> GetList(IEnumerable<long> customerIds, string traceId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<CustomerSearchResultRow>> Search(CustomerManagementSearchRequest searchRequest, string traceId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}