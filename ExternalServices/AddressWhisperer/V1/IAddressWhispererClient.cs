using ExternalServices.AddressWhisperer.Shared;

namespace ExternalServices.AddressWhisperer.V1;

public interface IAddressWhispererClient
{
    List<FoundSuggestion>? GetSuggestions(string sessionId, string text, int pageSize, int? countryid);

    AddressDetail? GetAddressDetail(string sessionId, long addressId);
}
