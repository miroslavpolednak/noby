using FluentValidation;

namespace DomainServices.ProductService.Api.Endpoints.CreateMortgage;

internal sealed class CreateMortgageRequestValidator : AbstractValidator<Contracts.CreateMortgageRequest>
{
    public CreateMortgageRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithMessage("CaseId is not specified").WithErrorCode("12008");

        RuleFor(t => t.Mortgage.ProductTypeId)
            .GreaterThan(0)
            .WithMessage("ProductTypeId is not specified").WithErrorCode("12009");

        RuleFor(t => t.Mortgage.PartnerId)
            .GreaterThan(0)
            .WithMessage("PartnerId is not specified").WithErrorCode("12010");

        RuleFor(t => t.Mortgage.LoanKindId)
            .GreaterThan(0)
            .WithMessage("LoanKindId is not specified").WithErrorCode("0");
    }
}

