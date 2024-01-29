using System.Text.RegularExpressions;
using CIS.Infrastructure.CisMediatR.GrpcValidation;
using CIS.InternalServices.NotificationService.LegacyContracts.Common;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v1.Sms.Validators;

public class PhoneValidator : AbstractValidator<Phone?>
{
    public PhoneValidator()
    {
        When(phone => phone is not null, () =>
        {
            RuleFor(phone => phone!.CountryCode)
                .NotEmpty()
                    .WithErrorCode(ErrorCodeMapper.CountryCodeRequired)
                .Matches(new Regex(@"^((\+?[0-9]{1,3})|([0-9]{1,5}))$"))
                    .WithErrorCode(ErrorCodeMapper.CountryCodeInvalid);
        
            RuleFor(phone => phone!.NationalNumber)
                .NotEmpty()
                    .WithErrorCode(ErrorCodeMapper.NationalNumberRequired)
                .Matches(new Regex(@"^[0-9]{1,14}$"))
                    .WithErrorCode(ErrorCodeMapper.NationalNumberInvalid);
        });
    }
}