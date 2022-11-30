using CIS.InternalServices.NotificationService.Contracts.Sms;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators.Sms;

public class SendSmsFromTemplateRequestValidator : AbstractValidator<SmsFromTemplateSendRequest>
{
    public SendSmsFromTemplateRequestValidator()
    {
        RuleFor(request => request.Phone)
            .NotNull()
                .WithErrorCode("todo")
                .WithMessage("todo")
            .SetValidator(new PhoneValidator())
                .WithErrorCode(nameof(SmsFromTemplateSendRequest.Phone))
                .WithMessage("todo");

        RuleFor(request => request.ProcessingPriority)
            .GreaterThan(0)
                .WithErrorCode(nameof(SmsFromTemplateSendRequest.ProcessingPriority))
                .WithMessage("todo");
        
        RuleFor(request => request.Type)
            .NotEmpty()
                .WithErrorCode(nameof(SmsFromTemplateSendRequest.Type))
                .WithMessage("todo");
        
        RuleFor(request => request.Placeholders)
            .NotNull()
                .WithErrorCode(nameof(SmsFromTemplateSendRequest.Placeholders))
                .WithMessage("todo");
    }
}