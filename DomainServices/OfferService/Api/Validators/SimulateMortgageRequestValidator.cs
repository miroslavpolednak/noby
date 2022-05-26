using FluentValidation;

namespace DomainServices.OfferService.Api.Validators;

internal class SimulateMortgageRequestValidator : AbstractValidator<Dto.SimulateMortgageMediatrRequest>
{
    public SimulateMortgageRequestValidator()
    {
        RuleFor(t => t.Request.ResourceProcessId)
           .Must((_, resourceProcessId) => Guid.TryParse(resourceProcessId, out Guid g))
           .WithMessage("ResourceProcessId is missing or is in invalid format").WithErrorCode("10008");

        RuleFor(t => t.Request.BasicParameters)
            .Must(p => p != null)
            .WithMessage("BasicParameters are not specified").WithErrorCode("99999"); //TODO: ErrorCode

        RuleFor(t => t.Request.SimulationInputs)
            .Must(p => p != null)
            .WithMessage("SimulationInputs are not specified").WithErrorCode("99999"); //TODO: ErrorCode

        RuleFor(t => t.Request.SimulationInputs.ProductTypeId)
            .GreaterThan(0)
            .WithMessage("SimulationInputs.ProductTypeId is not specified").WithErrorCode("99999"); //TODO: ErrorCode
    }
}