using CIS.Core.Validation;
using FluentValidation;

namespace FOMS.Api.Endpoints.Offer.Validators;

internal class SimulateValidator 
    : AbstractValidator<Dto.SimulateRequest>, IValidatableRequest
{
    public SimulateValidator()
    {
        RuleFor(x => x.TargetAmount)
            .NotNull().WithMessage("Cílová částka není zadána")
            .InclusiveBetween(20000, 99999999).WithMessage("Cílová částka není správně zadána");

        RuleFor(x => x.ProductCode)
            .NotNull().WithMessage("Tarif není zadán")
            .GreaterThan(0).WithMessage("Tarif není zadán");

        RuleFor(x => x.ActionCode)
            .NotNull().WithMessage("Obchodní akce není zadána")
            .GreaterThanOrEqualTo(0).WithMessage("Obchodní akce není zadána");
    }
}
