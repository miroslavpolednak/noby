using FluentValidation;

namespace DomainServices.ProductService.Api.Validators;

internal class UpdateMortgageRequestValidator : AbstractValidator<Dto.UpdateMortgageMediatrRequest>
{
    public UpdateMortgageRequestValidator()
    {
        RuleFor(t => t.Request.ProductId)
            .GreaterThan(0)
            .WithMessage("ProductId is not specified").WithErrorCode("99999"); //TODO: ErrorCode

        RuleFor(t => t.Request.Mortgage.ProductTypeId)
            .GreaterThan(0)
            .WithMessage("ProductTypeId is not specified").WithErrorCode("99999"); //TODO: ErrorCode

        RuleFor(t => t.Request.Mortgage.PartnerId)
            .GreaterThan(0)
            .WithMessage("PartnerId is not specified").WithErrorCode("99999"); //TODO: ErrorCode
    }
}

