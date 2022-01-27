using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api.Dto;

internal sealed class GetCaseCountsMediatrRequest
    : IRequest<GetCaseCountsResponse>
{
    public int CaseOwnerUserId { get; set; }

    public GetCaseCountsMediatrRequest(GetCaseCountsRequest request)
    {
        CaseOwnerUserId = request.CaseOwnerUserId;
    }
}
