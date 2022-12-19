using DomainServices.DocumentArchiveService.Contracts;
using FluentValidation;

namespace DomainServices.DocumentArchiveService.Api.Endpoints.GenerateDocumentId;

internal sealed class GenerateDocumentIdValidator
    : AbstractValidator<GenerateDocumentIdRequest>
{
    public GenerateDocumentIdValidator()
    {
        RuleFor(t => t)
            .NotEmpty();

        When(t => t is not null, () =>
        {
            RuleFor(t => t.EnvironmentName)
                .IsInEnum()
                .NotEqual(EnvironmentNames.Unknown)
                .WithMessage("Unknown EnvironmentName.").WithErrorCode("14009");

            RuleFor(t => t.EnvironmentIndex)
                .LessThanOrEqualTo(9)
                .WithMessage("Unknown EnvironmentIndex.").WithErrorCode("14010");
        });
    }
}
