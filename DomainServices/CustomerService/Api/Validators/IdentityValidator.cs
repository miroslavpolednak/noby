using CIS.Infrastructure.CisMediatR.GrpcValidation;
using FluentValidation;

namespace DomainServices.CustomerService.Api.Validators;

internal sealed class IdentityValidator 
    : AbstractValidator<SharedTypes.GrpcTypes.Identity>
{
    public IdentityValidator()
    {
        RuleFor(t => t.IdentityId)
            .GreaterThan(0)
            .WithErrorCode(11005);

        RuleFor(t => t.IdentityScheme)
            .IsInEnum()
            .NotEqual(SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes.Unknown)
            .WithErrorCode(11006);
    }
}
