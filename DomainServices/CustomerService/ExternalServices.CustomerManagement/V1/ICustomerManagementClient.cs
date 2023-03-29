using CIS.Infrastructure.ExternalServicesHelpers;

namespace DomainServices.CustomerService.ExternalServices.CustomerManagement.V1;

public interface ICustomerManagementClient
    : IExternalServiceClient
{
    Task<Contracts.CustomerBaseInfo> GetDetail(long customerId, CancellationToken cancellationToken = default(CancellationToken));
    
    Task<IReadOnlyList<Contracts.CustomerBaseInfo>> GetList(IEnumerable<long> customerIds, CancellationToken cancellationToken = default(CancellationToken));
    
    Task<IReadOnlyList<Contracts.CustomerSearchResultRow>> Search(Dto.CustomerManagementSearchRequest searchRequest, CancellationToken cancellationToken = default(CancellationToken));

    const string Version = "V1";
}