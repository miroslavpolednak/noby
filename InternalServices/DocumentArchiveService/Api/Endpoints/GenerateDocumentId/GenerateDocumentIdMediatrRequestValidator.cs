using FluentValidation;

namespace CIS.InternalServices.DocumentArchiveService.Api.Endpoints.GenerateDocumentId;

internal sealed class GenerateDocumentIdMediatrRequestValidator
    : AbstractValidator<GenerateDocumentIdMediatrRequest>
{
    public GenerateDocumentIdMediatrRequestValidator()
    {
        RuleFor(t => t.Request)
            .NotEmpty();

        When(t => t.Request is not null, () =>
        {
            RuleFor(t => t.Request.EnvironmentIndex)
                .LessThanOrEqualTo(9)
                .WithMessage("Environment Index must be less than 10").WithErrorCode("502");
        });
    }
}
