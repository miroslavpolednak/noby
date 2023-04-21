using CIS.InternalServices.NotificationService.Api.Helpers;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators.Sms;

public class PhoneNumberValidator : AbstractValidator<string>
{
    public PhoneNumberValidator()
    {
        RuleFor(phoneNumber => phoneNumber.ParsePhone())
            .NotEmpty()
            .SetValidator(new PhoneValidator());
    }
}