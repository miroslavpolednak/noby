namespace NOBY.Api.Endpoints.Cases.GetCaseParameters;

internal sealed record GetCaseParametersRequest(long CaseId)
    : IRequest<GetCaseParametersResponse>
{
}
