using CIS.InternalServices.NotificationService.Contracts.Sms;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators.Sms;

public class SendSmsRequestValidator : AbstractValidator<SendSmsRequest>
{
    public SendSmsRequestValidator()
    {
        RuleFor(request => request.Phone)
            .NotNull()
                .WithErrorCode(ErrorCodes.Validation.SendSms.PhoneRequired)
                .WithMessage($"{nameof(SendSmsRequest.Phone)} required.")
            .SetValidator(new PhoneValidator())
                .WithErrorCode(ErrorCodes.Validation.SendSms.PhoneInvalid)
                .WithMessage(nameof(SendSmsRequest.Phone));

        RuleFor(request => request.ProcessingPriority)
            .GreaterThan(0)
                .WithErrorCode(ErrorCodes.Validation.SendSms.ProcessPriorityInvalid)
                .WithMessage(nameof(SendSmsRequest.ProcessingPriority));

        RuleFor(request => request.Type)
            .NotEmpty()
                .WithErrorCode(ErrorCodes.Validation.SendSms.TypeInvalid)
                .WithMessage($"Invalid {nameof(SendSmsRequest.Type)}.");

        RuleFor(request => request.Text)
            .NotEmpty()
                .WithErrorCode(ErrorCodes.Validation.SendSms.TextRequired)
                .WithMessage($"{nameof(SendSmsRequest.Text)} required.")
            .MaximumLength(480)
                .WithErrorCode(ErrorCodes.Validation.SendSms.TextLengthLimitExceeded)
                .WithMessage($"Maximum length of {nameof(SendSmsRequest.Text)} is 480.");
    }    
}