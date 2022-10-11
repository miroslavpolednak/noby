using CIS.InternalServices.NotificationService.Contracts.Sms;
using CIS.InternalServices.NotificationService.Contracts.Sms.Dto;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators;

public class SendSmsRequestValidator : AbstractValidator<SmsSendRequest>
{
    public SendSmsRequestValidator()
    {
        RuleFor(request => request.Phone)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithErrorCode(nameof(SmsSendRequest.Phone))
            .ChildRules(phone =>
            {
                phone.RuleFor(p => p.CountryCode)
                    .NotEmpty()
                    .WithErrorCode($"{nameof(SmsSendRequest.Phone)}.{nameof(Phone.CountryCode)}");

                phone.RuleFor(p => p.NationalNumber)
                    .NotEmpty()
                    .WithErrorCode($"{nameof(SmsSendRequest.Phone)}.{nameof(Phone.NationalNumber)}");
            });

        RuleFor(request => request.ProcessingPriority)
            .GreaterThan(0)
            .WithErrorCode(nameof(SmsSendRequest.ProcessingPriority));
        
        RuleFor(request => request.Type)
            .NotEmpty()
            .WithErrorCode(nameof(SmsSendRequest.Type));
        
        RuleFor(request => request.Text)
            .NotEmpty()
            .WithErrorCode(nameof(SmsSendRequest.Text));
    }    
}