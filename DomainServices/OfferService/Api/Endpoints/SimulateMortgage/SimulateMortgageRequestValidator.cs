using DomainServices.OfferService.Contracts;
using FluentValidation;

namespace DomainServices.OfferService.Api.Endpoints.SimulateMortgage;

internal class SimulateMortgageRequestValidator 
    : AbstractValidator<SimulateMortgageRequest>
{
    public SimulateMortgageRequestValidator()
    {
        RuleFor(t => t.ResourceProcessId)
           .Must((_, resourceProcessId) => Guid.TryParse(resourceProcessId, out Guid g))
           .WithMessage("ResourceProcessId is missing or is in invalid format").WithErrorCode("10003");

        RuleFor(t => t.SimulationInputs)
            .Must(p => p != null)
            .WithMessage("SimulationInputs are not specified").WithErrorCode("10004");

        RuleFor(t => t.SimulationInputs.ProductTypeId)
            .GreaterThan(0)
            .WithMessage("SimulationInputs.ProductTypeId is not specified").WithErrorCode("10005");

        RuleFor(t => t.SimulationInputs.LoanKindId)
            .GreaterThan(0)
            .WithMessage("SimulationInputs.LoanKindId is not specified").WithErrorCode("10006");

        RuleFor(t => t.SimulationInputs.GuaranteeDateFrom)
           .Must(p => p != null && p >= DateTime.Today.AddDays(AppDefaults.MaxGuaranteeInDays * -1))
           .WithMessage($"SimulationInputs.GuaranteeDateFrom can't be older then {AppDefaults.MaxGuaranteeInDays} days").WithErrorCode("10007");

        RuleFor(t => t.SimulationInputs.MarketingActions)
            .Must(p => p != null)
            .WithMessage("SimulationInputs.MarketingActions are not specified").WithErrorCode("10008");

        RuleFor(t => t.SimulationInputs.FixedRatePeriod)
           .Must(p => p != null)
           .WithMessage("SimulationInputs.FixedRatePeriod is not specified").WithErrorCode("10009");

        RuleFor(t => t.SimulationInputs.LoanAmount)
           .Must(p => p != null)
           .WithMessage("SimulationInputs.LoanAmount is not specified").WithErrorCode("10010");

        RuleFor(t => t.SimulationInputs.LoanDuration)
           .Must(p => p != null)
           .WithMessage("SimulationInputs.LoanDuration is not specified").WithErrorCode("10011");

        RuleFor(t => t.SimulationInputs.CollateralAmount)
           .Must(p => p != null)
           .WithMessage("SimulationInputs.CollateralAmount is not specified").WithErrorCode("10018");

        When(t => t.BasicParameters?.GuaranteeDateTo is not null, () =>
        {
            RuleFor(t => t.BasicParameters.GuaranteeDateTo)
                .Must(t => t.Year == 0)
                .WithMessage("BasicParameters.GuaranteeDateTo is auto generated parameter - can't be set by consumer").WithErrorCode("10019");
        });
    }
}
