using CIS.InternalServices.NotificationService.Contracts.Email;
using CIS.InternalServices.NotificationService.Contracts.Email.Dto;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.Notification.SendEmail;

public class SendEmailRequestValidator : AbstractValidator<EmailSendRequest>
{
    public SendEmailRequestValidator()
    {
        RuleFor(request => request.From)
            .NotEmpty()
            .WithErrorCode(nameof(EmailSendRequest.From));
        
        RuleFor(request => request.To)
            .NotEmpty()
            .WithErrorCode(nameof(EmailSendRequest.To));
        
        RuleForEach(request => request.Bcc)
            .MaximumLength(255)
            .WithErrorCode(nameof(EmailSendRequest.Bcc));
        
        RuleForEach(request => request.Cc)
            .MaximumLength(255)
            .WithErrorCode(nameof(EmailSendRequest.Cc));
        
        RuleFor(request => request.ReplyTo)
            .MaximumLength(255)
            .WithErrorCode(nameof(EmailSendRequest.ReplyTo));
        
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