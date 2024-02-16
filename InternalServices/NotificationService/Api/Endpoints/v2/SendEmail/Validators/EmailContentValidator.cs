using CIS.InternalServices.NotificationService.Api.Configuration;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v2.SendEmail.Validators;

internal sealed class EmailContentValidator 
    : AbstractValidator<Contracts.v2.SendEmailRequest.Types.EmailContent>
{
    // todo: options move to error code mapper => WithErrorCode(int param)
    public EmailContentValidator(AppConfiguration appConfiguration)
    {
        RuleFor(content => content.Format)
            .NotEmpty()
                .WithErrorCode(ErrorCodeMapper.FormatRequired)
            .Must(format => appConfiguration.EmailFormats.Contains(format))
                .WithErrorCode($"{ErrorCodeMapper.FormatInvalid}")
                .WithMessage($"Allowed values for {nameof(Contracts.v2.SendEmailRequest.Types.EmailContent.Format)}: {string.Join(", ", appConfiguration.EmailFormats)}.");

        RuleFor(content => content.Language)
            .NotEmpty()
                .WithErrorCode(ErrorCodeMapper.LanguageRequired)
            .Must(language => appConfiguration.EmailLanguageCodes.Contains(language))
                .WithErrorCode($"{ErrorCodeMapper.LanguageInvalid}")
                .WithMessage($"Allowed values for {nameof(Contracts.v2.SendEmailRequest.Types.EmailContent.Language)}: {string.Join(", ", appConfiguration.EmailLanguageCodes)}.");
                
        RuleFor(content => content.Text)
            .NotEmpty()
                .WithErrorCode(ErrorCodeMapper.EmailTextRequired);
    }
}