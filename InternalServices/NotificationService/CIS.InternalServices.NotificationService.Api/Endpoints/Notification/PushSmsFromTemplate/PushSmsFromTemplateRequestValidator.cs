using CIS.InternalServices.NotificationService.Contracts.Sms;
using CIS.InternalServices.NotificationService.Contracts.Sms.Dto;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.Notification.PushSmsFromTemplate;

public class PushSmsFromTemplateRequestValidator : AbstractValidator<SmsFromTemplatePushRequest>
{
    public PushSmsFromTemplateRequestValidator()
    {
        RuleFor(request => request.Phone)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithErrorCode(nameof(SmsFromTemplatePushRequest.Phone))
            .ChildRules(phone =>
            {
                phone.RuleFor(p => p.CountryCode)
                    .NotEmpty()
                    .WithErrorCode($"{nameof(SmsFromTemplatePushRequest.Phone)}.{nameof(Phone.CountryCode)}");

                phone.RuleFor(p => p.NationalPhoneNumber)
                    .NotEmpty()
                    .WithErrorCode($"{nameof(SmsFromTemplatePushRequest.Phone)}.{nameof(Phone.NationalPhoneNumber)}");
            });

        RuleFor(request => request.ProcessingPriority)
            .GreaterThan(0)
            .WithErrorCode(nameof(SmsFromTemplatePushRequest.ProcessingPriority));
        
        RuleFor(request => request.Type)
            .NotEmpty()
            .WithErrorCode(nameof(SmsFromTemplatePushRequest.Type));
        
        RuleFor(request => request.Placeholders)
            .NotEmpty()
            .WithErrorCode(nameof(SmsFromTemplatePushRequest.Placeholders));
    }
}