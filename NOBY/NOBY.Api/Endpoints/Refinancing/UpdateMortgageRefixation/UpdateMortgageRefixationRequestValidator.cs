using FluentValidation;

namespace NOBY.Api.Endpoints.Refinancing.UpdateMortgageRefixation;

internal sealed class UpdateMortgageRefixationRequestValidator
    : AbstractValidator<UpdateMortgageRefixationRequest>
{
    public UpdateMortgageRefixationRequestValidator()
    {
        RuleFor(t => t.InterestRateDiscount)
            .NotEmpty()
            .GreaterThan(0)
            .When(t => t.HasInterestRateDiscount)
            .WithMessage("Interest rate discount is empty");
    }
}
