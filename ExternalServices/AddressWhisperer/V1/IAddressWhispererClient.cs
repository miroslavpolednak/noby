using ExternalServices.AddressWhisperer.Shared;

namespace ExternalServices.AddressWhisperer.V1;

public interface IAddressWhispererClient
{
    Task<IServiceCallResult> GetSuggestions(string sessionId, string text, int pageSize, int? countryid);

    Task<IServiceCallResult> GetAddressDetail(string sessionId, string addressId);
}
