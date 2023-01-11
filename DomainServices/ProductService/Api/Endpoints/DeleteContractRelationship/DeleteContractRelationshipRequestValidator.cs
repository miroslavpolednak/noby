using FluentValidation;

namespace DomainServices.ProductService.Api.Endpoints.DeleteContractRelationship;

internal sealed class DeleteContractRelationshipRequestValidator : AbstractValidator<Contracts.DeleteContractRelationshipRequest>
{
    public DeleteContractRelationshipRequestValidator()
    {
        RuleFor(t => t.ProductId)
           .GreaterThan(0)
           .WithMessage("ProductId is not specified").WithErrorCode("12014");

        RuleFor(t => t.PartnerId)
            .GreaterThan(0)
            .WithMessage("PartnerId is not specified").WithErrorCode("12010");
    }
}

