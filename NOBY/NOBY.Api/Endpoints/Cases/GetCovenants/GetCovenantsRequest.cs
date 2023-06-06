namespace NOBY.Api.Endpoints.Cases.GetCovenants;

internal sealed record GetCovenantsRequest(long CaseId)
    : IRequest<GetCovenantsResponse>
{
}
