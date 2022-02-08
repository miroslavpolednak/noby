using ExternalServices.CustomerManagement.V1.CMWrapper;

namespace ExternalServices.CustomerManagement.V1;

internal sealed class MockCMClient : ICMClient
{
    public async Task<IServiceCallResult> Search(SearchCustomerRequest model)
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
