using ExternalServices.CustomerManagement.V1.CMWrapper;

namespace ExternalServices.CustomerManagement.V1;

internal sealed class MockCMClient : ICMClient
{
    public async Task<IServiceCallResult> GetDetail(long model, CancellationToken cancellationToken)
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

        return await Task.FromResult(new SuccessfulServiceCallResult<CustomerBaseInfo>(result));
    }

    public async Task<IServiceCallResult> GetList(IEnumerable<long> model, CancellationToken cancellationToken)
    {
        var result = new List<CustomerBaseInfo>() {
            new CustomerBaseInfo() {
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

        return await Task.FromResult(new SuccessfulServiceCallResult<IEnumerable<CustomerBaseInfo>>(result));
    }

    public async Task<IServiceCallResult> Search(SearchCustomerRequest model, CancellationToken cancellationToken)
    {
        var result = new CustomerSearchResult()
        {
            IsThereMore = true,
            ResultRows = new List<CustomerSearchResultRow>() {
                    new CustomerSearchResultRow {
                        CustomerId = 123,
                        LifecycleStatusCode = LifecycleStatusEnum.ACTIVE,
                        Party = new NaturalPersonSearchResult {
                            BirthDate = DateTime.Now,
                            CzechBirthNumber = "0808081234",
                            FirstName = "Jméno",
                            Surname = "Příjmení",
                            LegalStatus = PartySearchResultLegalStatus.E
                        }
                    }
                }
        };

        return await Task.FromResult(new SuccessfulServiceCallResult<CustomerSearchResult>(result));
    }
}
