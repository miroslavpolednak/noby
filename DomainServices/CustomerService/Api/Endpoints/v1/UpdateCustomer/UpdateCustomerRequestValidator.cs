using CIS.Infrastructure.CisMediatR.GrpcValidation;
using DomainServices.CustomerService.Api.Validators;
using FluentValidation;

namespace DomainServices.CustomerService.Api.Endpoints.v1.UpdateCustomer;

internal sealed class UpdateCustomerRequestValidator
    : AbstractValidator<UpdateCustomerRequest>
{
    public UpdateCustomerRequestValidator()
    {
        RuleFor(m => m.Mandant)
            .IsInEnum()
            .NotEqual(SharedTypes.GrpcTypes.Mandants.Unknown)
            .WithErrorCode(ErrorCodeMapper.MandantIsEmpty);

        When(m => m.Mandant == SharedTypes.GrpcTypes.Mandants.Mp, () =>
        {
            RuleForEach(m => m.Identities).ChildRules(identity =>
            {
                identity
                    .When(i => i.IdentityScheme == SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes.Mp, () =>
                    {
                        identity.RuleFor(i => i.IdentityId)
                            .NotEmpty()
                            .GreaterThan(0)
                            .WithErrorCode(ErrorCodeMapper.MissingPartnerId);
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
                .WithErrorCode(ErrorCodeMapper.CantSetPEP);

            RuleFor(m => m.NaturalPerson.IsUSPerson)
                .Must(t => !t.GetValueOrDefault())
                .WithErrorCode(ErrorCodeMapper.CantSetUS);
        });
    }
}