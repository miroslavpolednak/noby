﻿using CIS.InternalServices.NotificationService.Contracts.Email;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators.Email;

public class SendEmailRequestValidator : AbstractValidator<SendEmailRequest>
{
    public SendEmailRequestValidator()
    {
        RuleFor(request => request.From)
            .NotNull()
                .WithErrorCode(ErrorCodes.SendEmail.FromRequired)
                .WithMessage($"{nameof(SendEmailRequest.From)} required.")
            .SetValidator(new EmailAddressFromValidator())
                .WithErrorCode(ErrorCodes.SendEmail.FromInvalid)
                .WithMessage($"Invalid {nameof(SendEmailRequest.From)}.");
        
        RuleFor(request => request.To)
            .NotEmpty()
                .WithErrorCode(ErrorCodes.SendEmail.ToNotEmpty)
                .WithMessage($"{nameof(SendEmailRequest.To)} must be not empty.")
            .ForEach(to => to.SetValidator(new EmailAddressValidator()))
                .WithErrorCode(ErrorCodes.SendEmail.ToInvalid)
                .WithMessage($"Invalid {nameof(SendEmailRequest.To)}.");

        RuleForEach(request => request.Bcc)
            .SetValidator(new EmailAddressValidator())
                .WithErrorCode(ErrorCodes.SendEmail.BccInvalid)
                .WithMessage($"Invalid {nameof(SendEmailRequest.Bcc)}.");

        RuleForEach(request => request.Cc)
            .SetValidator(new EmailAddressValidator())
                .WithErrorCode(ErrorCodes.SendEmail.CcInvalid)
                .WithMessage($"Invalid {nameof(SendEmailRequest.Cc)}.");

        When(request => request.ReplyTo is not null, () =>
        {
            RuleFor(request => request.ReplyTo!)
                .SetValidator(new EmailAddressValidator())
                    .WithErrorCode(ErrorCodes.SendEmail.ReplyToInvalid)
                    .WithMessage($"Invalid {nameof(SendEmailRequest.ReplyTo)}.");
        });

        RuleFor(request => request.Subject)
            .NotEmpty()
                .WithErrorCode(ErrorCodes.SendEmail.SubjectRequired)
                .WithMessage($"{nameof(SendEmailRequest.Subject)} required.")
            .MaximumLength(400)
                .WithErrorCode(ErrorCodes.SendEmail.SubjectInvalid)
                .WithMessage($"Invalid {nameof(SendEmailRequest.Subject)}.");

        RuleFor(request => request.Content)
            .NotEmpty()
                .WithErrorCode(ErrorCodes.SendEmail.ContentRequired)
                .WithMessage($"{nameof(SendEmailRequest.Content)} required.")
            .SetValidator(new EmailContentValidator())
                .WithErrorCode(ErrorCodes.SendEmail.ContentInvalid)
                .WithMessage($"Invalid {nameof(SendEmailRequest.Content)}.");

        RuleForEach(request => request.Attachments)
            .SetValidator(new EmailAttachmentValidator())
                .WithErrorCode(ErrorCodes.SendEmail.AttachmentsInvalid)
                .WithMessage($"Invalid {nameof(SendEmailRequest.Attachments)}.");
    }
}