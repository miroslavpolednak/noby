namespace NOBY.Api.Endpoints.Cases.GetCaseDocumentsFlag;

internal sealed record GetCaseMenuFlagsRequest(long CaseId)
    : IRequest<GetCaseMenuFlagsResponse>
{
}
