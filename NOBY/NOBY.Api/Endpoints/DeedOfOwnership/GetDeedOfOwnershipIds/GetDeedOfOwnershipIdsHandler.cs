using ExternalServices.Crem.V1;

namespace NOBY.Api.Endpoints.DeedOfOwnership.GetDeedOfOwnershipIds;

internal sealed class GetDeedOfOwnershipIdsHandler
    : IRequestHandler<GetDeedOfOwnershipIdsRequest, GetDeedOfOwnershipIdsResponse>
{
    public async Task<GetDeedOfOwnershipIdsResponse> Handle(GetDeedOfOwnershipIdsRequest request, CancellationToken cancellationToken)
    {
        var response = await _cremClient.GetFlatsForAddress(request.AddressPointId, cancellationToken);

        return new GetDeedOfOwnershipIdsResponse
        {
            DeedOfOwnershipId = response.Building.IsknDeedOfOwnershipId,
            Flats = response
                .Building
                .Flats
                .Select(t => new GetDeedOfOwnershipIdsResponseFlat
                {
                    DeedOfOwnershipId = t.IsknDeedOfOwnershipId,
                    FlatNumber = t.FlatNumber
                })
                .ToList()
        };
    }

    private readonly ICremClient _cremClient;

    public GetDeedOfOwnershipIdsHandler(ICremClient cremClient)
    {
        _cremClient = cremClient;
    }
}
