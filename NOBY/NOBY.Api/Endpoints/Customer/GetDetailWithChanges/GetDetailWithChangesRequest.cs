namespace NOBY.Api.Endpoints.Customer.GetDetailWithChanges;

internal record GetDetailWithChangesRequest(int CustomerOnSAId)
    : IRequest<GetDetailWithChangesResponse>
{
}
