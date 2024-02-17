namespace NOBY.Api.Endpoints.Refinancing.GetRefinancingParameters;

public sealed record GetRefinancingParametersRequest(long CaseId)
    : IRequest<GetRefinancingParametersResponse>
{
}
