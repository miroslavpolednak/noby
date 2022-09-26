using DomainServices.ProductService.Contracts;

namespace DomainServices.ProductService.Api.Dto;

internal record GetCustomersOnProductMediatrRequest(long ProductId)
    : IRequest<GetCustomersOnProductResponse>
{
}
