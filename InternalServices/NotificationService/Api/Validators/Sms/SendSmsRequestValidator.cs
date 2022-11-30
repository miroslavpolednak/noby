using CIS.InternalServices.NotificationService.Contracts.Sms;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators.Sms;

public class SendSmsRequestValidator : AbstractValidator<SmsSendRequest>
{
    public SendSmsRequestValidator()
    {
        RuleFor(request => request.Phone)
            .NotNull()
                .WithErrorCode("todo")
                .WithMessage("todo")
            .SetValidator(new PhoneValidator())
                .WithErrorCode(nameof(SmsSendRequest.Phone))
                .WithMessage("todo");

        RuleFor(request => request.ProcessingPriority)
            .GreaterThan(0)
                .WithErrorCode(nameof(SmsSendRequest.ProcessingPriority))
                .WithMessage("todo");
        
        RuleFor(request => request.Type)
            .NotEmpty()
                .WithErrorCode(nameof(SmsSendRequest.Type))
                .WithMessage("todo");
        
        RuleFor(request => request.Text)
            .NotEmpty()
                .WithErrorCode("todo")
                .WithMessage("todo")
            .MaximumLength(480)
                .WithErrorCode(nameof(SmsSendRequest.Text))
                .WithMessage("todo");
    }    
}