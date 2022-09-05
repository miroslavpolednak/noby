namespace DomainServices.CustomerService.Api.Clients.CustomerManagement.V1;

public class MockCustomerManagementClient : ICustomerManagementClient
{
    public Task<CustomerBaseInfo> GetDetail(long customerId, string traceId, CancellationToken cancellationToken)
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

    public Task<ICollection<CustomerBaseInfo>> GetList(IEnumerable<long> customerIds, string traceId, CancellationToken cancellationToken)
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

        return Task.FromResult<ICollection<CustomerBaseInfo>>(result);
    }

    public Task<ICollection<CustomerSearchResultRow>> Search(CustomerManagementSearchRequest searchRequest, string traceId, CancellationToken cancellationToken)
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

        return Task.FromResult<ICollection<CustomerSearchResultRow>>(result);
    }
}