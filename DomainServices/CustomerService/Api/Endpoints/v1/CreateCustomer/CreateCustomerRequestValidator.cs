using CIS.Infrastructure.CisMediatR.GrpcValidation;
using DomainServices.CustomerService.Api.Validators;
using FluentValidation;

namespace DomainServices.CustomerService.Api.Endpoints.v1.CreateCustomer;

internal sealed class CreateCustomerRequestValidator
    : AbstractValidator<CreateCustomerRequest>
{
    public CreateCustomerRequestValidator()
    {
        RuleFor(m => m.Mandant)
            .IsInEnum()
            .NotEqual(SharedTypes.GrpcTypes.Mandants.Unknown)
            .WithErrorCode(ErrorCodeMapper.MandantIsEmpty);

        When(m => m.Mandant == SharedTypes.GrpcTypes.Mandants.Mp,
             () =>
             {
                 RuleForEach(m => m.Identities).ChildRules(identity =>
                 {
                     identity.When(i => i.IdentityScheme == SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes.Mp,
                                   () =>
                                   {
                                       identity.RuleFor(i => i.IdentityId)
                                               .NotEmpty()
                                               .GreaterThan(0)
                                               .WithErrorCode(11016);
                                   })
                             .Otherwise(() =>
                             {
                                 identity.RuleFor(i => i).SetValidator(new IdentityValidator());
                             });
                 });
             });
    }
}