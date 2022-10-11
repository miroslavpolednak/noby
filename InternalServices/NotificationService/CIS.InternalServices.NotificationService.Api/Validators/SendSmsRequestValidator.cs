using CIS.InternalServices.NotificationService.Contracts.Sms;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators;

public class SendSmsRequestValidator : AbstractValidator<SmsSendRequest>
{
    public SendSmsRequestValidator()
    {
        RuleFor(request => request.Phone)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .SetValidator(new PhoneValidator())
            .WithErrorCode(nameof(SmsSendRequest.Phone));

        RuleFor(request => request.ProcessingPriority)
            .GreaterThan(0)
            .WithErrorCode(nameof(SmsSendRequest.ProcessingPriority));
        
        RuleFor(request => request.Type)
            .NotEmpty()
            .WithErrorCode(nameof(SmsSendRequest.Type));
        
        RuleFor(request => request.Text)
            .NotEmpty()
            .MaximumLength(480)
            .WithErrorCode(nameof(SmsSendRequest.Text));
    }    
}