using DomainServices.ProductService.Contracts;

namespace DomainServices.ProductService.Api.Dto;

internal sealed class GetHousingSavingsInstanceBasicDetailMediatrRequest
    : IRequest<GetHousingSavingsInstanceBasicDetailResponse>
{
    public long ProductInstanceId { get; init; }
    public GetHousingSavingsInstanceBasicDetailMediatrRequest(ProductInstanceIdRequest request)
    {
        ProductInstanceId = request.ProductInstanceId;
    }
}
