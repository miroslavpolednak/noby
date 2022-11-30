using CIS.InternalServices.NotificationService.Contracts.Email;
using Confluent.Kafka;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators.Email;

public class SendEmailRequestValidator : AbstractValidator<EmailSendRequest>
{
    public SendEmailRequestValidator()
    {
        RuleFor(request => request.From)
            .NotNull()
                .WithErrorCode(ErrorCodes.SendEmail.FromRequired)
                .WithMessage($"{nameof(EmailSendRequest.From)} required.")
            .SetValidator(new EmailAddressFromValidator())
                .WithErrorCode(ErrorCodes.SendEmail.FromInvalid)
                .WithMessage($"Invalid {nameof(EmailSendRequest.From)}.");
        
        RuleFor(request => request.To)
            .NotEmpty()
                .WithErrorCode(ErrorCodes.SendEmail.ToNotEmpty)
                .WithMessage($"{nameof(EmailSendRequest.To)} must be not empty.")
            .ForEach(to => to.SetValidator(new EmailAddressValidator()))
                .WithErrorCode(ErrorCodes.SendEmail.ToInvalid)
                .WithMessage($"Invalid {nameof(EmailSendRequest.To)}.");

        RuleForEach(request => request.Bcc)
            .SetValidator(new EmailAddressValidator())
                .WithErrorCode(ErrorCodes.SendEmail.BccInvalid)
                .WithMessage($"Invalid {nameof(EmailSendRequest.Bcc)}.");

        RuleForEach(request => request.Cc)
            .SetValidator(new EmailAddressValidator())
                .WithErrorCode(ErrorCodes.SendEmail.CcInvalid)
                .WithMessage($"Invalid {nameof(EmailSendRequest.Cc)}.");

        When(request => request.ReplyTo is not null, () =>
        {
            RuleFor(request => request.ReplyTo!)
                .SetValidator(new EmailAddressValidator())
                    .WithErrorCode(ErrorCodes.SendEmail.ReplyToInvalid)
                    .WithMessage($"Invalid {nameof(EmailSendRequest.ReplyTo)}.");
        });

        RuleFor(request => request.Subject)
            .NotEmpty()
                .WithErrorCode(ErrorCodes.SendEmail.SubjectRequired)
                .WithMessage($"{nameof(EmailSendRequest.Subject)} required.")
            .MaximumLength(400)
                .WithErrorCode(ErrorCodes.SendEmail.SubjectInvalid)
                .WithMessage($"Invalid {nameof(EmailSendRequest.Subject)}.");

        RuleFor(request => request.Content)
            .NotEmpty()
                .WithErrorCode(ErrorCodes.SendEmail.ContentRequired)
                .WithMessage($"{nameof(EmailSendRequest.Content)} required.")
            .SetValidator(new EmailContentValidator())
                .WithErrorCode(ErrorCodes.SendEmail.ContentInvalid)
                .WithMessage($"Invalid {nameof(EmailSendRequest.Content)}.");

        RuleForEach(request => request.Attachments)
            .SetValidator(new EmailAttachmentValidator())
                .WithErrorCode(ErrorCodes.SendEmail.AttachmentsInvalid)
                .WithMessage($"Invalid {nameof(EmailSendRequest.Attachments)}.");
    }
}