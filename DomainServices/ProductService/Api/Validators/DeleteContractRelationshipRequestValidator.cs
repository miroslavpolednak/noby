using FluentValidation;

namespace DomainServices.ProductService.Api.Validators;

internal class DeleteContractRelationshipRequestValidator : AbstractValidator<Dto.DeleteContractRelationshipMediatrRequest>
{
    public DeleteContractRelationshipRequestValidator()
    {
        RuleFor(t => t.Request.ProductId)
           .GreaterThan(0)
           .WithMessage("ProductId is not specified").WithErrorCode("12014");

        RuleFor(t => t.Request.PartnerId)
            .GreaterThan(0)
            .WithMessage("PartnerId is not specified").WithErrorCode("12010");
    }
}

