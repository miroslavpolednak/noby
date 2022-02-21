using DomainServices.ProductService.Contracts;

namespace DomainServices.ProductService.Api.Dto;

internal sealed class DeleteContractRelationshipMediatrRequest
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>, CIS.Core.Validation.IValidatableRequest
{
    public DeleteContractRelationshipRequest Request { get; init; }

    public DeleteContractRelationshipMediatrRequest(DeleteContractRelationshipRequest request)
    {
        Request = request;
    }
}