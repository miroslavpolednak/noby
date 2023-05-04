using CIS.Infrastructure.CisMediatR.GrpcValidation;
using FluentValidation;

namespace DomainServices.ProductService.Api.Endpoints.DeleteContractRelationship;

internal sealed class DeleteContractRelationshipRequestValidator : AbstractValidator<Contracts.DeleteContractRelationshipRequest>
{
    public DeleteContractRelationshipRequestValidator()
    {
        RuleFor(t => t.ProductId)
           .GreaterThan(0)
           .WithErrorCode(ErrorCodeMapper.InvalidArgument12014);

        RuleFor(t => t.PartnerId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.InvalidArgument12010);
    }
}

