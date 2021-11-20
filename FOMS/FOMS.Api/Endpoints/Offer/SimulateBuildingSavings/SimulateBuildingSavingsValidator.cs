using CIS.Core.Validation;
using FluentValidation;

namespace FOMS.Api.Endpoints.Offer.Validators;

internal class SimulateBuildingSavingsValidator 
    : AbstractValidator<Dto.SimulateBuildingSavingsRequest>, IValidatableRequest
{
    public SimulateBuildingSavingsValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Jmeno je prazdne");

        RuleFor(x => x.BirthDate).NotNull().WithMessage("datum narozeni");

        RuleFor(x => x.ProductCode)
            .NotNull().WithMessage("kod prod")
            .GreaterThan(0).WithMessage("kod prod 2");

        RuleFor(x => x.ActionCode)
            .NotNull().WithMessage("kod akce")
            .GreaterThan(0).WithMessage("kod akce 2");
    }
}
