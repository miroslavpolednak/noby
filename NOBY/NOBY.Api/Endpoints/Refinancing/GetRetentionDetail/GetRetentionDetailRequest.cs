namespace NOBY.Api.Endpoints.Refinancing.GetRetentionDetail;

internal sealed record GetRetentionDetailRequest(long CaseId, long ProcessId)
    : IRequest<GetRetentionDetailResponse>
{
}
