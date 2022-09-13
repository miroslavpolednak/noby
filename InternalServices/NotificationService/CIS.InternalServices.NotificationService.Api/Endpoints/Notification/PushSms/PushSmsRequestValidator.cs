using CIS.InternalServices.NotificationService.Contracts.Sms;
using CIS.InternalServices.NotificationService.Contracts.Sms.Dto;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.Notification.PushSms;

public class PushSmsRequestValidator : AbstractValidator<SmsPushRequest>
{
    public PushSmsRequestValidator()
    {
        RuleFor(request => request.Phone)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithErrorCode(nameof(SmsPushRequest.Phone))
            .ChildRules(phone =>
            {
                phone.RuleFor(p => p.CountryCode)
                    .NotEmpty()
                    .WithErrorCode($"{nameof(SmsPushRequest.Phone)}.{nameof(Phone.CountryCode)}");

                phone.RuleFor(p => p.NationalPhoneNumber)
                    .NotEmpty()
                    .WithErrorCode($"{nameof(SmsPushRequest.Phone)}.{nameof(Phone.NationalPhoneNumber)}");
            });

        RuleFor(request => request.ProcessingPriority)
            .GreaterThan(0)
            .WithErrorCode(nameof(SmsPushRequest.ProcessingPriority));
        
        RuleFor(request => request.Type)
            .NotEmpty()
            .WithErrorCode(nameof(SmsPushRequest.Type));
        
        RuleFor(request => request.Text)
            .NotEmpty()
            .WithErrorCode(nameof(SmsPushRequest.Text));
    }    
}