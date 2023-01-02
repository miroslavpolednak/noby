using CIS.Infrastructure.ExternalServicesHelpers;
using System.Collections.Immutable;

namespace DomainServices.CustomerService.ExternalServices.CustomerManagement.V1;

public interface ICustomerManagementClient
    : IExternalServiceClient
{
    Task<Contracts.CustomerBaseInfo> GetDetail(long customerId, CancellationToken cancellationToken = default(CancellationToken));
    
    Task<ImmutableList<Contracts.CustomerBaseInfo>> GetList(IEnumerable<long> customerIds, CancellationToken cancellationToken = default(CancellationToken));
    
    Task<ImmutableList<Contracts.CustomerSearchResultRow>> Search(Dto.CustomerManagementSearchRequest searchRequest, CancellationToken cancellationToken = default(CancellationToken));

    const string Version = "V1";
}