using CIS.InternalServices.NotificationService.Contracts.Sms.Dto;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators;

public class PhoneValidator : AbstractValidator<Phone>
{
    public PhoneValidator()
    {
        // todo: Standard E164
        RuleFor(phone => phone.CountryCode)
            .NotEmpty()
            .MaximumLength(5)
            .WithErrorCode(nameof(Phone.CountryCode));
        
        RuleFor(phone => phone.NationalNumber)
            .NotEmpty()
            .MaximumLength(14)
            .WithErrorCode(nameof(Phone.NationalNumber));
    }
}