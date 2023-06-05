using System.Text.RegularExpressions;
using CIS.Infrastructure.CisMediatR.GrpcValidation;
using CIS.InternalServices.NotificationService.Contracts.Common;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators.Sms;

public class PhoneValidator : AbstractValidator<Phone?>
{
    public PhoneValidator()
    {
        When(phone => phone is not null, () =>
        {
            RuleFor(phone => phone!.CountryCode)
                .NotEmpty()
                    .WithErrorCode(ErrorHandling.ErrorCodeMapper.CountryCodeRequired)
                .Matches(new Regex(@"^((\+?[0-9]{1,3})|([0-9]{1,5}))$"))
                    .WithErrorCode(ErrorHandling.ErrorCodeMapper.CountryCodeInvalid);
        
            RuleFor(phone => phone!.NationalNumber)
                .NotEmpty()
                    .WithErrorCode(ErrorHandling.ErrorCodeMapper.NationalNumberRequired)
                .Matches(new Regex(@"^[0-9]{1,14}$"))
                    .WithErrorCode(ErrorHandling.ErrorCodeMapper.NationalNumberInvalid);
        });
    }
}