using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.BackgroundServices.Cleaning;

internal sealed class CleaningJobConfigurationValidator
    : AbstractValidator<CleaningJobConfiguration>
{
    public CleaningJobConfigurationValidator()
    {
        RuleFor(t => t.CleanInProgressInMinutes)
            .NotEmpty();

        RuleFor(t => t.EmailSlaInMinutes)
            .NotEmpty();
    }
}
