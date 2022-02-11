using DomainServices.ProductService.Contracts;

namespace DomainServices.ProductService.Api.Dto;

internal class GetProductInstanceListMediatrRequest
    : IRequest<GetProductInstanceListResponse>
{
    public long CaseId { get; init; }
    public GetProductInstanceListMediatrRequest(GetProductInstanceListRequest request)
    {
        CaseId = request.CaseId;
    }
}
