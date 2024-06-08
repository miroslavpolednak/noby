using CIS.Infrastructure.CisMediatR.GrpcValidation;
using FluentValidation;

namespace DomainServices.CustomerService.Api.Endpoints.v1.ValidateContact;

internal sealed class ValidateContactRequestValidator
    : AbstractValidator<ValidateContactRequest>
{
    public ValidateContactRequestValidator()
    {
        RuleFor(request => request.Contact)
            .NotEmpty()
            .WithErrorCode(ErrorCodeMapper.ContactIsEmpty);

        RuleFor(request => request.ContactType)
            .NotEqual(ContactType.Unknown)
            .WithErrorCode(ErrorCodeMapper.ContactTypeIsEmpty)
            .IsInEnum()
            .WithErrorCode(ErrorCodeMapper.ContactTypeUnsupported);
    }
}