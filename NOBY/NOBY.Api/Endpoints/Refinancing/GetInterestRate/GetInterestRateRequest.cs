namespace NOBY.Api.Endpoints.Refinancing.GetInterestRate;

internal sealed class GetInterestRateRequest
    : IRequest<GetInterestRateResponse>
{
    /// <summary>
    /// ID Case-u
    /// </summary>
    public long CaseId { get; init; }

    public GetInterestRateRequest(long caseId)
    {
        CaseId = caseId;
    }
}
