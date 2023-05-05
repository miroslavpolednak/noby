using CIS.InternalServices.NotificationService.Api.Configuration;
using CIS.InternalServices.NotificationService.Api.Validators.Common;
using CIS.InternalServices.NotificationService.Contracts.Email;
using CIS.InternalServices.NotificationService.Contracts.Sms;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace CIS.InternalServices.NotificationService.Api.Validators.Email;

public class SendEmailRequestValidator : AbstractValidator<SendEmailRequest>
{
    public SendEmailRequestValidator(IOptions<AppConfiguration> options)
    {
        RuleFor(request => request.From)
            .NotNull()
                .WithErrorCode(ErrorCodes.Validation.SendEmail.FromRequired)
                .WithMessage($"{nameof(SendEmailRequest.From)} required.")
            .SetValidator(new EmailAddressFromValidator(options))
                .WithErrorCode(ErrorCodes.Validation.SendEmail.FromInvalid)
                .WithMessage($"Invalid {nameof(SendEmailRequest.From)}.");
        
        RuleFor(request => request.To)
            .NotEmpty()
                .WithErrorCode(ErrorCodes.Validation.SendEmail.ToNotEmpty)
                .WithMessage($"{nameof(SendEmailRequest.To)} must be not empty.")
            .ForEach(to => to.SetValidator(new EmailAddressValidator()))
                .WithErrorCode(ErrorCodes.Validation.SendEmail.ToInvalid)
                .WithMessage($"Invalid {nameof(SendEmailRequest.To)}.");

        RuleForEach(request => request.Bcc)
            .SetValidator(new EmailAddressValidator())
                .WithErrorCode(ErrorCodes.Validation.SendEmail.BccInvalid)
                .WithMessage($"Invalid {nameof(SendEmailRequest.Bcc)}.");

        RuleForEach(request => request.Cc)
            .SetValidator(new EmailAddressValidator())
                .WithErrorCode(ErrorCodes.Validation.SendEmail.CcInvalid)
                .WithMessage($"Invalid {nameof(SendEmailRequest.Cc)}.");

        When(request => request.ReplyTo is not null, () =>
        {
            RuleFor(request => request.ReplyTo!)
                .SetValidator(new EmailAddressValidator())
                    .WithErrorCode(ErrorCodes.Validation.SendEmail.ReplyToInvalid)
                    .WithMessage($"Invalid {nameof(SendEmailRequest.ReplyTo)}.");
        });

        RuleFor(request => request.Subject)
            .NotEmpty()
                .WithErrorCode(ErrorCodes.Validation.SendEmail.SubjectRequired)
                .WithMessage($"{nameof(SendEmailRequest.Subject)} required.")
            .MaximumLength(400)
                .WithErrorCode(ErrorCodes.Validation.SendEmail.SubjectInvalid)
                .WithMessage($"Invalid {nameof(SendEmailRequest.Subject)}.");

        RuleFor(request => request.Content)
            .NotEmpty()
                .WithErrorCode(ErrorCodes.Validation.SendEmail.ContentRequired)
                .WithMessage($"{nameof(SendEmailRequest.Content)} required.")
            .SetValidator(new EmailContentValidator(options))
                .WithErrorCode(ErrorCodes.Validation.SendEmail.ContentInvalid)
                .WithMessage($"Invalid {nameof(SendEmailRequest.Content)}.");

        RuleFor(request => request.Attachments.Count)
            .LessThanOrEqualTo(10)
                .WithErrorCode(ErrorCodes.Validation.SendEmail.AttachmentsCountLimitExceeded)
                .WithMessage($"Maximum count of {nameof(SendEmailRequest.Attachments)} is 10.");
        
        RuleForEach(request => request.Attachments)
            .SetValidator(new EmailAttachmentValidator())
                .WithErrorCode(ErrorCodes.Validation.SendEmail.AttachmentsInvalid)
                .WithMessage($"Invalid {nameof(SendEmailRequest.Attachments)}.");
        
        When(request => request.Identifier is not null, () =>
        {
            RuleFor(request => request.Identifier!)
                .SetValidator(new IdentifierValidator())
                .WithMessage($"Invalid {nameof(SendEmailRequest.Identifier)}.");
        });
    }
}