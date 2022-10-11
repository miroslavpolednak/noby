using CIS.InternalServices.NotificationService.Contracts.Email;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators;

public class SendEmailRequestValidator : AbstractValidator<EmailSendRequest>
{
    public SendEmailRequestValidator()
    {
        RuleFor(request => request.From)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .SetValidator(new EmailAddressValidator())
            .WithErrorCode(nameof(EmailSendRequest.From));
        
        RuleFor(request => request.To)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .ForEach(to =>
                to.SetValidator(new EmailAddressValidator()))
            .WithErrorCode(nameof(EmailSendRequest.To));

        RuleForEach(request => request.Bcc)
            .Cascade(CascadeMode.Stop)
            .SetValidator(new EmailAddressValidator())
            .WithErrorCode(nameof(EmailSendRequest.Bcc));

        RuleForEach(request => request.Cc)
            .Cascade(CascadeMode.Stop)
            .SetValidator(new EmailAddressValidator())
            .WithErrorCode(nameof(EmailSendRequest.Bcc));

        When(request => request.ReplyTo is not null, () =>
        {
            RuleFor(request => request.ReplyTo!)
                .Cascade(CascadeMode.Stop)
                .SetValidator(new EmailAddressValidator())
                .WithErrorCode(nameof(EmailSendRequest.Bcc));
        });

        RuleFor(request => request.Subject)
            .NotEmpty()
            .MaximumLength(400)
            .WithErrorCode(nameof(EmailSendRequest.Subject));

        RuleFor(request => request.Content)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .SetValidator(new EmailContentValidator())
            .WithErrorCode(nameof(EmailSendRequest.Content));

        RuleForEach(request => request.Attachments)
            .SetValidator(new EmailAttachmentValidator())
            .WithErrorCode(nameof(EmailSendRequest.Attachments));
    }
}