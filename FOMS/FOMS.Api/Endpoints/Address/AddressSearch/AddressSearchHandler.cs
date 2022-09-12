namespace FOMS.Api.Endpoints.Address.AddressSearch;

internal sealed class AddressSearchHandler
    : IRequestHandler<AddressSearchRequest, AddressSearchResponse>
{
    public async Task<AddressSearchResponse> Handle(AddressSearchRequest request, CancellationToken cancellationToken)
    {
        return new AddressSearchResponse
        {
            PageSize = 0
        };
    }

    public AddressSearchHandler()
    {

    }
}
