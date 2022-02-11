using DomainServices.ProductService.Contracts;

namespace DomainServices.ProductService.Api.Dto;

internal sealed class CreateContractRelationshipMediatrRequest
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>, CIS.Core.Validation.IValidatableRequest
{
    public CreateContractRelationshipRequest Request { get; init; }

    public CreateContractRelationshipMediatrRequest(CreateContractRelationshipRequest request)
    {
        Request = request;
    }
}