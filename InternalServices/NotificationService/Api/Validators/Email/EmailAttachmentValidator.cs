using CIS.InternalServices.NotificationService.Contracts.Email.Dto;
using FluentValidation;
using NOBY.Infrastructure.ErrorHandling;

namespace CIS.InternalServices.NotificationService.Api.Validators.Email;

public class EmailAttachmentValidator : AbstractValidator<EmailAttachment>
{
    public EmailAttachmentValidator()
    {
        RuleFor(attachment => attachment.Binary)
            .NotEmpty()
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.BinaryRequired)
            .Matches("^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)?$")
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.BinaryInvalid);
                
        RuleFor(attachment => attachment.Filename)
            .NotEmpty()
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.FilenameRequired)
            .MaximumLength(255)
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.FilenameLengthLimitExceeded);
    }
}