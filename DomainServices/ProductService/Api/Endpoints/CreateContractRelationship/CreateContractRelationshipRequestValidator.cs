using CIS.Infrastructure.CisMediatR.GrpcValidation;
using FluentValidation;

namespace DomainServices.ProductService.Api.Endpoints.CreateContractRelationship;

internal sealed class CreateContractRelationshipRequestValidator : AbstractValidator<Contracts.CreateContractRelationshipRequest>
{
    public CreateContractRelationshipRequestValidator()
    {
        RuleFor(t => t.ProductId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.InvalidArgument12014);

        RuleFor(t => t.Relationship)
            .Must((_, relationship) => relationship != null)
            .WithErrorCode(ErrorCodeMapper.InvalidArgument12015)
            
            .Must((_, relationship) => (relationship?.PartnerId ?? 0) > 0)
            .WithErrorCode(ErrorCodeMapper.InvalidArgument12016)
            
            .Must((_, relationship) => (relationship?.ContractRelationshipTypeId ?? 0) > 0)
            .WithErrorCode(ErrorCodeMapper.InvalidArgument12017);
    }
}

