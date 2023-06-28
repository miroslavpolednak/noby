namespace NOBY.Api.Endpoints.Cases.GetCovenant;

internal sealed record GetCovenantRequest(long CaseId, int CovenantOrder)
    : IRequest<GetCovenantResponse>
{
}
