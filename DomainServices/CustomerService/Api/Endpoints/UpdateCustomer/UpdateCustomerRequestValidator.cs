using DomainServices.CustomerService.Api.Validators;
using FluentValidation;

namespace DomainServices.CustomerService.Api.Endpoints.UpdateCustomer;

public class UpdateCustomerRequestValidator : AbstractValidator<UpdateCustomerRequest>
{
    public UpdateCustomerRequestValidator()
    {
        RuleFor(m => m.Mandant)
            .IsInEnum()
            .NotEqual(Mandants.Unknown)
            .WithMessage("Mandant must be not empty")
            .WithErrorCode("11008");

        When(m => m.Mandant == Mandants.Mp, () =>
        {
            RuleForEach(m => m.Identities).ChildRules(identity =>
            {
                identity
                    .When(i => i.IdentityScheme == Identity.Types.IdentitySchemes.Mp,() =>
                    {
                        identity.RuleFor(i => i.IdentityId)
                            .NotEmpty()
                            .GreaterThan(0)
                            .WithMessage("Unable to create customer in KonsDb without PartnerId.")
                            .WithErrorCode("11016");
                    })
                    .Otherwise(() =>
                    {
                        identity.RuleFor(i => i).SetValidator(new IdentityValidator());
                    });
            });
        });

        When(m => m.NaturalPerson is not null, () =>
        {
            RuleFor(m => m.NaturalPerson.IsPoliticallyExposed)
                .Must(t => !t.GetValueOrDefault())
                .WithMessage("Parametr isPoliticallyExposed je nastaven na true")
                .WithErrorCode("11027");

            RuleFor(m => m.NaturalPerson.IsUSPerson)
                .Must(t => !t.GetValueOrDefault())
                .WithMessage("Parametr isUSPerson je nastaven na true")
                .WithErrorCode("11028");
        });
    }
}