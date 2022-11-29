using FluentValidation;

namespace DomainServices.ProductService.Api.Validators;

internal class CreateMortgageRequestValidator : AbstractValidator<Dto.CreateMortgageMediatrRequest>
{
    public CreateMortgageRequestValidator()
    {
        RuleFor(t => t.Request.CaseId)
            .GreaterThan(0)
            .WithMessage("CaseId is not specified").WithErrorCode("12008");

        RuleFor(t => t.Request.Mortgage.ProductTypeId)
            .GreaterThan(0)
            .WithMessage("ProductTypeId is not specified").WithErrorCode("12009");

        RuleFor(t => t.Request.Mortgage.PartnerId)
            .GreaterThan(0)
            .WithMessage("PartnerId is not specified").WithErrorCode("12010");

        RuleFor(t => t.Request.Mortgage.LoanKindId)
            .GreaterThan(0)
            .WithMessage("LoanKindId is not specified").WithErrorCode("0");    }
}

