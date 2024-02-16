using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v2.SendEmail.Validators;

internal sealed class EmailAttachmentValidator 
    : AbstractValidator<Contracts.v2.SendEmailRequest.Types.EmailAttachment>
{
    public EmailAttachmentValidator()
    {
        RuleFor(attachment => attachment.Binary)
            .NotEmpty()
            .WithErrorCode(ErrorCodeMapper.BinaryRequired);
                
        RuleFor(attachment => attachment.Filename)
            .NotEmpty()
                .WithErrorCode(ErrorCodeMapper.FilenameRequired)
            .MaximumLength(255)
                .WithErrorCode(ErrorCodeMapper.FilenameLengthLimitExceeded);
    }
}