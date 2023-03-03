using DomainServices.OfferService.Contracts;
using FluentValidation;

namespace DomainServices.OfferService.Api.Endpoints.SimulateMortgage;

internal sealed class SimulateMortgageRequestValidator 
    : AbstractValidator<SimulateMortgageRequest>
{
    public SimulateMortgageRequestValidator()
    {
        RuleFor(t => t.ResourceProcessId)
           .Must((_, resourceProcessId) => Guid.TryParse(resourceProcessId, out Guid g))
           .WithErrorCode(ErrorCodeMapper.ResourceProcessIdIsEmpty);

        RuleFor(t => t.SimulationInputs)
            .Must(p => p != null)
            .WithErrorCode(ErrorCodeMapper.SimulationInputsIsEmpty);

        RuleFor(t => t.SimulationInputs.ProductTypeId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.ProductTypeIdIsEmpty);

        RuleFor(t => t.SimulationInputs.LoanKindId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.LoanKindIdIsEmpty);

        RuleFor(t => t.SimulationInputs.GuaranteeDateFrom)
           .Must(p => p != null && p >= DateTime.Today.AddDays(AppDefaults.MaxGuaranteeInDays * -1))
           .WithErrorCode(ErrorCodeMapper.GuaranteeDateFromTooOld);

        RuleFor(t => t.SimulationInputs.MarketingActions)
            .Must(p => p != null)
            .WithErrorCode(ErrorCodeMapper.MarketingActionsIsEmpty);

        RuleFor(t => t.SimulationInputs.FixedRatePeriod)
           .Must(p => p != null)
           .WithErrorCode(ErrorCodeMapper.FixedRatePeriodIsEmpty);

        RuleFor(t => t.SimulationInputs.LoanAmount)
           .Must(p => p != null)
           .WithErrorCode(ErrorCodeMapper.LoanAmountIsEmpty);

        RuleFor(t => t.SimulationInputs.LoanDuration)
           .Must(p => p != null)
           .WithErrorCode(ErrorCodeMapper.LoanDurationIsEmpty);

        RuleFor(t => t.SimulationInputs.CollateralAmount)
           .Must(p => p != null)
           .WithErrorCode(ErrorCodeMapper.CollateralAmountIsEmpty);

        When(t => t.BasicParameters?.GuaranteeDateTo is not null, () =>
        {
            RuleFor(t => t.BasicParameters.GuaranteeDateTo)
                .Must(t => t.Year == 0)
                .WithErrorCode(ErrorCodeMapper.GuaranteeDateToSet);
        });
    }
}
