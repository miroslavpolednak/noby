using CIS.InternalServices.NotificationService.Contracts.Email.Dto;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators.Email;

public class EmailContentValidator : AbstractValidator<EmailContent>
{
    public EmailContentValidator()
    {
        RuleFor(content => content.Format)
            .NotEmpty()
                .WithErrorCode(ErrorCodes.EmailContent.FormatRequired)
                .WithMessage($"{nameof(EmailContent.Format)} required.");
                
        RuleFor(content => content.Language)
            .NotEmpty()
                .WithErrorCode(ErrorCodes.EmailContent.LanguageRequired)
                .WithMessage($"{nameof(EmailContent.Language)} required.");
                
        RuleFor(content => content.Text)
            .NotEmpty()
                .WithErrorCode(ErrorCodes.EmailContent.TextRequired)
                .WithMessage($"{nameof(EmailContent.Text)} required.");
    }
}