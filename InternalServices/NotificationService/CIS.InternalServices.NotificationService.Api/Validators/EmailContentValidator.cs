using CIS.InternalServices.NotificationService.Contracts.Email.Dto;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators;

public class EmailContentValidator : AbstractValidator<EmailContent>
{
    public EmailContentValidator()
    {
        RuleFor(content => content.Format)
            .NotEmpty()
            .WithErrorCode(nameof(EmailContent.Format));
                
        RuleFor(content => content.Language)
            .NotEmpty()
            .WithErrorCode(nameof(EmailContent.Language));
                
        RuleFor(content => content.Text)
            .NotEmpty()
            .WithErrorCode(nameof(EmailContent.Text));
    }
}