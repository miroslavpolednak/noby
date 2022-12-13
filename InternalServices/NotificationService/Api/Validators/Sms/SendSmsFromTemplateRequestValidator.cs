using CIS.InternalServices.NotificationService.Contracts.Sms;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators.Sms;

public class SendSmsFromTemplateRequestValidator : AbstractValidator<SmsFromTemplateSendRequest>
{
    public SendSmsFromTemplateRequestValidator()
    {
        RuleFor(request => request.Phone)
            .NotNull()
                .WithErrorCode(ErrorCodes.SendSmsFromTemplate.PhoneRequired)
                .WithMessage($"{nameof(SmsFromTemplateSendRequest.Phone)} required.")
            .SetValidator(new PhoneValidator())
                .WithErrorCode(ErrorCodes.SendSmsFromTemplate.PhoneInvalid)
                .WithMessage(nameof(SmsFromTemplateSendRequest.Phone));
        
        RuleFor(request => request.ProcessingPriority)
            .GreaterThan(0)
                .WithErrorCode(ErrorCodes.SendSmsFromTemplate.ProcessPriorityInvalid)
                .WithMessage(nameof(SmsFromTemplateSendRequest.ProcessingPriority));
        
        RuleFor(request => request.Type)
            .NotEmpty()
                .WithErrorCode(ErrorCodes.SendSmsFromTemplate.TypeInvalid)
                .WithMessage($"Invalid {nameof(SmsFromTemplateSendRequest.Type)}.");

        RuleFor(request => request.Placeholders)
            .NotNull()
                .WithErrorCode(ErrorCodes.SendSmsFromTemplate.PlaceholdersRequired)
                .WithMessage($"{nameof(SmsFromTemplateSendRequest.Placeholders)} required.");
        
        // todo: validate placeholders
    }
}