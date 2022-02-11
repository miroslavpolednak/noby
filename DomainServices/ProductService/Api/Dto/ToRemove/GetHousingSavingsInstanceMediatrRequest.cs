using DomainServices.ProductService.Contracts;

namespace DomainServices.ProductService.Api.Dto;

internal class GetHousingSavingsInstanceMediatrRequest
    : IRequest<GetHousingSavingsInstanceResponse>
{
    public long ProductInstanceId { get; init; }
    public GetHousingSavingsInstanceMediatrRequest(ProductInstanceIdRequest request)
    {
        ProductInstanceId = request.ProductInstanceId;
    }
}
