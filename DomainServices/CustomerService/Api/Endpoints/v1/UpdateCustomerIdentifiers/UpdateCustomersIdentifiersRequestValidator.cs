using CIS.Infrastructure.CisMediatR.GrpcValidation;
using FluentValidation;

namespace DomainServices.CustomerService.Api.Endpoints.v1.UpdateCustomerIdentifiers;

internal sealed class UpdateCustomersIdentifiersRequestValidator
    : AbstractValidator<UpdateCustomerIdentifiersRequest>
{
    public UpdateCustomersIdentifiersRequestValidator()
    {
        RuleFor(m => m.Mandant)
            .IsInEnum()
            .NotEqual(SharedTypes.GrpcTypes.Mandants.Unknown)
            .WithErrorCode(ErrorCodeMapper.MandantIsEmpty);

        RuleFor(m => m.CustomerIdentities).Must(identities =>
        {
            var requiredSchemes = new[] { SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes.Mp, SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes.Kb };

            return identities.Select(c => c.IdentityScheme).Join(requiredSchemes, x => x, y => y, (x, _) => x).Count() >= 2;
        })
            .WithErrorCode(ErrorCodeMapper.RequestMustContainBothIdentities);
    }
}