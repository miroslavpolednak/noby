using ExternalServices.AddressWhisperer.Shared;

namespace ExternalServices.AddressWhisperer.V1;

internal sealed class MockAddressWhispererClient
    : IAddressWhispererClient
{
    public Task<IServiceCallResult> GetAddressDetail(string sessionId, string addressId)
    {
        return Task.FromResult<IServiceCallResult>(new SuccessfulServiceCallResult<AddressDetail>(new AddressDetail
        {
            Street = "Moje ulice",
            City = "Praha"
        }));
    }

    public Task<IServiceCallResult> GetSuggestions(string sessionId, string text, int pageSize, int? countryid)
    {
        return Task.FromResult<IServiceCallResult>(new SuccessfulServiceCallResult<List<FoundSuggestion>>(new List<FoundSuggestion>
        {
            new FoundSuggestion { AddressId = "1", Title = "Moje ulice, Praha" }
        }));
    }
}
