using FluentValidation;

namespace DomainServices.ProductService.Api.Endpoints.UpdateMortgage;

internal class UpdateMortgageRequestValidator : AbstractValidator<Contracts.UpdateMortgageRequest>
{
    public UpdateMortgageRequestValidator()
    {
        RuleFor(t => t.ProductId)
            .GreaterThan(0)
            .WithMessage("ProductId is not specified").WithErrorCode("12014");

        RuleFor(t => t.Mortgage.ProductTypeId)
            .GreaterThan(0)
            .WithMessage("ProductTypeId is not specified").WithErrorCode("12009");

        RuleFor(t => t.Mortgage.PartnerId)
            .GreaterThan(0)
            .WithMessage("PartnerId is not specified").WithErrorCode("12010");
    }
}

