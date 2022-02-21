using FluentValidation;

namespace DomainServices.ProductService.Api.Validators;

internal class GetMortgageRequestValidator : AbstractValidator<Dto.GetMortgageMediatrRequest>
{
    public GetMortgageRequestValidator()
    {
        RuleFor(t => t.Request.ProductId)
            .GreaterThan(0)
            .WithMessage("ProductId is not specified").WithErrorCode("99999"); //TODO: ErrorCode
    }
}

