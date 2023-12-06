using ExternalServices.RuianAddress.V1;
using NOBY.Api.Endpoints.Address.KatuzSearch.Dto;

namespace NOBY.Api.Endpoints.Address.KatuzSearch;

internal class KatuzSearchHandler : IRequestHandler<KatuzSearchRequest, KatuzSearchResponse>
{
    private readonly IRuianAddressClient _ruianAddressClient;

    public KatuzSearchHandler(IRuianAddressClient ruianAddressClient)
    {
        _ruianAddressClient = ruianAddressClient;
    }

    public async Task<KatuzSearchResponse> Handle(KatuzSearchRequest request, CancellationToken cancellationToken)
    {
        var result = await _ruianAddressClient.FindTerritory(request.SearchText, request.PageSize, cancellationToken);

        return new KatuzSearchResponse
        {
            PageSize = result.Count,
            Rows = result.Select(k => new KatuzLine
            {
                Id = k.Id,
                Title = k.Name ?? string.Empty
            }).ToList()
        };
    }
}