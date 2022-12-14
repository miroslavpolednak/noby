using DomainServices.CustomerService.Api.Dto;
using FluentValidation;

namespace DomainServices.CustomerService.Api.Validators;

internal class CreateCustomerMediatrRequestValidator : AbstractValidator<CreateCustomerMediatrRequest>
{
    public CreateCustomerMediatrRequestValidator()
    {
        RuleFor(m => m.Request.Mandant).IsInEnum().NotEqual(Mandants.Unknown).WithMessage("Mandant must be not empty").WithErrorCode("11008");

        When(m => m.Request.Mandant == Mandants.Mp,
             () =>
             {
                 RuleForEach(m => m.Request.Identities).ChildRules(identity =>
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