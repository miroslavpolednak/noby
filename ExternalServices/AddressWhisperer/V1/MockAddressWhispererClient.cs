using ExternalServices.AddressWhisperer.Shared;

namespace ExternalServices.AddressWhisperer.V1;

internal sealed class MockAddressWhispererClient
    : IAddressWhispererClient
{
    public AddressDetail? GetAddressDetail(string sessionId, long addressId)
    {
        throw new NotImplementedException();
    }

    public List<FoundSuggestion>? GetSuggestions(string sessionId, string text, int pageSize, int? countryid)
    {
        throw new NotImplementedException();
    }
}
