using CIS.InternalServices.NotificationService.Contracts.Email.Dto;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators;

public class EmailAttachmentValidator : AbstractValidator<EmailAttachment>
{
    public EmailAttachmentValidator()
    {
        RuleFor(attachment => attachment.Binary)
            .NotEmpty()
            .WithErrorCode(nameof(EmailAttachment.Binary));
                
        RuleFor(attachment => attachment.Filename)
            .NotEmpty()
            .MaximumLength(255)
            .WithErrorCode(nameof(EmailAttachment.Filename));
    }
}