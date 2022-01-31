using FluentValidation;

namespace FOMS.Api.Endpoints.Offer.Validators;

internal class SimulateMortgageRequestValidator
     : AbstractValidator<Dto.SimulateMortgageRequest>
{
    public SimulateMortgageRequestValidator()
    {
        RuleFor(t => t.ResourceProcessId)
            .NotEmpty()
            .WithMessage("ResourceProcessId is empty");
    }
}
