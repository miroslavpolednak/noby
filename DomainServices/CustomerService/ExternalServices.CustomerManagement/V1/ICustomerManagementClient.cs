using CIS.Infrastructure.ExternalServicesHelpers;

namespace DomainServices.CustomerService.ExternalServices.CustomerManagement.V1;

public interface ICustomerManagementClient
    : IExternalServiceClient
{
    Task<Contracts.CustomerBaseInfo> GetDetail(long customerId, string traceId, CancellationToken cancellationToken = default(CancellationToken));
    
    Task<ICollection<Contracts.CustomerBaseInfo>> GetList(IEnumerable<long> customerIds, string traceId, CancellationToken cancellationToken = default(CancellationToken));
    
    Task<ICollection<Contracts.CustomerSearchResultRow>> Search(Dto.CustomerManagementSearchRequest searchRequest, string traceId, CancellationToken cancellationToken = default(CancellationToken));

    const string Version = "V1";
}