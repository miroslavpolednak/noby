using DomainServices.CustomerService.ExternalServices.CustomerManagement.Dto;
using DomainServices.CustomerService.ExternalServices.CustomerManagement.V2.Contracts;

namespace DomainServices.CustomerService.ExternalServices.CustomerManagement.V2;

internal class MockCustomerManagementClient : ICustomerManagementClient
{
    public Task<CustomerInfo> GetDetail(long customerId, CancellationToken cancellationToken = default)
    {
        var result = new CustomerInfo
        {
            CustomerId = 123,
            LifecycleStatusCode = LifecycleStatusEnum.ACTIVE,
            Party = new Party
            {
                NaturalPersonAttributes = new NaturalPersonAttributes
                {
                    BirthDate = DateTime.Now,
                    CzechBirthNumber = "0808081234",
                    FirstName = "Jméno",
                    Surname = "Příjmení"
                }
            }
        };

        return Task.FromResult(result);
    }

    public Task<IReadOnlyList<CustomerInfo>> GetList(IEnumerable<long> customerIds, CancellationToken cancellationToken = default)
    {
        var result = new List<CustomerInfo> {
            new() {
                CustomerId = 123,
                LifecycleStatusCode = LifecycleStatusEnum.ACTIVE,
                Party = new Party
                {
                    NaturalPersonAttributes = new NaturalPersonAttributes
                    {
                        BirthDate = DateTime.Now,
                        CzechBirthNumber = "0808081234",
                        FirstName = "Jméno",
                        Surname = "Příjmení"
                    }
                }
            }
        };

        return Task.FromResult<IReadOnlyList<CustomerInfo>>(result.AsReadOnly());
    }

    public Task<IReadOnlyList<CustomerSearchResultRow>> Search(CustomerManagementSearchRequest searchRequest, CancellationToken cancellationToken = default)
    {
        var result = new List<CustomerSearchResultRow>
        {
            new()
            {
                CustomerId = 123,
                LifecycleStatusCode = LifecycleStatusEnum.ACTIVE,
                Party = new PartySearchResult()
                {
                    NaturalPersonAttributes = new NaturalPersonAttributesSearch()
                    {
                        BirthDate = DateTime.Now,
                        CzechBirthNumber = "0808081234",
                        FirstName = "Jméno",
                        Surname = "Příjmení"
                    }
                }
            }
        };

        return Task.FromResult<IReadOnlyList<CustomerSearchResultRow>>(result.AsReadOnly());
    }
}