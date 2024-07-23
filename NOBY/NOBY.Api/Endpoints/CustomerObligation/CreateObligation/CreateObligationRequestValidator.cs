using FluentValidation;

namespace NOBY.Api.Endpoints.CustomerObligation.CreateObligation;

internal sealed class CreateObligationRequestValidator
    : AbstractValidator<CustomerObligationCreateObligationRequest>
{
    public CreateObligationRequestValidator()
    {
        RuleFor(t => t.ObligationTypeId)
            .GreaterThan(0);

        RuleFor(t => t.CustomerOnSAId)
            .GreaterThan(0);
    }
}
