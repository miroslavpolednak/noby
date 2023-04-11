using DomainServices.CustomerService.Api.Validators;
using FluentValidation;

namespace DomainServices.CustomerService.Api.Endpoints.CreateCustomer;

internal sealed class CreateCustomerRequestValidator : AbstractValidator<CreateCustomerRequest>
{
    public CreateCustomerRequestValidator()
    {
        RuleFor(m => m.Mandant).IsInEnum().NotEqual(Mandants.Unknown).WithMessage("Mandant must be not empty").WithErrorCode("11008");

        When(m => m.Mandant == Mandants.Mp,
             () =>
             {
                 RuleForEach(m => m.Identities).ChildRules(identity =>
                 {
                     identity.When(i => i.IdentityScheme == Identity.Types.IdentitySchemes.Mp,
                                   () =>
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
    }
}