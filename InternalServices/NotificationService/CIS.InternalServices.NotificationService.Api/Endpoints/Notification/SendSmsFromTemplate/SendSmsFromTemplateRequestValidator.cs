using CIS.InternalServices.NotificationService.Contracts.Sms;
using CIS.InternalServices.NotificationService.Contracts.Sms.Dto;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.Notification.SendSmsFromTemplate;

public class SendSmsFromTemplateRequestValidator : AbstractValidator<SmsFromTemplateSendRequest>
{
    public SendSmsFromTemplateRequestValidator()
    {
        RuleFor(request => request.Phone)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithErrorCode(nameof(SmsFromTemplateSendRequest.Phone))
            .ChildRules(phone =>
            {
                phone.RuleFor(p => p.CountryCode)
                    .NotEmpty()
                    .WithErrorCode($"{nameof(SmsFromTemplateSendRequest.Phone)}.{nameof(Phone.CountryCode)}");

                phone.RuleFor(p => p.NationalPhoneNumber)
                    .NotEmpty()
                    .WithErrorCode($"{nameof(SmsFromTemplateSendRequest.Phone)}.{nameof(Phone.NationalPhoneNumber)}");
            });

        RuleFor(request => request.ProcessingPriority)
            .GreaterThan(0)
            .WithErrorCode(nameof(SmsFromTemplateSendRequest.ProcessingPriority));
        
        RuleFor(request => request.Type)
            .NotEmpty()
            .WithErrorCode(nameof(SmsFromTemplateSendRequest.Type));
        
        RuleFor(request => request.Placeholders)
            .NotNull()
            .WithErrorCode(nameof(SmsFromTemplateSendRequest.Placeholders));
    }
}