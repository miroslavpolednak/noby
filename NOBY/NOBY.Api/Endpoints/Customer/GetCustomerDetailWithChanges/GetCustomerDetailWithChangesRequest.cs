namespace NOBY.Api.Endpoints.Customer.GetCustomerDetailWithChanges;

internal sealed record GetCustomerDetailWithChangesRequest(int CustomerOnSAId)
    : IRequest<GetCustomerDetailWithChangesResponse>
{
}
