using CIS.InternalServices.NotificationService.Contracts.Sms;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators.Sms;

public class SendSmsRequestValidator : AbstractValidator<SmsSendRequest>
{
    public SendSmsRequestValidator()
    {
        RuleFor(request => request.Phone)
            .NotNull()
                .WithErrorCode(ErrorCodes.SendSms.PhoneRequired)
                .WithMessage($"{nameof(SmsSendRequest.Phone)} required.")
            .SetValidator(new PhoneValidator())
                .WithErrorCode(ErrorCodes.SendSms.PhoneInvalid)
                .WithMessage(nameof(SmsSendRequest.Phone));

        RuleFor(request => request.ProcessingPriority)
            .GreaterThan(0)
                .WithErrorCode(ErrorCodes.SendSms.ProcessPriorityInvalid)
                .WithMessage(nameof(SmsSendRequest.ProcessingPriority));

        RuleFor(request => request.Type)
            .NotEmpty()
                .WithErrorCode(ErrorCodes.SendSms.TypeInvalid)
                .WithMessage($"Invalid {nameof(SmsSendRequest.Type)}.");

        RuleFor(request => request.Text)
            .NotEmpty()
                .WithErrorCode(ErrorCodes.SendSms.TextRequired)
                .WithMessage($"{nameof(SmsSendRequest.Text)} required.")
            .MaximumLength(480)
                .WithErrorCode(ErrorCodes.SendSms.TextLengthLimitExceeded)
                .WithMessage($"Maximum length of {nameof(SmsSendRequest.Text)} is 480.");
    }    
}