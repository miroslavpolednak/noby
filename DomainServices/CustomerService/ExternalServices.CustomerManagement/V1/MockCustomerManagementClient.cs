using DomainServices.CustomerService.ExternalServices.CustomerManagement.Dto;
using DomainServices.CustomerService.ExternalServices.CustomerManagement.V1.Contracts;

namespace DomainServices.CustomerService.ExternalServices.CustomerManagement.V1;

internal sealed class MockCustomerManagementClient 
    : ICustomerManagementClient
{
    public Task<CustomerBaseInfo> GetDetail(long customerId, CancellationToken cancellationToken)
    {
        var result = new CustomerBaseInfo()
        {
            CustomerId = 123,
            LifecycleStatusCode = LifecycleStatusEnum.ACTIVE,
            Party = new NaturalPerson
            {
                BirthDate = DateTime.Now,
                CzechBirthNumber = "0808081234",
                FirstName = "Jméno",
                Surname = "Příjmení"
            }
        };

        return Task.FromResult(result);
    }

    public Task<IReadOnlyList<CustomerBaseInfo>> GetList(IEnumerable<long> customerIds, CancellationToken cancellationToken)
    {
        var result = new List<CustomerBaseInfo>() {
            new() {
                CustomerId = 123,
                LifecycleStatusCode = LifecycleStatusEnum.ACTIVE,
                Party = new NaturalPerson {
                    BirthDate = DateTime.Now,
                    CzechBirthNumber = "0808081234",
                    FirstName = "Jméno",
                    Surname = "Příjmení"
                }
            }
        };

        return Task.FromResult<IReadOnlyList<CustomerBaseInfo>>(result.AsReadOnly());
    }

    public Task<IReadOnlyList<CustomerSearchResultRow>> Search(CustomerManagementSearchRequest searchRequest, CancellationToken cancellationToken)
    {
        var result = new List<CustomerSearchResultRow>()
        {
            new()
            {
                CustomerId = 123,
                LifecycleStatusCode = LifecycleStatusEnum.ACTIVE,
                Party = new NaturalPersonSearchResult
                {
                    BirthDate = DateTime.Now,
                    CzechBirthNumber = "0808081234",
                    FirstName = "Jméno",
                    Surname = "Příjmení",
                    LegalStatus = PartySearchResultLegalStatus.E
                }
            }
        };

        return Task.FromResult<IReadOnlyList<CustomerSearchResultRow>>(result.AsReadOnly());
    }
}