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
        
        RuleFor(t => t.ProductTypeId)
            .GreaterThan(0)
            .WithMessage("ProductTypeId must be > 0");
    }
}
