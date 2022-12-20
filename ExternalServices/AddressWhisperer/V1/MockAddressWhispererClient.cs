using ExternalServices.AddressWhisperer.Dto;

namespace ExternalServices.AddressWhisperer.V1;

internal sealed class MockAddressWhispererClient
    : IAddressWhispererClient
{
    public Task<AddressDetail?> GetAddressDetail(string sessionId, string addressId, string title, string? country, CancellationToken cancellationToken)
    {
        return Task.FromResult<AddressDetail?>(new AddressDetail
        {
            Street = "Moje ulice",
            City = "Praha"
        });
    }

    public Task<List<Dto.FoundSuggestion>> GetSuggestions(string sessionId, string text, int pageSize, string? country, CancellationToken cancellationToken)
    {
        return Task.FromResult(new List<FoundSuggestion>
        {
            new FoundSuggestion { AddressId = "1", Title = "Moje ulice, Praha" }
        });
    }
}
