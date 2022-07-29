using FluentValidation;

namespace FOMS.Api.Endpoints.CustomerObligation.CreateObligation;

internal class CreateObligationRequestValidator
    : AbstractValidator<CreateObligationRequest>
{
    public CreateObligationRequestValidator()
    {
        RuleFor(t => t.ObligationTypeId)
            .GreaterThan(0);

        RuleFor(t => t.CustomerOnSAId)
            .GreaterThan(0);
    }
}
