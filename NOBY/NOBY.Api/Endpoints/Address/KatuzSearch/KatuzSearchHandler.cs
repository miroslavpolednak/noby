using ExternalServices.RuianAddress.V1;

namespace NOBY.Api.Endpoints.Address.KatuzSearch;

internal sealed class KatuzSearchHandler(IRuianAddressClient _ruianAddressClient)
        : IRequestHandler<AddressKatuzSearchRequest, AddressKatuzSearchResponse>
{
    public async Task<AddressKatuzSearchResponse> Handle(AddressKatuzSearchRequest request, CancellationToken cancellationToken)
    {
        var result = await _ruianAddressClient.FindTerritory(request.SearchText, request.PageSize, cancellationToken);

        return new AddressKatuzSearchResponse
        {
            PageSize = result.Count,
            Rows = result.Select(k => new AddressKatuzSearchKatuzLine
            {
                KatuzId = k.Id,
                KatuzTitle = k.Name ?? string.Empty
            }).OrderBy(k => k.KatuzTitle).ToList()
        };
    }
}