namespace NOBY.Api.Endpoints.Cases.GetCaseParameters;

internal record GetCaseParametersRequest(long CaseId)
    : IRequest<GetCaseParametersResponse>
{
}
