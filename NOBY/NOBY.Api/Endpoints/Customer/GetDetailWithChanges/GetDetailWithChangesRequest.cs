namespace NOBY.Api.Endpoints.Customer.GetDetailWithChanges;

internal sealed record GetDetailWithChangesRequest(int CustomerOnSAId)
    : IRequest<GetDetailWithChangesResponse>
{
}
