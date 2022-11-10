using DomainServices.DocumentArchiveService.Contracts;
using FluentValidation;

namespace DomainServices.DocumentArchiveService.Api.Endpoints.GenerateDocumentId;

internal sealed class GenerateDocumentIdMediatrRequestValidator
    : AbstractValidator<GenerateDocumentIdMediatrRequest>
{
    public GenerateDocumentIdMediatrRequestValidator()
    {
        RuleFor(t => t.Request)
            .NotEmpty();

        When(t => t.Request is not null, () =>
        {
            RuleFor(t => t.Request.EnvironmentName)
                .IsInEnum()
                .NotEqual(EnvironmentNames.Unknown)
                .WithMessage("Unknown EnvironmentName.").WithErrorCode("14009");

            RuleFor(t => t.Request.EnvironmentIndex)
                .LessThanOrEqualTo(9)
                .WithMessage("Unknown EnvironmentIndex.").WithErrorCode("14010");
        });
    }
}
