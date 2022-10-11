using CIS.InternalServices.NotificationService.Contracts.Sms;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators;

public class SendSmsFromTemplateRequestValidator : AbstractValidator<SmsFromTemplateSendRequest>
{
    public SendSmsFromTemplateRequestValidator()
    {
        RuleFor(request => request.Phone)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .SetValidator(new PhoneValidator())
            .WithErrorCode(nameof(SmsFromTemplateSendRequest.Phone));

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