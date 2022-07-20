using FluentValidation;

namespace FOMS.Api.Endpoints.Customer.Identify;

public class IdentifyRequestValidator
    : AbstractValidator<IdentifyRequest>
{
    public IdentifyRequestValidator()
    {
        RuleFor(t => t.CustomerIdentity)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .ChildRules(x =>
            {
                x.RuleFor(t => t.Id)
                    .GreaterThan(0);
                x.RuleFor(t => t.Scheme)
                    .IsInEnum()
                    .Must(t => (int)t > 0);
            });
    }
}
