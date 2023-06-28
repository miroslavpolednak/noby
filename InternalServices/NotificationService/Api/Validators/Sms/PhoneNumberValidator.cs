using CIS.Infrastructure.CisMediatR.GrpcValidation;
using CIS.InternalServices.NotificationService.Api.Helpers;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators.Sms;

public class PhoneNumberValidator : AbstractValidator<string>
{
    public PhoneNumberValidator()
    {
        RuleFor(phoneNumber => phoneNumber.ParsePhone())
            .NotNull()
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.PhoneInvalid)
            .SetValidator(new PhoneValidator())
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.PhoneInvalid);
    }
}