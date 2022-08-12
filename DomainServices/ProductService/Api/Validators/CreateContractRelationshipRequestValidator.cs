using FluentValidation;
using DomainServices.ProductService.Contracts;

namespace DomainServices.ProductService.Api.Validators;

internal class CreateContractRelationshipRequestValidator : AbstractValidator<Dto.CreateContractRelationshipMediatrRequest>
{
    public CreateContractRelationshipRequestValidator()
    {
        RuleFor(t => t.Request.ProductId)
            .GreaterThan(0)
            .WithMessage("ProductId is not specified").WithErrorCode("12014");

        RuleFor(t => t.Request.Relationship)
            .Must((_, relationship) => relationship != null)
            .WithMessage("Relationship not provided").WithErrorCode("12015")
            .Must((_, relationship) => (relationship?.PartnerId ?? 0) > 0)
            .WithMessage("Relationship PartnerId not specified").WithErrorCode("12016")
             .Must((_, relationship) => (relationship?.ContractRelationshipTypeId ?? 0) > 0)
            .WithMessage("Relationship ContractRelationshipTypeId not specified").WithErrorCode("12017");
    }
}

