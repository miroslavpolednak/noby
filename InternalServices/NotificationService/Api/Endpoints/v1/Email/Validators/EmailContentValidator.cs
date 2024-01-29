using CIS.Infrastructure.CisMediatR.GrpcValidation;
using CIS.InternalServices.NotificationService.Api.Configuration;
using CIS.InternalServices.NotificationService.LegacyContracts.Email.Dto;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v1.Email.Validators;

internal sealed class EmailContentValidator : AbstractValidator<EmailContent>
{
    // todo: options move to error code mapper => WithErrorCode(int param)
    public EmailContentValidator(IOptions<AppConfiguration> options)
    {
        var formats = options.Value.EmailFormats;
        RuleFor(content => content.Format)
            .NotEmpty()
                .WithErrorCode(ErrorCodeMapper.FormatRequired)
            .Must(format => formats.Contains(format))
                .WithErrorCode($"{ErrorCodeMapper.FormatInvalid}")
                .WithMessage($"Allowed values for {nameof(EmailContent.Format)}: {string.Join(", ", formats)}.");

        var languageCodes = options.Value.EmailLanguageCodes;
        RuleFor(content => content.Language)
            .NotEmpty()
                .WithErrorCode(ErrorCodeMapper.LanguageRequired)
            .Must(language => languageCodes.Contains(language))
                .WithErrorCode($"{ErrorCodeMapper.LanguageInvalid}")
                .WithMessage($"Allowed values for {nameof(EmailContent.Language)}: {string.Join(", ", languageCodes)}.");
                
        RuleFor(content => content.Text)
            .NotEmpty()
                .WithErrorCode(ErrorCodeMapper.EmailTextRequired);
    }
}