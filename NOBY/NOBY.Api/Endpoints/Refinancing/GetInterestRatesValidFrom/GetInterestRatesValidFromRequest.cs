namespace NOBY.Api.Endpoints.Refinancing.GetInterestRatesValidFrom;

internal sealed class GetInterestRatesValidFromRequest
    : IRequest<GetInterestRatesValidFromResponse>
{
    /// <summary>
    /// ID Case-u
    /// </summary>
    public long CaseId { get; init; }

    public GetInterestRatesValidFromRequest(long caseId)
    {
        CaseId = caseId;
    }
}
