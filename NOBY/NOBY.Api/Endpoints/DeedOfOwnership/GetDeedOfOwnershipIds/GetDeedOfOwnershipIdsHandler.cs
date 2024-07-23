using ExternalServices.Crem.V1;

namespace NOBY.Api.Endpoints.DeedOfOwnership.GetDeedOfOwnershipIds;

internal sealed class GetDeedOfOwnershipIdsHandler(ICremClient _cremClient)
        : IRequestHandler<GetDeedOfOwnershipIdsRequest, DeedOfOwnershipGetDeedOfOwnershipIdsResponse>
{
    public async Task<DeedOfOwnershipGetDeedOfOwnershipIdsResponse> Handle(GetDeedOfOwnershipIdsRequest request, CancellationToken cancellationToken)
    {
        var response = await _cremClient.GetFlatsForAddress(request.AddressPointId, cancellationToken);

        return new DeedOfOwnershipGetDeedOfOwnershipIdsResponse
        {
            DeedOfOwnershipId = response.Building.IsknDeedOfOwnershipId,
            Flats = response
                .Building
                .Flats?
                .Select(t => new DeedOfOwnershipGetDeedOfOwnershipIdsFlat
                {
                    MannerOfUseFlatShortName = t.MannerOfUseFlatShortName,
                    DeedOfOwnershipId = t.IsknDeedOfOwnershipId,
                    FlatNumber = t.FlatNumber
                })
                .ToList()
        };
    }
}
