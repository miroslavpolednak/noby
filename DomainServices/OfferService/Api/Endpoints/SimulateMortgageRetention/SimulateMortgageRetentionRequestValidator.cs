using DomainServices.OfferService.Contracts;
using FluentValidation;

namespace DomainServices.OfferService.Api.Endpoints.SimulateMortgageRetention;

internal sealed class SimulateMortgageRetentionRequestValidator
    : AbstractValidator<SimulateMortgageRetentionRequest>
{
    public SimulateMortgageRetentionRequestValidator()
    {
        RuleFor(t => t.SimulationInputs)
            .Must(p => p != null)
            .WithErrorCode(ErrorCodeMapper.SimulationInputsIsEmpty);

        RuleFor(t => t.BasicParameters)
            .Must(p => p != null)
            .WithErrorCode(ErrorCodeMapper.SimulationInputsIsEmpty);

        When(t => t.BasicParameters is not null, () =>
        {
            RuleFor(t => (decimal)t.BasicParameters.Amount)
                .GreaterThanOrEqualTo(0)
                .WithErrorCode(ErrorCodeMapper.MortgageRetentionAmountNotValid);

            RuleFor(t => (decimal)t.BasicParameters.AmountDiscount!)
                .GreaterThanOrEqualTo(0)
                .When(t => t.BasicParameters.AmountDiscount != null)
                .WithErrorCode(ErrorCodeMapper.MortgageRetentionAmountIndividualPriceNotValid);
        });
    }
}
