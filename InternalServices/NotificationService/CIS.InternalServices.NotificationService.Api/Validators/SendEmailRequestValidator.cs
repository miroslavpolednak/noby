using CIS.InternalServices.NotificationService.Contracts.Email;
using CIS.InternalServices.NotificationService.Contracts.Email.Dto;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators;

public class SendEmailRequestValidator : AbstractValidator<EmailSendRequest>
{
    public SendEmailRequestValidator()
    {
        RuleFor(request => request.From)
            .NotNull()
            .SetValidator(new EmailAddressValidator())
            .WithErrorCode(nameof(EmailSendRequest.From));
        
        RuleFor(request => request.To)
            .NotEmpty()
            .ForEach(to =>
                to.SetValidator(new EmailAddressValidator()))
            .WithErrorCode(nameof(EmailSendRequest.To));

        RuleForEach(request => request.Bcc)
            .SetValidator(new EmailAddressValidator())
            .WithErrorCode(nameof(EmailSendRequest.Bcc));

        RuleForEach(request => request.Cc)
            .SetValidator(new EmailAddressValidator())
            .WithErrorCode(nameof(EmailSendRequest.Bcc));

        When(request => request.ReplyTo is not null, () =>
        {
            RuleFor(request => request.ReplyTo!)
                .SetValidator(new EmailAddressValidator())
                .WithErrorCode(nameof(EmailSendRequest.Bcc));
        });

        RuleFor(request => request.Subject)
            .NotEmpty()
            .MaximumLength(400)
            .WithErrorCode(nameof(EmailSendRequest.Subject));
        
        RuleFor(request => request.Content)
            .NotEmpty()
            .WithErrorCode(nameof(EmailSendRequest.Content))
            .ChildRules(content =>
            {
                content.RuleFor(c => c.Format)
                    .NotEmpty()
                    .WithErrorCode($"{nameof(EmailSendRequest.Content)}.{nameof(EmailContent.Format)}");
                
                content.RuleFor(c => c.Language)
                    .NotEmpty()
                    .WithErrorCode($"{nameof(EmailSendRequest.Content)}.{nameof(EmailContent.Language)}");
                
                content.RuleFor(c => c.Text)
                    .NotEmpty()
                    .WithErrorCode($"{nameof(EmailSendRequest.Content)}.{nameof(EmailContent.Text)}");
            });
        
        RuleForEach(request => request.Attachments)
            .ChildRules(attachment =>
            {
                attachment.RuleFor(a => a.Binary)
                    .NotEmpty()
                    .WithErrorCode($"{nameof(EmailSendRequest.Attachments)}.{nameof(EmailAttachment.Binary)}");
                
                attachment.RuleFor(a => a.Filename)
                    .NotEmpty()
                    .MaximumLength(255)
                    .WithErrorCode($"{nameof(EmailSendRequest.Attachments)}.{nameof(EmailAttachment.Filename)}");
            });
    }
}