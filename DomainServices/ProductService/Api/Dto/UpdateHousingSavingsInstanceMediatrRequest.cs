using DomainServices.ProductService.Contracts;

namespace DomainServices.ProductService.Api.Dto;

internal sealed class UpdateHousingSavingsInstanceMediatrRequest
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>
{
    public long ProductInstanceId { get; init; }
    public UpdateHousingSavingsInstanceMediatrRequest(ProductInstanceIdRequest request)
    {
        ProductInstanceId = request.ProductInstanceId;
    }
}
