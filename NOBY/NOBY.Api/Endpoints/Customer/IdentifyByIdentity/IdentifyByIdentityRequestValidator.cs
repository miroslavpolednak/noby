using FluentValidation;

namespace NOBY.Api.Endpoints.Customer.IdentifyByIdentity;

public class IdentifyByIdentityRequestValidator
    : AbstractValidator<IdentifyByIdentityRequest>
{
    public IdentifyByIdentityRequestValidator()
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
