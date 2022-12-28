using CIS.InternalServices.NotificationService.Contracts.Sms;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators.Sms;

public class SendSmsFromTemplateRequestValidator : AbstractValidator<SendSmsFromTemplateRequest>
{
    public SendSmsFromTemplateRequestValidator()
    {
        RuleFor(request => request.Phone)
            .NotNull()
                .WithErrorCode(ErrorCodes.SendSmsFromTemplate.PhoneRequired)
                .WithMessage($"{nameof(SendSmsFromTemplateRequest.Phone)} required.")
            .SetValidator(new PhoneValidator())
                .WithErrorCode(ErrorCodes.SendSmsFromTemplate.PhoneInvalid)
                .WithMessage(nameof(SendSmsFromTemplateRequest.Phone));
        
        RuleFor(request => request.ProcessingPriority)
            .GreaterThan(0)
                .WithErrorCode(ErrorCodes.SendSmsFromTemplate.ProcessPriorityInvalid)
                .WithMessage(nameof(SendSmsFromTemplateRequest.ProcessingPriority));
        
        RuleFor(request => request.Type)
            .NotEmpty()
                .WithErrorCode(ErrorCodes.SendSmsFromTemplate.TypeInvalid)
                .WithMessage($"Invalid {nameof(SendSmsFromTemplateRequest.Type)}.");

        RuleFor(request => request.Placeholders)
            .NotNull()
                .WithErrorCode(ErrorCodes.SendSmsFromTemplate.PlaceholdersRequired)
                .WithMessage($"{nameof(SendSmsFromTemplateRequest.Placeholders)} required.");
        
        // todo: validate placeholders
    }
}