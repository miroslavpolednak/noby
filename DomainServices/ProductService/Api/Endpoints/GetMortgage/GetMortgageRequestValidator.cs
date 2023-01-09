using FluentValidation;

namespace DomainServices.ProductService.Api.Endpoints.GetMortgage;

internal class GetMortgageRequestValidator : AbstractValidator<Contracts.GetMortgageRequest>
{
    public GetMortgageRequestValidator()
    {
        RuleFor(t => t.ProductId)
            .GreaterThan(0)
            .WithMessage("ProductId is not specified").WithErrorCode("12014");
    }
}

