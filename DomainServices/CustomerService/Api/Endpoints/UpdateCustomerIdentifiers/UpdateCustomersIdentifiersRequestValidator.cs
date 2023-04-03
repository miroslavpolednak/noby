using FluentValidation;

namespace DomainServices.CustomerService.Api.Endpoints.UpdateCustomerIdentifiers;

internal sealed class UpdateCustomersIdentifiersRequestValidator : AbstractValidator<UpdateCustomerIdentifiersRequest>
{
    public UpdateCustomersIdentifiersRequestValidator()
    {
        RuleFor(m => m.Mandant).IsInEnum().NotEqual(Mandants.Unknown).WithMessage("Mandant must be not empty").WithErrorCode("11008");

        RuleFor(m => m.CustomerIdentities).Must(identities =>
        {
            var requiredSchemes = new[] { Identity.Types.IdentitySchemes.Mp, Identity.Types.IdentitySchemes.Kb };

            return identities.Select(c => c.IdentityScheme).Join(requiredSchemes, x => x, y => y, (x, _) => x).Count() >= 2;
        }).WithMessage("Request must contain both KB ID and MP ID");
    }
}