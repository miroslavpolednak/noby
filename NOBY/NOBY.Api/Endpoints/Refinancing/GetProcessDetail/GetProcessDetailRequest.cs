namespace NOBY.Api.Endpoints.Refinancing.GetProcessDetail;

internal sealed record GetProcessDetailRequest(long caseId, long processId): IRequest<GetProcessDetailResponse>;
