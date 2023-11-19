using FluentValidation;

namespace DomainServices.ProductService.Api.Endpoints.GetMortgage;

internal sealed class GetMortgageRequestValidator : AbstractValidator<Contracts.GetMortgageRequest>
{
    public GetMortgageRequestValidator()
    {
        RuleFor(t => t.ProductId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.InvalidArgument12014);
    }
}

