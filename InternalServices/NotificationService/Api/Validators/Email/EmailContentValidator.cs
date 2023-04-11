using CIS.InternalServices.NotificationService.Api.Configuration;
using CIS.InternalServices.NotificationService.Contracts.Email.Dto;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace CIS.InternalServices.NotificationService.Api.Validators.Email;

public class EmailContentValidator : AbstractValidator<EmailContent>
{
    public EmailContentValidator(IOptions<AppConfiguration> options)
    {
        var formats = options.Value.EmailFormats;
        RuleFor(content => content.Format)
            .NotEmpty()
                .WithErrorCode(ErrorCodes.Validation.EmailContent.FormatRequired)
                .WithMessage($"{nameof(EmailContent.Format)} required.")
            .Must(format => formats.Contains(format))
                .WithErrorCode(ErrorCodes.Validation.EmailContent.FormatInvalid)
                .WithMessage($"Allowed values for {nameof(EmailContent.Format)}: {string.Join(',', formats)}.");

        var languageCodes = options.Value.EmailLanguageCodes;
        RuleFor(content => content.Language)
            .NotEmpty()
                .WithErrorCode(ErrorCodes.Validation.EmailContent.LanguageRequired)
                .WithMessage($"{nameof(EmailContent.Language)} required.")
            .Must(language => languageCodes.Contains(language))
                .WithErrorCode(ErrorCodes.Validation.EmailContent.LanguageInvalid)
                .WithMessage($"Allowed values for {nameof(EmailContent.Language)}: {string.Join(',', languageCodes)}.");
                
        RuleFor(content => content.Text)
            .NotEmpty()
                .WithErrorCode(ErrorCodes.Validation.EmailContent.TextRequired)
                .WithMessage($"{nameof(EmailContent.Text)} required.");
    }
}