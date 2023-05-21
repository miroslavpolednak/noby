using CIS.Infrastructure.ExternalServicesHelpers;

namespace DomainServices.CustomerService.ExternalServices.CustomerManagement.V2;

public interface ICustomerManagementClient : IExternalServiceClient
{
    const string Version = "V2";

    Task<Contracts.CustomerInfo> GetDetail(long customerId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Contracts.CustomerInfo>> GetList(IEnumerable<long> customerIds, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Contracts.CustomerSearchResultRow>> Search(Dto.CustomerManagementSearchRequest searchRequest, CancellationToken cancellationToken = default);
}