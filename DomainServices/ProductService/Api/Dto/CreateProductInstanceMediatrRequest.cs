using DomainServices.ProductService.Contracts;

namespace DomainServices.ProductService.Api.Dto;

internal sealed class CreateProductInstanceMediatrRequest
    : IRequest<CreateProductInstanceResponse>, CIS.Core.Validation.IValidatableRequest
{
    public long CaseId { get; init; }
    public int ProductInstanceTypeId { get; init; }

    public CreateProductInstanceMediatrRequest(CreateProductInstanceRequest request)
    {
        CaseId = request.CaseId;
        ProductInstanceTypeId = request.ProductInstanceTypeId;
    }
}
