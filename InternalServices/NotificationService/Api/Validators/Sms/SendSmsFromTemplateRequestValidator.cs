using CIS.InternalServices.NotificationService.Contracts.Sms;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators.Sms;

public class SendSmsFromTemplateRequestValidator : AbstractValidator<SendSmsFromTemplateRequest>
{
    public SendSmsFromTemplateRequestValidator()
    {
        RuleFor(request => request.PhoneNumber)
            .NotNull()
                .WithErrorCode(ErrorCodes.Validation.SendSmsFromTemplate.PhoneNumberRequired)
                .WithMessage($"{nameof(SendSmsFromTemplateRequest.PhoneNumber)} required.")
            .SetValidator(new PhoneNumberValidator())
                .WithErrorCode(ErrorCodes.Validation.SendSmsFromTemplate.PhoneNumberInvalid)
                .WithMessage($"Invalid {nameof(SendSmsRequest.PhoneNumber)}.");
        
        RuleFor(request => request.ProcessingPriority)
            .GreaterThan(0)
                .WithErrorCode(ErrorCodes.Validation.SendSmsFromTemplate.ProcessPriorityInvalid)
                .WithMessage($"Invalid {nameof(SendSmsRequest.ProcessingPriority)}.");
        
        RuleFor(request => request.Type)
            .NotEmpty()
                .WithErrorCode(ErrorCodes.Validation.SendSmsFromTemplate.TypeInvalid)
                .WithMessage($"Invalid {nameof(SendSmsFromTemplateRequest.Type)}.");

        RuleFor(request => request.Placeholders)
            .NotNull()
                .WithErrorCode(ErrorCodes.Validation.SendSmsFromTemplate.PlaceholdersRequired)
                .WithMessage($"{nameof(SendSmsFromTemplateRequest.Placeholders)} required.")
            .Must(placeholders =>
            {
                return placeholders.Select(p => p.Value).All(p => p.Length > 0);
            })
                .WithErrorCode(ErrorCodes.Validation.SendSmsFromTemplate.PlaceholdersInvalid)
                .WithMessage($"{nameof(SendSmsFromTemplateRequest.Placeholders)} must contain non-empty values.")
            .Must(placeholders =>
            {
                var totalCount = placeholders.Count;
                var uniqueCount = placeholders.Select(p => p.Key).Distinct().Count();

                return totalCount == uniqueCount;
            })
                .WithErrorCode(ErrorCodes.Validation.SendSmsFromTemplate.PlaceholdersInvalid)
                .WithMessage($"{nameof(SendSmsFromTemplateRequest.Placeholders)} must contain unique keys.");
    }
}