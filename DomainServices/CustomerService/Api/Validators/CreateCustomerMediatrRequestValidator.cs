using DomainServices.CustomerService.Api.Dto;
using FluentValidation;

namespace DomainServices.CustomerService.Api.Validators;

internal class CreateCustomerMediatrRequestValidator : AbstractValidator<CreateCustomerMediatrRequest>
{
    public CreateCustomerMediatrRequestValidator()
    {
        When(m => m.Request.Identities.Count == 2,
             () =>
             {
                 RuleForEach(m => m.Request.Identities).ChildRules(identity =>
                 {
                     SetIdentitySchemeValidator(identity);
                     SetIdentityCombinationValidator(identity, Identity.Types.IdentitySchemes.Kb, validator => validator.NotEmpty().GreaterThan(0));
                     SetIdentityCombinationValidator(identity, Identity.Types.IdentitySchemes.Mp, validator => validator.NotEmpty().GreaterThan(0));
                 });
             });

        When(m => m.Request.Identities.Count != 2,
             () =>
             {
                 RuleForEach(m => m.Request.Identities).ChildRules(identity =>
                 {
                     SetIdentitySchemeValidator(identity);

                     SetIdentityCombinationValidator(identity, Identity.Types.IdentitySchemes.Kb, validator => validator.Empty());
                     SetIdentityCombinationValidator(identity, Identity.Types.IdentitySchemes.Mp, validator => validator.NotEmpty().GreaterThan(0));
                 });
             });
    }

    private static void SetIdentitySchemeValidator(AbstractValidator<Identity> validator)
    {
        validator.RuleFor(i => i.IdentityScheme)
                 .IsInEnum()
                 .NotEqual(Identity.Types.IdentitySchemes.Unknown)
                 .WithMessage("IdentityScheme must be specified")
                 .WithErrorCode("11006");
    }

    private static void SetIdentityCombinationValidator(
        AbstractValidator<Identity> validator,
        Identity.Types.IdentitySchemes scheme,
        Func<IRuleBuilder<Identity, long>, IRuleBuilderOptions<Identity, long>> configureValidator)
    {
        validator.When(i => i.IdentityScheme == scheme,
                       () => configureValidator(validator.RuleFor(i => i.IdentityId))
                             .WithMessage("Unsupported combination of identifiers (identity schemas and identities).")
                             .WithErrorCode("11016"));
    }
}