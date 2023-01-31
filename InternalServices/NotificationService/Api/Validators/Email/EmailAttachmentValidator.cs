using CIS.InternalServices.NotificationService.Contracts.Email.Dto;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators.Email;

public class EmailAttachmentValidator : AbstractValidator<EmailAttachment>
{
    public EmailAttachmentValidator()
    {
        RuleFor(attachment => attachment.Binary)
            .NotEmpty()
                .WithErrorCode(ErrorCodes.EmailAttachment.BinaryRequired)
                .WithMessage($"{nameof(EmailAttachment.Binary)} required.")
            .Matches("^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)?$")
                .WithErrorCode(ErrorCodes.EmailAttachment.BinaryInvalid)
                .WithMessage($"{nameof(EmailAttachment.Binary)} must be encoded in Base64.");
                
        RuleFor(attachment => attachment.Filename)
            .NotEmpty()
                .WithErrorCode(ErrorCodes.EmailAttachment.FilenameRequired)
                .WithMessage($"{nameof(EmailAttachment.Filename)} required.")
            .MaximumLength(255)
                .WithErrorCode(ErrorCodes.EmailAttachment.FilenameLengthLimitExceeded)
                .WithMessage($"Maximum length of {nameof(EmailAttachment.Filename)} is 255.");
    }
}