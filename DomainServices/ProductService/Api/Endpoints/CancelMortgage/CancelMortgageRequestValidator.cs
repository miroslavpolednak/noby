using FluentValidation;

namespace DomainServices.ProductService.Api.Endpoints.CancelMortgage;

internal sealed class CancelMortgageRequestValidator : AbstractValidator<CancelMortgageRequest>
{
    public CancelMortgageRequestValidator()
    {
        RuleFor(t => t.ProductId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.InvalidArgument12014);
    }
}