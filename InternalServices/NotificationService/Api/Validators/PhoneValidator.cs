using System.Text.RegularExpressions;
using CIS.InternalServices.NotificationService.Contracts.Sms.Dto;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators;

public class PhoneValidator : AbstractValidator<Phone>
{
    public PhoneValidator()
    {
        RuleFor(phone => phone.CountryCode)
            .NotEmpty()
            .Matches(new Regex(@"^((\+?[0-9]{1,3})|([0-9]{1,5}))$"))
            .WithErrorCode(nameof(Phone.CountryCode));
        
        RuleFor(phone => phone.NationalNumber)
            .NotEmpty()
            .Matches(new Regex(@"^[0-9]{1,14}$"))
            .WithErrorCode(nameof(Phone.NationalNumber));
    }
}