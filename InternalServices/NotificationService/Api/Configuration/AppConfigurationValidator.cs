using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Configuration;

internal sealed class AppConfigurationValidator
    : AbstractValidator<AppConfiguration>
{
    public AppConfigurationValidator()
    {
        RuleFor(t => t.Consumers)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .ForEach(t =>
            {
                t.ChildRules(tt =>
                {
                    tt.RuleFor(r => r.Username).NotEmpty();
                    tt.RuleFor(r => r.ConsumerId).NotEmpty();
                });
            });

        RuleFor(t => t.EmailSenders)
            .NotNull()
            .ChildRules(t =>
            {
                t.RuleFor(tt => tt.Mcs).NotEmpty();
                t.RuleFor(tt => tt.Mpss).NotEmpty();
            });

        RuleFor(t => t.EmailFormats)
            .NotEmpty();

        RuleFor(t => t.EmailLanguageCodes)
            .NotEmpty();
    }
}
