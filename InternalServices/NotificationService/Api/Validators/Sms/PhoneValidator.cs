using System.Text.RegularExpressions;
using CIS.InternalServices.NotificationService.Contracts.Common;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators.Sms;

public class PhoneValidator : AbstractValidator<Phone>
{
    public PhoneValidator()
    {
        RuleFor(phone => phone.CountryCode)
            .NotEmpty()
                .WithErrorCode(ErrorCodes.Phone.CountryCodeRequired)
                .WithMessage($"{nameof(Phone.CountryCode)} required.")
            .Matches(new Regex(@"^((\+?[0-9]{1,3})|([0-9]{1,5}))$"))
                .WithErrorCode(ErrorCodes.Phone.CountryCodeInvalid)
                .WithMessage($"Invalid {nameof(Phone.CountryCode)}.");
        
        RuleFor(phone => phone.NationalNumber)
            .NotEmpty()
                .WithErrorCode(ErrorCodes.Phone.NationalNumberRequired)
                .WithMessage($"{nameof(Phone.NationalNumber)} required.")
            .Matches(new Regex(@"^[0-9]{1,14}$"))
                .WithErrorCode(ErrorCodes.Phone.NationalNumberInvalid)
                .WithMessage($"Invalid {nameof(Phone.NationalNumber)}.");
    }
}