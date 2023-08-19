namespace NOBY.Api.Endpoints.DeedOfOwnership.GetDeedOfOwnershipIds;

internal sealed record GetDeedOfOwnershipIdsRequest(long AddressPointId)
    : IRequest<GetDeedOfOwnershipIdsResponse>
{
}
